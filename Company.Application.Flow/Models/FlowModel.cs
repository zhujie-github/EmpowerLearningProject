using Company.Application.Share.Filter;
using Company.Application.Share.Flow;
using Company.Application.Share.Prism;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Company.Application.Flow.Models
{
    [ExposedService(types: [typeof(IFlowModel), typeof(IFlowProvider)])]
    public class FlowModel : ReactiveObject, IFlowModel, IFlowProvider
    {
        /// <summary>
        /// 当前选中的算法实体
        /// </summary>
        [Reactive]
        public IFilterModel? Filter { get; set; }

        public ObservableCollection<IFilterModel> Filters { get; set; } = [];

        public bool IsEnabled { get; private set; }

        /// <summary>
        /// 本流程实例中的观察者，可观察想要观察的属性
        /// </summary>
        public IObservable<Unit> Observable { get; private set; }

        public FlowModel()
        {
            this.WhenAnyValue(x => x.Filter).Subscribe(NavigationView); //显示当前Filter对应的View
            Observable = this.WhenAnyValue(x => x.IsEnabled, x => x.Filters.Count).Select(_ => Unit.Default);
        }

        public void AddFilter(IFilterModel filter)
        {
            if (Filters.FirstOrDefault(i => filter.GetType().FullName == i.GetType().FullName) != null)
                return;
            Filters.Add(filter);
            Filter = filter;
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

        public bool SetEnabled(bool isEnabled)
        {
            return IsEnabled = isEnabled;
        }

        public IObservable<Unit> GetObservable()
        {
            var list = new List<IObservable<Unit>>();
            list.AddRange(Filters.Select(i => i.GetObservable()));
            list.Add(Observable);
            return list.Merge();
        }

        public bool DoFilters(UnmanagedArray2D<ushort> photo)
        {
            var list = Filters.Where(i => i.IsEnabled).ToList();
            foreach (var filter in list)
            {
                filter.DoFilter(photo);
            }

            return list.Count > 0;
        }
    }
}
