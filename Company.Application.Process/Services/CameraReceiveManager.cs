using Company.Application.Process.Models;
using Company.Core.Ioc;
using Company.Hardware.Camera;

namespace Company.Application.Process.Services
{
    /// <summary>
    /// 图像接收服务
    /// </summary>
    [ExposedService]
    public class CameraReceiveManager
    {
        private CameraProcessModel CameraProcessModel { get; }

        private ICamera Camera { get; }

        public CameraReceiveManager(CameraProcessModel cameraProcessModel, ICamera camera)
        {
            CameraProcessModel = cameraProcessModel;
            Camera = camera;
            Camera.OnGrabbed += Camera_OnGrabbed;
        }

        private void Camera_OnGrabbed(Photo photo)
        {
            CameraProcessModel.Write(photo);
        }
    }
}
