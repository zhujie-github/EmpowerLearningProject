using Company.Application.Filter.Attributes;
using Company.Application.Share.Filter;
using Company.Core.Ioc;
using System.Reflection;

namespace Company.Application.Filter.Services
{
    /// <summary>
    /// 创建所有滤波算法的服务工厂
    /// </summary>
    [ExposedService(types: [typeof(IFilterFactory)])]
    public class FilterFactory : IFilterFactory
    {
        public List<IFilterModel> CreateFilterModels()
        {
            var filters = new List<IFilterModel>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(t => typeof(IFilterModel).IsAssignableFrom(t));
            foreach (var type in types)
            {
                var filterAttribute = type.GetCustomAttribute<FilterAttribute>();
                if (filterAttribute != null && filterAttribute.IsEnabled)
                {
                    var obj = Activator.CreateInstance(type);
                    if (obj is IFilterModel model)
                    {
                        filters.Add(model);
                    }
                }
            }

            return filters;
        }
    }
}
