using Company.Core.Enums;

namespace Company.Application.Share.Main
{
    /// <summary>
    /// 鼠标工作模式提供者
    /// </summary>
    public interface IMouseWorkModeProvider
    {
        /// <summary>
        /// 鼠标工作模式
        /// </summary>
        MouseWorkMode MouseWorkMode { get; set; }

        /// <summary>
        /// 鼠标工作模式的观察者
        /// </summary>
        IObservable<MouseWorkMode> MouseWorkModeObservable { get; }
    }
}
