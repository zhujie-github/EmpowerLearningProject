namespace Company.Hardware.Camera
{
    internal interface ICamera
    {
        /// <summary>
        /// 事件：相机拍照完成
        /// </summary>
        event Action<IntPtr> ImageCaptured;

        /// <summary>
        /// 相机是否初始化完成
        /// </summary>
        /// <returns></returns>
        bool Initialized { get; }

        /// <summary>
        /// 相机初始化
        /// </summary>
        /// <returns></returns>
        bool Init(CameraConfig cameraConfig);

        /// <summary>
        /// 相机关闭
        /// </summary>
        /// <returns></returns>
        void Close();

        /// <summary>
        /// 触发相机拍照
        /// </summary>
        /// <returns></returns>
        bool Trigger();
    }
}
