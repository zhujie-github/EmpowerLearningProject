using Company.Application.Preview.Views;
using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Preview
{
    //[Module(ModuleName = ModuleNames.ApplicationPreviewModule, OnDemand = true)]
    public class ApplicationPreviewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
            containerProvider.Resolve<RegionManager>().RegisterViewWithRegion<PreviewView>(RegionNames.CameraRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
            //containerRegistry.RegisterForNavigation<PreviewView>();
        }
    }
}
