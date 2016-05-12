using Fanex.TaskScheduler.WindowsService.Tests.Mocks;
using NUnit.Framework;
using System.IO;

namespace Fanex.TaskScheduler.WindowsService.Tests
{
    public class ExecutionRunnerTests
    {
        [Test]
        public void Execute_SimpleConsole_Successfully()
        {
            var executableFile = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "SimpleConsole.exe");
            var executionRunner = SimpleInjectorContainerTests.Container.GetInstance<ExecutableFileRunner>();
            var result = executionRunner.Execute(executableFile);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void Execute_ErrorConsole_Successfully()
        {
            var executableFile = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "ErrorConsole.exe");
            var executionRunner = SimpleInjectorContainerTests.Container.GetInstance<ExecutableFileRunner>();
            var result = executionRunner.Execute(executableFile);

            Assert.AreNotEqual(0, result);
        }
    }
}
