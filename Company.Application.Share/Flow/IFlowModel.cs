using Company.Application.Share.Filter;
using System.Collections.ObjectModel;

namespace Company.Application.Share.Flow
{
    public interface IFlowModel
    {
        ObservableCollection<IFilterModel> Filters { get; }

        void AddFilter(IFilterModel model);
    }
}
