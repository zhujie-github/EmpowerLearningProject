using Company.Application.Share.Preview;
using Company.Application.Share.Process;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;

namespace Company.Application.Preview.Models
{
    [ExposedService]
    public class CameraDisplayModel : ICameraDisplayModel
    {
        private ICameraProcessModel CameraProcessModel { get; }
        private readonly DoubleBuffer<ColorBGRA> _buffer = new();

        public IObservable<UnmanagedArray2D<ColorBGRA>?> CameraObservable { get; }
        public UnmanagedArray2D<ColorBGRA>? Photo => _buffer.Current;

        public CameraDisplayModel(ICameraProcessModel cameraProcessModel)
        {
            CameraProcessModel = cameraProcessModel;

            CameraObservable = this.WhenAnyValue(p => p._buffer.Current); //这里监视Photo会无效
            cameraProcessModel.Observable.Subscribe(p => UpdateImageSource());
        }

        private void UpdateImageSource()
        {
            if (CameraProcessModel.Photo == null)
                return;
            _buffer.Write(CameraProcessModel.Photo);
        }
    }
}
