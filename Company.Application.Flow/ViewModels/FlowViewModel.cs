using Company.Core.Ioc;
using Company.Core.Extensions;
using ReactiveUI;
using System.Windows.Input;
using Company.Application.Flow.Views;
using Company.Application.Share.Filter;
using Company.Core.Helpers;
using Company.Application.Flow.Models;

namespace Company.Application.Flow.ViewModels
{
    public class FlowViewModel : ReactiveObject
    {
        public FlowModel? FlowModel { get; }

        public ICommand AddFilterCommand { get; }

        public FlowViewModel()
        {
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
                    FlowModel?.AddFilter(model);
                }
            });
        }
    }
}
