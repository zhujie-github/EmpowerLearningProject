using Company.Application.Process.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Process
{
    [Module(ModuleName = ModuleNames.ApplicationProcessModule, OnDemand = true)]
    [ModuleDependency(ModuleNames.ApplicationFlowModule)]
    public class ApplicationProcessModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<ProcessView>(RegionNames.ProcessRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
