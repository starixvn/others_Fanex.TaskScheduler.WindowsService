using Fanex.Data;
using Fanex.Data.OldConnectionStringProvider;
using SimpleInjector;
using System;
using System.Configuration;

namespace Fanex.TaskScheduler.WindowsService
{
    public static class RegisterService
    {
        static RegisterService()
        {
            Container = new Container();

            Register();
        }

        public static Container Container { get; set; }

        public static void Register()
        {
            Container.Register<LogService, LogServiceImp>(Lifestyle.Transient);
            Container.Register<ExecutableFileRunner, ExecutableFileRunnerImp>(Lifestyle.Transient);
            Container.Register<ExecutableFileFinder, ExecutableFileFinderImp>(Lifestyle.Transient);
            Container.Register(() => new TaskSchedulerService());

            try
            {
                OnebookUMInitializer.InitializeUMClientManager();
                OnebookUMInitializer.RegisterSimpleInjector(Container);
            }
            catch (Exception ex)
            {
                Fanex.Logging.LogService.Error("Fanex.TaskScheduler.WindowsService", ex);
            }
        }

        public static void ConfigureFanexData()
        {
            var storedProcedurePath = ConfigurationManager.AppSettings["StoredProcedurePath"];
            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider($@"~/{storedProcedurePath}")
                .Run();
        }
    }
}