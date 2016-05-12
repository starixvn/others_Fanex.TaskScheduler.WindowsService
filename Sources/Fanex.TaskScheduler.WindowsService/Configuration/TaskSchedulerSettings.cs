using System.Configuration;

namespace Fanex.TaskScheduler.WindowsService.Configuration
{
    public class TaskSchedulerSettings : ConfigurationSection
    {
        public static TaskSchedulerSettings Instance => ConfigurationManager.GetSection("TaskScheduler") as TaskSchedulerSettings;

        [ConfigurationProperty("Tasks", IsRequired = true, IsDefaultCollection = true)]
        [ConfigurationCollection(
                   typeof(TaskCollection),
                   AddItemName = "add",
                   ClearItemsName = "clear",
                   RemoveItemName = "remove")]
        public TaskCollection Tasks => base["Tasks"] as TaskCollection;
    }
}
