using Company.Core.Extensions;
using System.Reflection;

namespace Company.Hardware.ControlCard.AdTech
{
    public class AdTechControlCardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.InitializeAssembly(Assembly.GetExecutingAssembly());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterAssembly(Assembly.GetExecutingAssembly());

            if (!containerRegistry.IsRegistered<IControlCard>())
            {
                containerRegistry.RegisterSingleton<IControlCard, AdTechControlCard>(); //注册众为兴控制卡到IoC容器
            }
        }
    }
}
