using Company.Core.Extensions;
using System.Reflection;

namespace Company.Hardware.Camera.HIK
{
    public class HikCameraModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());

            if (!containerRegistry.IsRegistered<ICamera>())
            {
                containerRegistry.RegisterSingleton<ICamera, HikCamera>(); //注册海康相机到IoC容器
            }
        }
    }
}
