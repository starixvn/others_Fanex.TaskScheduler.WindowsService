using System.Configuration;

namespace Fanex.TaskScheduler.WindowsService.Configuration
{
    public class TaskElement : ConfigurationElement
    {
        [ConfigurationProperty("Id", IsRequired = true)]
        public int Id => (int)this["Id"];

        [ConfigurationProperty("Executable", IsRequired = true)]
        public string Executable => (string)this["Executable"];
    }
}
