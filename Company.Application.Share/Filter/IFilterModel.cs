using Company.Core.Models;
using System.Reactive;

namespace Company.Application.Share.Filter
{
    /// <summary>
    /// 图像滤波处理算法实体接口，对应OpenCV的图像处理函数
    /// </summary>
    public interface IFilterModel
    {
        /// <summary>
        /// 算法名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// WPF界面名称
        /// </summary>
        string View { get; }

        /// <summary>
        /// 图标
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// 执行当前算法
        /// </summary>
        /// <param name="photo"></param>
        void DoFilter(UnmanagedArray2D<ushort> photo);

        /// <summary>
        /// 获取当前算法的观察者
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> GetObservable();
    }
}
