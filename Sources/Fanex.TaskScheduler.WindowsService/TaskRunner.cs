using System;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using ONEbook.UM.Services;
using Quartz;

namespace Fanex.TaskScheduler.WindowsService
{
    public class TaskRunner : JobBase
    {
        private readonly ExecutableFileRunner executableFileRunner;

        private readonly ExecutableFileFinder executableFileFinder;

        private readonly IUMControlAdapter maintenanceService;

        public TaskRunner()
        {
            executableFileRunner = SimpleInjectorContainer.Container.GetInstance<ExecutableFileRunner>();
            executableFileFinder = SimpleInjectorContainer.Container.GetInstance<ExecutableFileFinder>();
            maintenanceService = SimpleInjectorContainer.Container.GetInstance<IUMControlAdapter>();
        }

        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            return maintenanceService.GetUnderMaintenanceTime().IsUnderMaintenanceTime
                    ? RunWhenMaintenance(schedule)
                    : RunWhenNoMaintenance(schedule);
        }

        private JobMessage RunWhenNoMaintenance(Schedule schedule)
        {
            var executableFile = executableFileFinder.GetExecutableFile(schedule.Id);
            var result = executableFileRunner.Execute(executableFile);

            return new JobMessage
            {
                JobStatus = result.JobStatus,
                Schedule = schedule,
                Mail = result.Mail
            };
        }

        private JobMessage RunWhenMaintenance(Schedule schedule)
        {
            return new JobMessage
            {
                JobStatus = JobStatus.JobFailed,
                Schedule = schedule
            };
        }
    }
}