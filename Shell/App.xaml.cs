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
                Logger.Error((Exception)e.ExceptionObject);
            };

            //应用程序异常
            Current.DispatcherUnhandledException += (s, e) =>
            {
                Logger.Error(e.Exception);
            };

            //多线程异常
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Logger.Error(e.Exception);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Logger.Info("应用程序启动");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Logger.Info("应用程序退出");
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
            moduleCatalog.AddModule<Company.Core.CoreModule>();
            moduleCatalog.AddModule<Company.Application.Main.ApplicationMainModule>();
            moduleCatalog.AddModule<Company.Application.Login.ApplicationLoginModule>();
            moduleCatalog.AddModule<Company.Application.Menu.ApplicationMenuModule>();
            moduleCatalog.AddModule<Company.Application.Image.ApplicationImageModule>();
            moduleCatalog.AddModule<Company.Hardware.Camera.None.NoneCameraModule>();
            moduleCatalog.AddModule<Company.Hardware.Detector.None.NoneDetectorModule>();
            moduleCatalog.AddModule<Company.Application.Initialize.ApplicationInitializeModule>();
            moduleCatalog.AddModule<Company.Application.Config.ApplicationConfigModule>();
        }
    }
}
