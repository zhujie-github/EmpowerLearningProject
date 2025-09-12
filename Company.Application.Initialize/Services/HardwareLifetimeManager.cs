using Company.Application.Share.Configs;
using Company.Core.Ioc;
using Company.Hardware.Camera;
using Company.Hardware.ControlCard;
using Company.Hardware.Detector;
using Company.Logger;

namespace Company.Application.Initialize.Services
{
    /// <summary>
    /// 硬件生命周期管理器
    /// </summary>
    [ExposedService]
    public class HardwareLifetimeManager(ISystemConfigProvider systemConfigProvider, ICamera camera, IDetector detector,
        IControlCard controlCard)
    {
        /// <summary>
        /// 所有硬件加载成功
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// 初始化所有硬件
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string?)> InitAsync()
        {
            string? msg = null;

            if (IsInitialized)
            {
                msg = "重复初始化硬件";
                return (false, msg);
            }

            var task_camera = Task.Run(() => camera.Init(systemConfigProvider.CameraConfig));
            var task_detector = Task.Run(() => detector.Init(systemConfigProvider.DetectorConfig));
            var task_controlCard = Task.Run(() => controlCard.Init(systemConfigProvider.ControlCardConfig, true));
            var taskResults = await Task.WhenAll(task_camera, task_detector, task_controlCard);
            IsInitialized = taskResults.All(r => r.Item1);
            var msgs = new List<string>();
            if (!taskResults[0].Item1)
                msgs.Add($"相机:{taskResults[0].Item2}".TrimEnd(':'));
            if (!taskResults[1].Item1)
                msgs.Add($"平板探测器:{taskResults[1].Item2}".TrimEnd(':'));
            if (!taskResults[2].Item1)
                msgs.Add($"控制卡:{taskResults[2].Item2}".TrimEnd(':'));

            return (IsInitialized, string.Join(" | ", msgs));
        }

        /// <summary>
        /// 关闭所有硬件
        /// </summary>
        public void Close()
        {
            try
            {
                camera.Close();
            }
            catch (Exception ex)
            {
                NLogger.Error(ex);
            }

            try
            {
                detector.Close();
            }
            catch (Exception ex)
            {
                NLogger.Error(ex);
            }

            try
            {
                controlCard.Close();
            }
            catch (Exception ex)
            {
                NLogger.Error(ex);
            }

            IsInitialized = false;
            NLogger.Info("所有硬件已关闭");
        }
    }
}
