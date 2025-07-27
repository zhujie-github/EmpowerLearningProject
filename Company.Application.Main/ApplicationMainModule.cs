using Company.Application.Main.Views;
using Company.Application.Share.Prism;

namespace Company.Application.Main
{
    /// <summary>
    /// 主模块 - 按需延迟加载
    /// </summary>
    [Module(ModuleName = ModuleNames.ApplicationMainModule, OnDemand = true)]
    [ModuleDependency(ModuleNames.ApplicationImageModule)]
    [ModuleDependency(ModuleNames.ApplicationMenuModule)]
    public class ApplicationMainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<MainView>(RegionNames.MainRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>();
        }
    }
}
