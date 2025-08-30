using Company.Application.Share.Filter;
using Company.Application.Share.Flow;
using Company.Application.Share.Prism;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Company.Application.Flow.Models
{
    [ExposedService(types: [typeof(IFlowModel)])]
    public class FlowModel : ReactiveObject, IFlowModel
    {
        /// <summary>
        /// 当前选中的算法实体
        /// </summary>
        [Reactive]
        public IFilterModel? Filter { get; set; }

        public ObservableCollection<IFilterModel> Filters { get; set; } = [];

        public FlowModel()
        {
            this.WhenAnyValue(x => x.Filter).Subscribe(NavigationView); //显示当前Filter对应的View
        }

        public void AddFilter(IFilterModel model)
        {
            Filters.Add(model);
            Filter = model;
        }

        private void NavigationView(IFilterModel? filter)
        {
            if (string.IsNullOrWhiteSpace(filter?.GetType().FullName)) return;
            var param = new NavigationParameters
            {
                { filter.GetType().FullName!, filter }
            };
            PrismProvider.RegionManager.RequestNavigate(RegionNames.FilterRegion, filter.View, param);
        }
    }
}
