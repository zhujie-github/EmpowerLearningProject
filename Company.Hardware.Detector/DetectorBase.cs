namespace Company.Hardware.Detector
{
    public abstract class DetectorBase : IDetector
    {
        public DetectorConfig? DetectorConfig { get; private set; }

        public bool Initialized { get; private set; } = false;
        public bool IsCapturing { get; private set; } = false;

        public event Action<DetectorImage>? ImageCaptured;

        public bool Init(DetectorConfig detectorConfig)
        {
            DetectorConfig = detectorConfig;

            if (Initialized)
            {
                throw new InvalidOperationException("Detector is already initialized.");
            }

            if (!DoInit(out var errMsg))
            {
                throw new InvalidOperationException($"Detector initialization failed: {errMsg}");
            }

            Initialized = true;
            return true;
        }

        public void Close()
        {
            if (Initialized)
            {
                if (!DoClose(out var errMsg))
                {
                    throw new InvalidOperationException($"Detector close failed: {errMsg}");
                }
                Initialized = false;
            }
        }

        public void Capture()
        {
            if (Initialized)
            {
                if (!DoCapture(out var errMsg))
                {
                    throw new InvalidOperationException($"Detector capture failed: {errMsg}");
                }
                IsCapturing = true;
            }
        }

        /// <summary>
        /// 拍照时触发的事件
        /// </summary>
        /// <param name="image"></param>
        public void OnImageCaptured(DetectorImage image)
        {
            ImageCaptured?.Invoke(image);
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
        /// 抽象方法，由具体相机实现类实现，触发相机拍照逻辑。
        /// </summary>
        public abstract bool DoCapture(out string errMsg);
    }
}
