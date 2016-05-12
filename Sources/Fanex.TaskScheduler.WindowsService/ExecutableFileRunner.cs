namespace Fanex.TaskScheduler.WindowsService
{
    public interface ExecutableFileRunner
    {
        int Execute(string executableFile);
    }
}