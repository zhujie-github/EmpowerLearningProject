using Company.Core.Extensions;
using System.Reflection;

namespace Company.Hardware.ControlCard.None
{
    public class NoneControlCardModule : IModule
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
                containerRegistry.RegisterSingleton<IControlCard, NoneControlCard>(); //注册仿真控制卡到IoC容器
            }
        }
    }
}
