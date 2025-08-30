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
    [Filter(Index = 0, IsEnabled = true)]
    public class TestFilterModel : FilterModelBase
    {
        [Reactive]
        public ushort Value { get; set; }

        public override string Name => "OpenCV与C++测试";

        public override string View => nameof(TestFilterView);

        public override string Icon => "Icon_Test";

        public override void DoFilter(UnmanagedArray2D<ushort> photo)
        {
            CppMethods.CppTest(new CppImage16UC1(photo), new CppImage16UC1(photo), Value);
        }

        public override IObservable<Unit> GetObservable()
        {
            return this.WhenAnyValue(x => x.Value, x => x.IsEnabled).Select(x => Unit.Default);
        }
    }
}
