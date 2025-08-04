using Company.Core.Events;
using Company.Core.Ioc;
using Company.Core.Models;
using Company.Logger;

namespace Company.Hardware.Detector
{
    public abstract class DetectorBase : IDetector
    {
        public DetectorConfig? Config { get; private set; }

        public bool Initialized { get; private set; } = false;
        public bool IsGrabbing { get; protected set; } = false;

        public event Action<DetectorImage>? OnGrabbed;

        public DetectorBase()
        {
            PrismProvider.EventAggregator?.GetEvent<ConfigChangedEvent>().Subscribe(OnConfigChanged);
        }

        public (bool, string?) Init(DetectorConfig detectorConfig)
        {
            Config = detectorConfig;
            string? msg;

            if (Initialized)
            {
                msg = "Detector is already initialized.";
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
                    NLogger.Error($"Detector close failed: {errMsg}");
                    return;
                }
                Initialized = false;
                IsGrabbing = false; // 确保关闭时重置抓拍状态
            }
        }

        public void Grab()
        {
            if (Initialized)
            {
                if (!DoGrab(out var errMsg))
                {
                    NLogger.Error($"Detector grab failed: {errMsg}");
                    return;
                }
            }
        }

        /// <summary>
        /// 拍照时触发的事件
        /// </summary>
        /// <param name="image"></param>
        public void InvokeOnGrabbed(UnmanagedArray2D<ushort> image)
        {
            if (OnGrabbed == null) return;

            foreach (Action<DetectorImage> item in OnGrabbed.GetInvocationList().Cast<Action<DetectorImage>>())
            {
                item.Invoke(new DetectorImage(image));
            }
        }

        /// <summary>
        /// 配置改变时触发的事件
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnConfigChanged()
        {
            NLogger.Info(
                $"平板探测器配置改变：{nameof(Config.Width)}: {Config?.Width}, {nameof(Config.Height)}: {Config?.Height}");
        }

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机初始化逻辑。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoInit(out string? errMsg);

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机关闭逻辑。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoClose(out string? errMsg);

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，触发相机拍照逻辑。
        /// </summary>
        public abstract bool DoGrab(out string? errMsg);
    }
}
