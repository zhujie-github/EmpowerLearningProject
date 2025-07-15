namespace Company.Hardware.Detector
{
    public interface IDetector
    {
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="detectorConfig"></param>
        /// <returns></returns>
        bool Init(DetectorConfig detectorConfig);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}
