using System;

namespace Fanex.TaskScheduler.WindowsService
{
    public class LogServiceImp : LogService
    {
        public void Log(Exception exception)
        {
            Fanex.Logging.LogService.Error(exception.Message, exception);
        }
    }
}
