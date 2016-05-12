using System.Configuration;

namespace Fanex.TaskScheduler.WindowsService.Configuration
{
    [ConfigurationCollection(typeof(TaskElement), AddItemName = "Task")]
    public class TaskCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        public TaskElement this[int index]
        {
            get
            {
                return (TaskElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TaskElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TaskElement)element).Id;
        }
    }
}