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
    [Filter]
    public class GaussianBlurFilterModel : FilterModelBase
    {
        [Reactive]
        public ushort Width { get; set; } = 3;

        [Reactive]
        public ushort Height { get; set; } = 3;

        public override string Name => "高斯滤波算法";

        public override string View => nameof(GaussianBlurFilterView);

        public override string Icon => "Icon_GaussianBlur";

        public override void DoFilter(UnmanagedArray2D<ushort> photo)
        {
            CppMethods.CppGaussianBlur(new CppImage16UC1(photo), new CppImage16UC1(photo), Width, Height);
        }

        public override IObservable<Unit> GetObservable()
        {
            return this.WhenAnyValue(x => x.Width, x => x.Height, x => x.IsEnabled).Select(x => Unit.Default);
        }
    }
}
