using Company.Application.Config.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Config
{
    [Module(ModuleName = ModuleNames.ApplicationConfigModule, OnDemand = true)]
    public class ApplicationConfigModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterDialog<ConfigView>();
        }
    }
}
