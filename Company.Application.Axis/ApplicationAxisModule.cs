using Company.Application.Axis.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Axis
{
    [Module(ModuleName = ModuleNames.ApplicationAxisModule, OnDemand = true)]
    public class ApplicationAxisModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<AxisView>(RegionNames.AxisRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
