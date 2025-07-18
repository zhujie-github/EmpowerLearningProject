using Company.Application.Share.Configs;
using Company.Core.Ioc;
using Company.Hardware.Camera;
using Company.Hardware.Detector;

namespace Company.Application.Config.Services
{
    /// <summary>
    /// 系统配置提供者，作用是将系统配置传给不同的模块，用接口实现数据通讯
    /// </summary>
    [ExposedService(Lifetime.Singleton, true, typeof(ISystemConfigProvider), typeof(ISoftwareConfigProvider), typeof(ICameraConfigProvider), typeof(IDetectorConfigProvider))]
    public class SystemConfigProvider : ISystemConfigProvider, ISoftwareConfigProvider, ICameraConfigProvider, IDetectorConfigProvider
    {
        private SystemConfigManager SystemConfigManager { get; }

        public SoftwareConfig SoftwareConfig { get; private set; }

        public CameraConfig CameraConfig { get; private set; }

        public DetectorConfig DetectorConfig { get; private set; }

        public event Action? ConfigChanged;

        public SystemConfigProvider(SystemConfigManager systemConfigManager)
        {
            SystemConfigManager = systemConfigManager;
            SoftwareConfig = SystemConfigManager.Config.SoftwareConfig;
            CameraConfig = SystemConfigManager.Config.CameraConfig;
            DetectorConfig = SystemConfigManager.Config.DetectorConfig;
        }
    }
}
