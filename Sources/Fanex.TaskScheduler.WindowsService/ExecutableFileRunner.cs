using Fanex.MailDelivery.Client.Messages;

namespace Fanex.TaskScheduler.WindowsService
{
    public interface ExecutableFileRunner
    {
        JobResultMessage Execute(string executableFile);
    }
}