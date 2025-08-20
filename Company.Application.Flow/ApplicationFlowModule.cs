using Company.Application.Flow.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Flow
{
    [Module(ModuleName = ModuleNames.ApplicationFlowModule, OnDemand = true)]
    [ModuleDependency(ModuleNames.ApplicationFilterModule)]
    public class ApplicationFlowModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<FlowView>(RegionNames.FlowRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            containerRegistry.RegisterDialog<AddFilterView>();
        }
    }
}
