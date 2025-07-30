using Company.Application.Share.Prism;
using Company.Core.Extensions;
using System.Reflection;

namespace Company.Application.Process
{
    [Module(ModuleName = ModuleNames.ApplicationProcessModule, OnDemand = true)]
    public class ApplicationProcessModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
