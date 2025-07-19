using Company.Application.Share.Configs;
using Company.Core.Ioc;
using Company.Hardware.Camera;
using Company.Hardware.Detector;

namespace Company.Application.Initialize.Services
{
    /// <summary>
    /// 硬件生命周期管理器
    /// </summary>
    [ExposedService(Lifetime.Singleton, true)]
    public class HardwareLifetimeManager(ISystemConfigProvider systemConfigProvider, ICamera camera, IDetector detector)
    {
        /// <summary>
        /// 所有硬件加载成功
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        private ISystemConfigProvider SystemConfigProvider { get; } = systemConfigProvider;

        private ICamera Camera { get; } = camera;

        private IDetector Detector { get; } = detector;

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

            var task_camera = Task.Run(() => Camera.Init(SystemConfigProvider.CameraConfig));
            var task_detector = Task.Run(() => Detector.Init(SystemConfigProvider.DetectorConfig));
            var taskResults = await Task.WhenAll(task_camera, task_detector);
            IsInitialized = taskResults.All(r => r.Item1);
            var msgs = new List<string>();
            if (!taskResults[0].Item1)
                msgs.Add($"相机:{taskResults[0].Item2}".TrimEnd(':'));
            if (!taskResults[1].Item1)
                msgs.Add($"平板探测器:{taskResults[1].Item2}".TrimEnd(':'));

            return (IsInitialized, string.Join(" | ", msgs));
        }

        /// <summary>
        /// 关闭所有硬件
        /// </summary>
        public void Close()
        {
            try
            {
                Camera.Close();
            }
            catch// (Exception ex)
            {
                // 处理相机关闭异常 todo
            }
            try
            {
                Detector.Close();
            }
            catch// (Exception ex)
            {
                // 处理探测器关闭异常 todo
            }
            IsInitialized = false;
        }
    }
}
