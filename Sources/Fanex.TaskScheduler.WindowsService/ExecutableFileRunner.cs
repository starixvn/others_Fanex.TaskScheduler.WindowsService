using Fanex.MailDelivery.Core.Models;

namespace Fanex.TaskScheduler.WindowsService
{
    public interface ExecutableFileRunner
    {
        JobMessage Execute(string executableFile);
    }
}