using Fanex.TaskScheduler.WindowsService.Configuration;
using NUnit.Framework;
using System.Linq;

namespace Fanex.TaskScheduler.WindowsService.Tests
{
    public class TaskSchedulerSettingsTests
    {
        [Test]
        public void TaskSchedulerSettings_Is_Loaded_Successfully()
        {
            var taskSchedulerSettings = TaskSchedulerSettings.Instance;

            var tasks = taskSchedulerSettings.Tasks.Cast<TaskElement>().ToList();
            Assert.AreEqual(1, tasks.Count);

            var task = tasks[0];
            Assert.That(task, Has.Property("Id").EqualTo(1));
            Assert.That(task, Has.Property("Executable").EqualTo(@"D:\Test.exe"));
        }
    }
}
