namespace Company.Hardware.Camera
{
    public abstract class CameraBase : ICamera
    {
        public CameraConfig? CameraConfig { get; private set; }

        public bool Initialized { get; private set; } = false;

        public event Action<Photo>? ImageCaptured;

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

        public void OnImageCaptured(Photo photo)
        {
            ImageCaptured?.Invoke(photo);
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
