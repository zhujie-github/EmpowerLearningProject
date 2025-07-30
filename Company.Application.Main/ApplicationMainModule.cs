using Company.Application.Main.Views;
using Company.Application.Share.Prism;

namespace Company.Application.Main
{
    /// <summary>
    /// ��ģ�� - �����ӳټ���
    /// </summary>
    [Module(ModuleName = ModuleNames.ApplicationMainModule, OnDemand = true)]
    [ModuleDependency(ModuleNames.ApplicationImageModule)]
    [ModuleDependency(ModuleNames.ApplicationMenuModule)]
    public class ApplicationMainModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<MainView>(RegionNames.MainRegion);
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<PrimaryView>(RegionNames.PrimaryRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>();
        }
    }
}
