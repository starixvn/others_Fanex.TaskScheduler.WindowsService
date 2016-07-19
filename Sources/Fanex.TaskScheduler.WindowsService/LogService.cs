using System;

namespace Fanex.TaskScheduler.WindowsService
{
    public interface LogService
    {
        void Log(Exception exception);

        void Log(string message);
    }
}