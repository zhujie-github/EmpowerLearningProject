using Company.Application.Flow.Models;
using Company.Application.Flow.Views;
using Company.Application.Share.Filter;
using Company.Core.Cache;
using Company.Core.Extensions;
using Company.Core.Helpers;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Windows.Input;

namespace Company.Application.Flow.ViewModels
{
    public class FlowViewModel : ReactiveObject
    {
        private ICacheManager CacheManager { get; }
        private IFilterFactory FilterFactory { get; }

        public FlowModel FlowModel { get; }

        /// <summary>
        /// 是否启用流程服务
        /// </summary>
        [Reactive]
        public bool IsEnabled { get; private set; }

        public ICommand AddFilterCommand { get; }

        public FlowViewModel(FlowModel flowModel, ICacheManager cacheManager, IFilterFactory filterFactory)
        {
            FlowModel = flowModel;
            CacheManager = cacheManager;
            FilterFactory = filterFactory;

            AddFilterCommand = ReactiveCommand.Create(AddFilter);

            InitializeCachedFilters();
            FlowModel.Observable.Subscribe(SaveFilters);
        }

        /// <summary>
        /// 加载缓存的过滤算法
        /// </summary>
        private void InitializeCachedFilters()
        {
            CacheManager.Get(CacheKey.Filters, out HashSet<string>? filters);
            if (filters != null && filters.Count > 0)
            {
                var all = FilterFactory.CreateFilterModels();

                foreach (var item in filters)
                {
                    var filter = all.FirstOrDefault(t => t.GetType().FullName == item);
                    if (filter != null)
                    {
                        FlowModel.Filters.Add(filter);
                    }
                }
            }
        }

        /// <summary>
        /// 保存当前选中的过滤算法集合
        /// </summary>
        /// <param name="unit"></param>
        private void SaveFilters(Unit unit)
        {
            var filters = new HashSet<string>();
            foreach (var item in FlowModel.Filters)
            {
                filters.Add(item.GetType().FullName!);
            }
            CacheManager.Set(CacheKey.Filters, filters);
        }

        /// <summary>
        /// 添加过滤算法模型
        /// </summary>
        private void AddFilter()
        {
            PrismProvider.DialogService.ShowDialog<AddFilterView>(p =>
            {
                if (p.Result == ButtonResult.OK)
                {
                    var model = p.Parameters.GetValue<IFilterModel>("DisplayModel");
                    Assert.NotNull(model);
                    FlowModel.AddFilter(model);
                }
            });
        }
    }
}
