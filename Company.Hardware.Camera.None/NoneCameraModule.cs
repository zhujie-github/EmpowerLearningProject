using Company.Core.Extensions;
using System.Reflection;

namespace Company.Hardware.Camera.None
{
    public class NoneCameraModule : IModule
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
                containerRegistry.Register<ICamera, NoneCamera>(); //注册仿真相机到IoC容器
            }
        }
    }
}
