using Company.Application.Share.Filter;
using Company.Core.Ioc;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Company.Application.Flow.Models
{
    [ExposedService]
    public class FlowModel : ReactiveObject
    {
        /// <summary>
        /// 当前选中的算法实体
        /// </summary>
        public IFilterModel? Filter { get; set; }

        public ObservableCollection<IFilterModel> Filters { get; set; } = [];

        public void AddFilter(IFilterModel model)
        {
            Filters.Add(model);
            Filter = model;
        }
    }
}
