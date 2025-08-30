using Company.Algorithm.Unwrapper;
using Company.Application.Filter.Attributes;
using Company.Application.Filter.Views;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Linq;

namespace Company.Application.Filter.Models
{
    [Filter(Index = 1, IsEnabled = true)]
    public class SobelFilterModel : FilterModelBase
    {
        [Reactive]
        public int Value { get; set; }

        public override string Name => "索贝尔算法";

        public override string View => nameof(SobelFilterView);

        public override string Icon => "Icon_Sobel";

        public override void DoFilter(UnmanagedArray2D<ushort> photo)
        {
            CppMethods.CppSobel(new CppImage16UC1(photo), new CppImage16UC1(photo), Value);
        }

        public override IObservable<Unit> GetObservable()
        {
            return this.WhenAnyValue(x => x.Value, x => x.IsEnabled).Select(x => Unit.Default);
        }
    }
}