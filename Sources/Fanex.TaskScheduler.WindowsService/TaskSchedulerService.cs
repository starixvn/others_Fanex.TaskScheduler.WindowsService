using Fanex.MailDelivery.Client;

namespace Fanex.TaskScheduler.WindowsService
{
    public class TaskSchedulerService
    {
        public void Start()
        {
            MailClientHandler.Start();
        }

        public void Stop()
        {
            MailClientHandler.Stop();
        }
    }
}