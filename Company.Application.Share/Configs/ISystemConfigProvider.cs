using Company.Hardware.Camera;
using Company.Hardware.ControlCard;
using Company.Hardware.Detector;

namespace Company.Application.Share.Configs
{
    /// <summary>
    /// 系统配置提供者
    /// </summary>
    public interface ISystemConfigProvider : ISoftwareConfigProvider, ICameraConfigProvider, IDetectorConfigProvider,
        IControlCardConfigProvider
    {
    }
}
