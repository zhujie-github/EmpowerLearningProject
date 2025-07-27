using Company.Application.Config;
using Company.Application.Image;
using Company.Application.Main;
using Company.Application.Menu;
using Company.Application.Share.Prism;
using Company.Core;
using Company.Logger;
using System.Windows;

namespace Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            //程序域异常
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                NLogger.Error((Exception)e.ExceptionObject);
            };

            //应用程序异常
            Current.DispatcherUnhandledException += (s, e) =>
            {
                NLogger.Error(e.Exception);
            };

            //多线程异常
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                NLogger.Error(e.Exception);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            NLogger.Info("应用程序开始启动");
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            NLogger.Info("应用程序已经退出");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<ApplicationMainModule>();
            moduleCatalog.AddModule<ApplicationMenuModule>();
            moduleCatalog.AddModule<ApplicationImageModule>();
            moduleCatalog.AddModule<ApplicationConfigModule>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            //使用目录模块
            return new DirectoryModuleCatalog
            {
                ModulePath = ModuleNames.ModulePath
            };
        }
    }
}
