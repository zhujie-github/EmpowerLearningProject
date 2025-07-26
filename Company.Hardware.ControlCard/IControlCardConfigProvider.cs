namespace Company.Hardware.ControlCard
{
    /// <summary>
    /// 控制卡配置提供者接口
    /// </summary>
    public interface IControlCardConfigProvider
    {
        /// <summary>
        /// 控制卡配置
        /// </summary>
        ControlCardConfig ControlCardConfig { get; }

        /// <summary>
        /// 控制卡配置改变事件
        /// </summary>
        event Action? ConfigChanged;
    }
}
