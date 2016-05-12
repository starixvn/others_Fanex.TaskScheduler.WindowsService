namespace Fanex.TaskScheduler.WindowsService
{
    public interface ExecutableFileFinder
    {
        string GetExecutableFile(int taskId);
    }
}