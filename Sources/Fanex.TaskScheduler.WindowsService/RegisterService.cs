using System.Configuration;
using Fanex.Data;
using Fanex.Data.OldConnectionStringProvider;
using ONEbook.UM.Services;
using ONEbook.UM.Services.Impl;
using SimpleInjector;

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
            Container.Register<IUMControlAdapter, UMControlAdapter>(Lifestyle.Transient);
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
