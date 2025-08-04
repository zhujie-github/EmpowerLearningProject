using Company.Core.Models;

namespace Company.Application.Share.Process
{
    /// <summary>
    /// 16位平板探测器图像数据接口，管理原始数据并提供一个观察者
    /// </summary>
    public interface IDetectorProcessModel
    {
        UnmanagedArray2D<ushort>? SourcePhoto { get; }

        IObservable<UnmanagedArray2D<ushort>?> SourceObservable { get; }

        UnmanagedArray2D<ushort>? TargetPhoto { get; }

        IObservable<UnmanagedArray2D<ushort>?> TargetObservable { get; }
    }
}
