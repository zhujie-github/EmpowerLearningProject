using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Company.Application.Filter.ViewModels
{
    public abstract class FilterViewModelBase<TFilterModel> : ReactiveObject, INavigationAware
    {
        [Reactive]
        public TFilterModel FilterModel { get; private set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
