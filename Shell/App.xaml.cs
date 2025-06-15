using System.Configuration;
using System.Data;
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
            return new Views.MainWindow();
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
        }
    }
}
