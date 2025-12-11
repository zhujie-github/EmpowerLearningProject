using Company.Core.Enums;

namespace Company.Application.Share.Mouse
{
    /// <summary>
    /// 鼠标操作类型的提供者
    /// </summary>
    public interface IMouseOperationProvider
    {
        /// <summary>
        /// 当前鼠标操作类型
        /// </summary>
        MouseOperationType? MouseOperationType { get; set; }

        /// <summary>
        /// 当前鼠标操作类型的观察者
        /// </summary>
        IObservable<MouseOperationType?> Observable { get; }

        /// <summary>
        /// 取消鼠标操作
        /// </summary>
        /// <param name="mouseOperationType"></param>
        void Cancel(MouseOperationType? mouseOperationType = null);
    }
}