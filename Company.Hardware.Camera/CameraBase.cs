namespace Company.Hardware.Camera
{
    public abstract class CameraBase : ICamera
    {
        private CameraConfig? CameraConfig { get; set; }

        public bool Initialized { get; private set; }

        public event Action<nint>? ImageCaptured;

        public bool Init(CameraConfig cameraConfig)
        {
            CameraConfig = cameraConfig;
            if (Initialized)
            {
                throw new Exception("Camera is already initialized.");
            }

            if (DoInit())
            {
                Initialized = true;
                return true;
            }

            return false;
        }

        public void Close()
        {
            if (Initialized)
            {
                DoClose();
                Initialized = false;
            }
        }

        public void OnImageCaptured(nint image)
        {
            ImageCaptured?.Invoke(image);
        }

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机初始化逻辑。
        /// </summary>
        /// <returns></returns>
        protected abstract bool DoInit();

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，执行相机关闭逻辑。
        /// </summary>
        protected abstract void DoClose();

        /// <summary>
        /// 抽象方法，由具体相机实现类实现，触发相机拍照逻辑。
        /// </summary>
        /// <returns></returns>
        public abstract bool Trigger();
    }
}
