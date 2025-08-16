using Company.Core.Enums;

namespace Company.Application.Share.Main
{
    /// <summary>
    /// 图像缩放提供者
    /// </summary>
    public interface IZoomModeProvider
    {
        /// <summary>
        /// 图像缩放模式
        /// </summary>
        ZoomMode ZoomMode { get; set; }

        /// <summary>
        /// 图像缩放模式观察者
        /// </summary>
        IObservable<ZoomMode> ZoomModeObservable { get; }
    }
}
