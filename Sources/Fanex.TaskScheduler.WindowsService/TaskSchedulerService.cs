using Fanex.MailDelivery.Client;
using System;
using System.IO;

namespace Fanex.TaskScheduler.WindowsService
{
    public class TaskSchedulerService
    {
        public void Start()
        {
            try
            {
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                MailClientHandler.Start();
            }
            catch (Exception e)
            {
                File.WriteAllText("/log.txt", e.Message);
            }
        }

        public void Stop()
        {
            MailClientHandler.Stop();
        }
    }
}