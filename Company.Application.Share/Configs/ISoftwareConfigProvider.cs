namespace Company.Application.Share.Configs
{
    /// <summary>
    /// 软件配置提供者
    /// </summary>
    public interface ISoftwareConfigProvider
    {
        /// <summary>
        /// 软件配置
        /// </summary>
        SoftwareConfig SoftwareConfig { get; }

        /// <summary>
        /// 软件配置改变事件
        /// </summary>
        event Action? ConfigChanged;
    }
}
