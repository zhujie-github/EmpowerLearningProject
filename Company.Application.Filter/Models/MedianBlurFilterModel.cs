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
    public class MedianBlurFilterModel : FilterModelBase
    {
        /// <summary>
        /// 孔径大小，1 3 5 7 9（卷积核大小）
        /// </summary>
        [Reactive]
        public int Value { get; set; } = 3;

        public override string Name => "中值滤波算法";

        public override string View => nameof(MedianBlurFilterView);

        public override string Icon => "Icon_MedianBlur";

        public override void DoFilter(UnmanagedArray2D<ushort> photo)
        {
            CppMethods.CppMedianBlur(new CppImage16UC1(photo), new CppImage16UC1(photo), Value);
        }

        public override IObservable<Unit> GetObservable()
        {
            return this.WhenAnyValue(x => x.Value, x => x.IsEnabled).Select(x => Unit.Default);
        }
    }
}
