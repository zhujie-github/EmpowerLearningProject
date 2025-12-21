using Company.Application.Share.Image;
using Company.Application.Share.Process;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.Image.Models
{
    /// <summary>
    /// 显示16位探测器图像的模型
    /// </summary>
    [ExposedService(types: typeof(IDetectorDisplayModel))]
    public class DetectorDisplayModel : IDetectorDisplayModel
    {
        private IDetectorProcessModel _processModel;
        public DetectorDisplayModel(IDetectorProcessModel processModel)
        {
            _processModel = processModel;

            processModel.TargetObservable.Subscribe(p => UpdateImageSource());

            Observable = this.WhenAnyValue(p => p._buffer.Current);
        }

        private void UpdateImageSource()
        {
            if(_processModel.TargetPhoto != null )
            {
                _buffer.Write(_processModel.TargetPhoto);
            }
        }

        private DoubleBuffer<ushort> _buffer { get; set; } =  new DoubleBuffer<ushort>();

        public UnmanagedArray2D<ushort> Photo => _buffer.Current;

        public IObservable<UnmanagedArray2D<ushort>> Observable { get; set; }
    }
}
