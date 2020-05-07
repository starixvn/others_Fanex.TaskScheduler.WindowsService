using Fanex.Data;
using Fanex.MailDelivery.Client;
using ONEbook.UM.Services;
using System.Configuration;
using System.Net;
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
            RegisterService.ConfigureFanexData();
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
            var um = RegisterService.Container.GetInstance<IUMControlAdapter>();
            if (!um.GetUnderMaintenanceTime().IsUnderMaintenanceTime)
            {
                CheckIn();
            }

            isOnProcessing = false;
        }

        private void CheckIn()
        {
            var serviceName = ConfigurationManager.AppSettings["ServiceName"];
            var criteria = new ServiceMonitoringCheckInCriteria
            {
                Interval = Interval,
                IPServer = GetLocalIP(),
                OrderBy = 2,
                ServiceName = serviceName
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        private string GetLocalIP()
        {
            var ip = string.Empty;
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ip = addr.ToString();
                }
            }

            return ip;
        }
    }
}