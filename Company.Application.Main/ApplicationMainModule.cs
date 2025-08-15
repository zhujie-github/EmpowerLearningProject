using Company.Application.Main.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Main
{
    /// <summary>
    /// 主模块 - 按需延迟加载
    /// </summary>
    [Module(ModuleName = ModuleNames.ApplicationMainModule, OnDemand = true)]
    [ModuleDependency(ModuleNames.ApplicationMenuModule)]
    [ModuleDependency(ModuleNames.ApplicationImageModule)]
    [ModuleDependency(ModuleNames.ApplicationProcessModule)]
    public class ApplicationMainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<MainView>(RegionNames.MainRegion);
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<PrimaryView>(RegionNames.PrimaryRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterForNavigation<MainView>();
        }
    }
}
