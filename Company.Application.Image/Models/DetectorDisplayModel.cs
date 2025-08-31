using Company.Application.Share.Image;
using Company.Application.Share.Process;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;

namespace Company.Application.Image.Models
{
    /// <summary>
    /// 显示16位探测器图像的类型
    /// </summary>
    [ExposedService(types: [typeof(IDetectorDisplayModel)])]
    public class DetectorDisplayModel : IDetectorDisplayModel
    {
        private DoubleBuffer<ushort> _buffer { get; set; } = new();

        public UnmanagedArray2D<ushort>? Photo => _buffer.Current;

        public IObservable<UnmanagedArray2D<ushort>?> Observable { get; set; }

        public IDetectorProcessModel _processModel;

        public DetectorDisplayModel(IDetectorProcessModel processModel)
        {
            _processModel = processModel;
            processModel.TargetObservable.Subscribe(x => UpdateImageSource());
            Observable = this.WhenAnyValue(x => x._buffer.Current);
        }

        private void UpdateImageSource()
        {
            if (_processModel.TargetPhoto == null) return;
            _buffer.Write(_processModel.TargetPhoto);
        }
    }
}
