using Company.Application.Share.Configs;
using Company.Hardware.Camera;
using Company.Hardware.ControlCard;
using Company.Hardware.Detector;

namespace Company.Application.Config.Models
{
    public class SystemConfigModel
    {
        public SoftwareConfig SoftwareConfig { get; set; } = new SoftwareConfig();

        public CameraConfig CameraConfig { get; set; } = new CameraConfig();

        public DetectorConfig DetectorConfig { get; set; } = new DetectorConfig();

        public ControlCardConfig ControlCardConfig { get; set; } = new ControlCardConfig();
    }
}
