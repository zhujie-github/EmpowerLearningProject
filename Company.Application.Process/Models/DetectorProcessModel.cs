using Company.Application.Share.Process;
using Company.Core.Ioc;
using Company.Core.Models;
using Company.Hardware.Detector;
using ReactiveUI;

namespace Company.Application.Process.Models
{
    /// <summary>
    /// 16位FPD图像处理模型
    /// </summary>
    [ExposedService(types:typeof(IDetectorProcessModel))]
    public class DetectorProcessModel : ReactiveObject, IDetectorProcessModel
    {
        /// <summary>
        /// 原图双缓冲内存
        /// </summary>
        public DoubleBuffer<ushort> SourceBuffer { get; set; } = new DoubleBuffer<ushort>();

        /// <summary>
        /// 目标双缓冲内存
        /// </summary>
        public DoubleBuffer<ushort> TargetBuffer { get; set; } = new DoubleBuffer<ushort>();

        public UnmanagedArray2D<ushort>? SourcePhoto => SourceBuffer.Current;

        public IObservable<UnmanagedArray2D<ushort>?> SourceObservable { get; }

        public UnmanagedArray2D<ushort>? TargetPhoto => TargetBuffer.Current;

        public IObservable<UnmanagedArray2D<ushort>?> TargetObservable { get; }

        public DetectorProcessModel()
        {
            SourceObservable = this.WhenAnyValue(x => x.SourceBuffer.Current);
            TargetObservable = this.WhenAnyValue(x => x.TargetBuffer.Current);
        }

        /// <summary>
        /// 将图像写入目标双缓冲内存
        /// </summary>
        /// <param name="photo"></param>
        public void Write(UnmanagedArray2D<ushort> photo)
        {
            TargetBuffer.Write(photo);
        }

        /// <summary>
        /// 将源图写入源双缓冲内存
        /// </summary>
        /// <param name="photo"></param>
        public void Write(DetectorImage photo)
        {
            SourceBuffer.Write(photo.Header, photo.Width, photo.Height);
        }
    }
}
