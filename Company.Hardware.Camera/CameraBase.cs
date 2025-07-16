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
                //todo log error
            }

            if (!DoInit(out var errMsg))
            {
                throw new Exception($"Camera initialization failed: {errMsg}");
                //todo log error
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
                    throw new Exception($"Camera close failed: {errMsg}");
                    //todo log error
                }
                Initialized = false;
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
        public abstract void Trigger();
    }
}
