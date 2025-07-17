namespace Company.Hardware.Detector
{
    /// <summary>
    /// FPD平板探测器的配置类
    /// </summary>
    public class DetectorConfig
    {
        /// <summary>
        /// 照片名称
        /// </summary>
        public string Photo { get; set; } = "CT.tiff";

        /// <summary>
        /// 照片宽度（像素）
        /// </summary>
        public int Width { get; set; } = 638;

        /// <summary>
        /// 照片高度（像素）
        /// </summary>
        public int Height { get; set; } = 844;
    }
}
