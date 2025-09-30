using Company.Core.Models;

namespace Company.Application.Share.Preview
{
    /// <summary>
    /// 相机显示接口
    /// </summary>
    public interface ICameraDisplayModel
    {
        /// <summary>
        /// 相机图像
        /// </summary>
        UnmanagedArray2D<ColorBGRA>? Photo { get; }

        /// <summary>
        /// 相机图像观察者
        /// </summary>
        IObservable<UnmanagedArray2D<ColorBGRA>?> CameraObservable { get; }
    }
}
