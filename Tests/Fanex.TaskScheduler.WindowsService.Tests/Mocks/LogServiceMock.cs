using System;

namespace Fanex.TaskScheduler.WindowsService.Tests.Mocks
{
    public class LogServiceMock : LogService
    {
        public void Log(string message)
        {
        }

        public void Log(Exception exception)
        {
        }
    }
}
