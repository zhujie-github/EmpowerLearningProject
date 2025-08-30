using Company.Application.Flow.Models;
using Company.Application.Flow.Views;
using Company.Application.Share.Filter;
using Company.Core.Extensions;
using Company.Core.Helpers;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;

namespace Company.Application.Flow.ViewModels
{
    public class FlowViewModel : ReactiveObject
    {
        public FlowModel FlowModel { get; }

        /// <summary>
        /// 是否启用流程服务
        /// </summary>
        [Reactive]
        public bool IsEnabled { get; private set; }

        public ICommand AddFilterCommand { get; }

        public FlowViewModel(FlowModel flowModel)
        {
            FlowModel = flowModel;
            AddFilterCommand = ReactiveCommand.Create(AddFilter);
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
