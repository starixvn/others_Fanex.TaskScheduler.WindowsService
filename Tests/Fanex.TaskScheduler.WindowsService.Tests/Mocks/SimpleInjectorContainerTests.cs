using SimpleInjector;

namespace Fanex.TaskScheduler.WindowsService.Tests.Mocks
{
    public static class SimpleInjectorContainerTests
    {
        static SimpleInjectorContainerTests()
        {
            Container = new Container();
            Register();
        }

        public static Container Container { get; set; }

        public static void Register()
        {
            Container.Register<LogService, LogServiceMock>(Lifestyle.Transient);
            Container.Register<ExecutableFileRunner, ExecutableFileRunnerImp>(Lifestyle.Transient);
        }
    }
}
