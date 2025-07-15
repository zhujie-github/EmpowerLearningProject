using Company.Application.Initialize.Models;
using Company.Core.Ioc;
using Company.Hardware.Camera;
using Company.Hardware.Detector;

namespace Company.Application.Initialize.Services
{
    /// <summary>
    /// 硬件生命周期管理器
    /// </summary>
    [ExposedService(Lifetime.Singleton, true)]
    public class HardwareLifetimeManager
    {
        /// <summary>
        /// 所有硬件加载成功
        /// </summary>
        public bool IsInitialized { get; set; } = false;

        public ICamera Camera { get; }

        public IDetector Detector { get; }

        public HardwareLifetimeManager(ICamera camera, IDetector detector)
        {
            Camera = camera;
            Detector = detector;
        }

        /// <summary>
        /// 初始化所有硬件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<InitResult> InitAsync()
        {
            if (IsInitialized)
            {
                throw new Exception("重复初始化硬件");
            }

            var task_camera = Task.Run(() => Camera.Init(new CameraConfig()));
            var task_detector = Task.Run(() => Detector.Init(new DetectorConfig()));
            var taskResults = await Task.WhenAll(task_camera, task_detector);
            var result = taskResults.All(r => r);
            if (!result)
            {
                var msg = "硬件初始化失败：";
                if (!taskResults[0])
                {
                    msg += "相机|";
                }
                if (!taskResults[1])
                {
                    msg += "平板探测器|";
                }
                msg = msg.TrimEnd('|');
                return new InitResult
                {
                    Success = false,
                    Message = msg
                };
            }

            IsInitialized = true;
            return new InitResult
            {
                Success = true,
                Message = "硬件初始化成功"
            };
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
            //IsInitialized = false;
        }
    }
}
