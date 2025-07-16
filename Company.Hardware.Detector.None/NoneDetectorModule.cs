using Company.Core.Extensions;
using System.Reflection;

namespace Company.Hardware.Detector.None
{
    public class NoneDetectorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());

            if (!containerRegistry.IsRegistered<IDetector>())
            {
                containerRegistry.RegisterSingleton<IDetector, NoneDetector>(); //注册仿真平板探测器到IoC容器
            }
        }
    }
}
