using Company.Core.Models;

namespace Company.Application.Share.Image
{
    /// <summary>
    /// 显示16位探测器图像的接口
    /// </summary>
    public interface IDetectorDisplayModel
    {
        UnmanagedArray2D<ushort>? Photo { get; }

        IObservable<UnmanagedArray2D<ushort>?> Observable { get; }
    }
}
