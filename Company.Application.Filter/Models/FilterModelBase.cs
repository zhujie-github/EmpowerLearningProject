using Company.Application.Share.Filter;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace Company.Application.Filter.Models
{
    public abstract class FilterModelBase : ReactiveObject, IFilterModel
    {
        public abstract string Name { get; }

        public abstract string View { get; }

        public abstract string Icon { get; }

        [Reactive]
        public bool IsEnabled { get; set; }

        public abstract void Filter(UnmanagedArray2D<ushort> photo);

        public abstract IObservable<Unit> GetObservable();
    }
}
