namespace Company.Hardware.Camera
{
    /// <summary>
    /// 相机配置提供者
    /// </summary>
    public interface ICameraConfigProvider
    {
        /// <summary>
        /// 相机配置
        /// </summary>
        CameraConfig CameraConfig { get; }

        /// <summary>
        /// 相机配置改变事件
        /// </summary>
        event Action? ConfigChanged;
    }
}
