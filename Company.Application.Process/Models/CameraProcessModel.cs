using Company.Application.Share.Process;
using Company.Core.Ioc;
using Company.Core.Models;
using Company.Hardware.Camera;
using ReactiveUI;

namespace Company.Application.Process.Models
{
    [ExposedService(types:typeof(ICameraProcessModel))]
    public class CameraProcessModel : ReactiveObject, ICameraProcessModel
    {
        /// <summary>
        /// 双缓冲内存区域
        /// </summary>
        public DoubleBuffer<ColorBGRA> Buffer { get; set; } = new DoubleBuffer<ColorBGRA>();

        /// <summary>
        /// 原始图像
        /// </summary>
        public UnmanagedArray2D<ColorBGRA>? Photo => Buffer.Current;

        public IObservable<UnmanagedArray2D<ColorBGRA>?> Observable { get; }

        public CameraProcessModel()
        {
            Observable = this.WhenAnyValue(p => p.Buffer.Current);
        }

        /// <summary>
        /// 将图像写入双缓冲区内存
        /// </summary>
        /// <param name="photo"></param>
        public void Write(Photo photo)
        {
            Buffer.Write(photo.Header, photo.Width, photo.Height);
        }
    }
}
