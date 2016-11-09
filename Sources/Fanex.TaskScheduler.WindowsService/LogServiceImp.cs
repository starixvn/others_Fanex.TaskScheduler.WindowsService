using System;

namespace Fanex.TaskScheduler.WindowsService
{
    public class LogServiceImp : LogService
    {
        public void Log(string message)
        {
            Logging.LogService.Info(message);
        }

        public void Log(Exception exception)
        {
            var message = exception.Message;
            if (!message.Contains("DataIsNotReady"))
            {
                Logging.LogService.Error(exception.Message, exception);
            }
        }
    }
}