using Company.Core.Models;
using System.Reactive;
using System.Windows.Documents;

namespace Company.Application.Share.Flow
{
    /// <summary>
    /// 图像算法提供者
    /// </summary>
    public interface IFlowProvider
    {
        bool IsEnabled { get; }

        bool SetEnabled(bool isEnabled);

        /// <summary>
        /// 获取所有图像算法的观察者
        /// </summary>
        IObservable<Unit> GetObservable();

        /// <summary>
        /// 对当前的FPD图像进行处理
        /// </summary>
        /// <param name="photo"></param>
        bool DoFilters(UnmanagedArray2D<ushort> photo);
    }
}
