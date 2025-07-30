using Company.Application.Share.Process;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;

namespace Company.Application.Process.Models
{
    [ExposedService(types:typeof(ICameraProcessModel))]
    public class CameraProcessModel : ReactiveObject, ICameraProcessModel
    {
        public DoubleBuffer<ColorBGRA>? Buffer { get; set; } = new DoubleBuffer<ColorBGRA>();

        public IObservable<UnmanagedArray2D<ColorBGRA>>? Observable { get; set; }

        public UnmanagedArray2D<ColorBGRA>? Photo {  get; set; }
    }
}
