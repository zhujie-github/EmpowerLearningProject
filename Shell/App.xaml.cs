using System.Windows;

namespace Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
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
