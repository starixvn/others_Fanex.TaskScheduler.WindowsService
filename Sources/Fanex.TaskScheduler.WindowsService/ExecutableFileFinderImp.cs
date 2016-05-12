using Fanex.TaskScheduler.WindowsService.Configuration;
using System.Linq;

namespace Fanex.TaskScheduler.WindowsService
{
    public class ExecutableFileFinderImp : ExecutableFileFinder
    {
        public string GetExecutableFile(int taskId)
        {
            return TaskSchedulerSettings
                .Instance
                .Tasks
                .Cast<TaskElement>()
                .Single(t => t.Id == taskId)
                .Executable;
        }
    }
}
