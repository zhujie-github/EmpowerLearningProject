using Company.Core.Ioc;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Extensions
{
    /// <summary>
    /// 依赖注入扩展类，可以实现加载模块时，实例化标注为ExposedServiceAttribute的类。
    /// </summary>
    public static class DependencyExtension
    {
        private static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.CustomAttributes.Any(i => i.AttributeType == typeof(ExposedServiceAttribute)));
        }

        private static IEnumerable<ExposedServiceAttribute> GetExposedServices(Type type)
        {
            return type.GetTypeInfo().GetCustomAttributes<ExposedServiceAttribute>();
        }

        private static void RegisterAssembly(IContainerRegistry containerRegistry, Type type)
        {
            foreach(var exposedService in GetExposedServices(type))
            {
                if (exposedService.Lifetime == Lifetime.Singleton)
                {
                    containerRegistry.RegisterSingleton(type); //注册单例
                }
                //else if (exposedService.Lifetime == Lifetime.Transient)
                //{
                //    containerRegistry.Register(type); //注册瞬态
                //}

                foreach (var itype in exposedService.Types)
                {
                    if (exposedService.Lifetime == Lifetime.Singleton)
                    {
                        containerRegistry.RegisterSingleton(itype, type); //以接口注册单例
                    }
                    else if (exposedService.Lifetime == Lifetime.Transient)
                    {
                        containerRegistry.Register(itype, type); //以接口注册瞬态
                    }
                }
            }
        }

        public static void RegisterAssembly(this IContainerRegistry containerRegistry, Assembly assembly)
        {
            foreach (var type in GetTypes(assembly))
            {
                RegisterAssembly(containerRegistry, type);
            }
        }

        private static void InitializeAssembly(IContainerProvider containerProvider, Type type)
        {
            foreach (var exposedService in GetExposedServices(type))
            {
                if (exposedService.AutoInitialize && exposedService.Lifetime == Lifetime.Singleton)
                {
                    containerProvider.Resolve(type);
                }
            }
        }

        public static void InitializeAssembly(this IContainerProvider containerProvider, Assembly assembly)
        {
            foreach (var type in GetTypes(assembly))
            {
                InitializeAssembly(containerProvider, type);
            }
        }
    }
}
