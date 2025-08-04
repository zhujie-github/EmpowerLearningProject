using Company.Application.Process.Models;
using Company.Core.Ioc;
using Company.Hardware.Detector;

namespace Company.Application.Process.Services
{
    /// <summary>
    /// 16位FPD图像接收管理者，单例常驻内存
    /// </summary>
    [ExposedService]
    public class DetectorReceiveManager
    {
        private DetectorProcessModel DetectorProcessModel { get; }

        private IDetector Detector { get; }

        public DetectorReceiveManager(DetectorProcessModel detectorProcessModel, IDetector detector)
        {
            DetectorProcessModel = detectorProcessModel;
            Detector = detector;
            Detector.OnGrabbed += DetectorOnGrabbed;
        }

        private void DetectorOnGrabbed(DetectorImage image)
        {
            DetectorProcessModel.Write(image); //真正地接收16位原图并写入双缓冲内存
        }
    }
}
