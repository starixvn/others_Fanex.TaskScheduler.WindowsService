using Fanex.MailDelivery.Client;
using System.Timers;

namespace Fanex.TaskScheduler.WindowsService
{
    public class TaskSchedulerService
    {
        private const int Milliseconds = 1000;
        private const int Interval = 60;
        private Timer timer;
        private bool isOnProcessing;

        public void Start()
        {
            Register();
            timer.Start();
            MailClientHandler.Start();
        }

        public void Stop()
        {
            timer.Stop();
            MailClientHandler.Stop();
        }

        private void Register()
        {
            timer = new Timer();
            timer.Interval = Interval * Milliseconds;
            timer.Elapsed += (sender, e) =>
            {
                if (!isOnProcessing)
                {
                    Execute();
                }
            };
        }

        private void Execute()
        {
            isOnProcessing = true;

            isOnProcessing = false;
        }
    }
}