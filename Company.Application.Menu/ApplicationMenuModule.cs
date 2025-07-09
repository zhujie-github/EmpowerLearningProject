using Company.Application.Menu.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Menu
{
    /// <summary>
    /// 主菜单模块 - 按需延迟加载
    /// </summary>
    [Module(ModuleName = ModuleNames.ApplicationMenuModule, OnDemand = true)]
    public class ApplicationMenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            //containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<MenuView>(RegionNames.MenuRegion);
            containerProvider.Resolve<IRegionManager>().RequestNavigate(RegionNames.MenuRegion, ViewNames.MenuView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterForNavigation<MenuView>();
        }
    }
}
