using Company.Application.Draw.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Draw
{
    [Module(ModuleName = ModuleNames.ApplicationDrawModule, OnDemand = true)]
    public class ApplicationDrawModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<DrawToolView>(RegionNames.DrawToolRegion);
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion<DrawTextView>(RegionNames.DrawTextRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
