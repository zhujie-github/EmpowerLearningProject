using Company.Application.Login.Views;
using Company.Application.Share.Prism;

namespace Company.Application.Login
{
    /// <summary>
    /// 登录模块 - 按需延迟加载
    /// </summary>
    [Module(ModuleName = ModuleNames.ApplicationLoginModule, OnDemand = true)]
    public class ApplicationLoginModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<LoginView>(RegionNames.MainRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView>();
        }
    }
}
