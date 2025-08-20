using Company.Application.Share.Filter;
using Company.Core.Ioc;
using ReactiveUI;
using System.Windows.Input;

namespace Company.Application.Flow.ViewModels
{
    public class AddFilterViewModel : DialogViewModelBase
    {
        public ICommand SubmitCommand { get; }

        /// <summary>
        /// 所有算法
        /// </summary>
        public List<IFilterModel> DisplayModels { get; }

        /// <summary>
        /// 当前选中的算法
        /// </summary>
        public IFilterModel? DisplayModel { get; set; }

        public AddFilterViewModel()
        {
            Title = "选择滤波算法";
            DisplayModels = []; //todo
            SubmitCommand = ReactiveCommand.Create(Submit);
        }

        private void Submit()
        {
            //todo
        }
    }
}
