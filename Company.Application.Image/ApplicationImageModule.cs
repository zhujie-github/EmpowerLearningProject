using Company.Application.Image.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Image
{
    /// <summary>
    /// 图片模块 - 按需延迟加载
    /// </summary>
    [Module(ModuleName = ModuleNames.ApplicationImageModule, OnDemand = true)]
    public class ApplicationImageModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<IRegionManager>().RequestNavigate(RegionNames.ImageRegion, ViewNames.ImageView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterForNavigation<ImageView>();
        }
    }
}
