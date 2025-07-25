using Company.Core.Events;
using Company.Core.Ioc;
using Company.Logger;

namespace Company.Hardware.Camera
{
    public abstract class CameraBase : ICamera
    {
        public CameraConfig? Config { get; private set; }

        public bool Initialized { get; private set; } = false;

        public bool IsCapturing { get; protected set; } = false;

        public event Action<Photo>? ImageCaptured;

        public CameraBase()
        {
            PrismProvider.EventAggregator?.GetEvent<ConfigChangedEvent>().Subscribe(OnConfigChanged);
        }

        public (bool, string?) Init(CameraConfig cameraConfig)
        {
            Config = cameraConfig;
            string? msg;

            if (Initialized)
            {
                msg = "Camera is already initialized.";
                return (false, msg);
            }

            try
            {
                if (!DoInit(out msg))
                {
                    return (false, msg);
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return (false, msg);
            }

            Initialized = true;
            return (true, msg);
        }

        public void Close()
        {
            if (Initialized)
            {
                if (!DoClose(out var errMsg))
                {
                    NLogger.Error($"Camera close failed: {errMsg}");
                    return;
                }
                Initialized = false;
                IsCapturing = false; // 确保关闭时重置抓拍状态
            }
        }

        public void Capture()
        {
            if (Initialized)
            {
                if (!DoCapture(out var errMsg))
                {
                    NLogger.Error($"Camera capture failed: {errMsg}");
                    return;
                }
            }
        }

        /// <summary>
        /// 拍照时触发的事件
        /// </summary>
        /// <param name="photo"></param>
        public void OnImageCaptured(Photo photo)
        {
            ImageCaptured?.Invoke(photo);
        }

        /// <summary>
        /// 配置改变时触发的事件
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnConfigChanged()
        {
            NLogger.Info(
                $"相机配置改变：{nameof(Config.Width)}: {Config?.Width}, {nameof(Config.Height)}: {Config?.Height}");
        }

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机初始化逻辑。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoInit(out string errMsg);

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机关闭逻辑。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoClose(out string errMsg);

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机抓拍逻辑。
        /// </summary>
        public abstract bool DoCapture(out string errMsg);
    }
}
