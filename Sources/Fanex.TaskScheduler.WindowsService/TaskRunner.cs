using Fanex.MailDelivery.Client.Enumerations;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;
using ONEbook.UM.Services;
using Quartz;

namespace Fanex.TaskScheduler.WindowsService
{
    public class TaskRunner : JobBase
    {
        private readonly ExecutableFileRunner executableFileRunner;

        private readonly ExecutableFileFinder executableFileFinder;

        private readonly IMaintenanceService maintenanceService;

        public TaskRunner()
        {
            executableFileRunner = SimpleInjectorContainer.Container.GetInstance<ExecutableFileRunner>();
            executableFileFinder = SimpleInjectorContainer.Container.GetInstance<ExecutableFileFinder>();
            maintenanceService = SimpleInjectorContainer.Container.GetInstance<IMaintenanceService>();
        }

        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            return maintenanceService.IsUnderMaintenance()
                ? RunWhenMaintenance(schedule)
                : RunWhenNoMaintenance(schedule);
        }

        private JobResultMessage RunWhenNoMaintenance(Schedule schedule)
        {
            var executableFile = executableFileFinder.GetExecutableFile(schedule.Id);
            var result = executableFileRunner.Execute(executableFile);

            return new JobResultMessage
            {
                JobStatus = result == 0 ? JobStatus.Success : JobStatus.JobFailed,
                Schedule = schedule
            };
        }

        private JobResultMessage RunWhenMaintenance(Schedule schedule)
        {
            return new JobResultMessage
            {
                JobStatus = JobStatus.JobFailed,
                Schedule = schedule
            };
        }
    }
}
