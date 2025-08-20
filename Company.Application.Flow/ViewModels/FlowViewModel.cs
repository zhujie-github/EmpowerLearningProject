using Company.Core.Ioc;
using Company.Core.Extensions;
using ReactiveUI;
using System.Windows.Input;
using Company.Application.Flow.Views;

namespace Company.Application.Flow.ViewModels
{
    public class FlowViewModel : ReactiveObject
    {
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
                    //todo
                }
            });
        }
    }
}
