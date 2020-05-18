using Topshelf;

namespace Fanex.TaskScheduler.WindowsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TaskSchedulerService>(s =>
                {
                    s.ConstructUsing(name => new TaskSchedulerService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.SetDescription("Fanex.TaskScheduler.WindowsService Host");
                x.SetDisplayName("Fanex.TaskScheduler.WindowsService");
                x.SetServiceName("Fanex.TaskScheduler.WindowsService");
            });
        }
    }
}