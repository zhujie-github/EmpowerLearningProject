namespace Company.Hardware.Camera
{
    public interface ICamera
    {
        /// <summary>
        /// 事件：相机拍照完成
        /// </summary>
        event Action<Photo>? OnGrabbed;

        /// <summary>
        /// 相机是否初始化完成
        /// </summary>
        /// <returns></returns>
        bool Initialized { get; }

        /// <summary>
        /// 相机是否正在抓拍
        /// </summary>
        bool IsCapturing { get; }

        /// <summary>
        /// 相机初始化
        /// </summary>
        /// <param name="cameraConfig"></param>
        /// <returns></returns>
        (bool, string?) Init(CameraConfig cameraConfig);

        /// <summary>
        /// 相机关闭
        /// </summary>
        /// <returns></returns>
        void Close();

        /// <summary>
        /// 相机抓拍
        /// </summary>
        /// <returns></returns>
        void Capture();
    }
}
