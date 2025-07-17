namespace Company.Hardware.Detector
{
    /// <summary>
    /// 平板探测器配置提供者
    /// </summary>
    public interface IDetectorConfigProvider
    {
        /// <summary>
        /// 平板探测器配置
        /// </summary>
        DetectorConfig DetectorConfig { get; }

        /// <summary>
        /// 平板探测器配置改变事件
        /// </summary>
        event Action ConfigChanged;
    }
}
