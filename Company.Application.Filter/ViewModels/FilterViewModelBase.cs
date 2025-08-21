using Company.Application.Filter.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Company.Application.Filter.ViewModels
{
    public abstract class FilterViewModelBase<TFilterModel> : ReactiveObject, INavigationAware where TFilterModel : FilterModelBase, new()
    {
        [Reactive]
        public TFilterModel? FilterModel { get; private set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 从当前页面跳转到其他页面
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            FilterModel = default;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            FilterModel = navigationContext.Parameters.GetValue<TFilterModel>(typeof(TFilterModel).FullName ?? "");
        }
    }
}
