namespace Company.Hardware.Detector
{
    /// <summary>
    /// 平板探测器接口
    /// </summary>
    public interface IDetector
    {
        /// <summary>
        /// 事件：平板探测器拍照完成
        /// </summary>
        event Action<DetectorImage>? OnGrabbed;

        /// <summary>
        /// 是否初始化完成
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// 相机是否正在抓拍
        /// </summary>
        bool IsCapturing { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="detectorConfig"></param>
        /// <returns></returns>
        (bool, string?) Init(DetectorConfig detectorConfig);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 相机拍照
        /// </summary>
        void Capture();
    }
}
