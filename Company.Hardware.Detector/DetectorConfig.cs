namespace Company.Hardware.Detector
{
    /// <summary>
    /// FPD平板探测器的配置类
    /// </summary>
    public class DetectorConfig
    {
        /// <summary>
        /// 默认图像
        /// </summary>
        public string Photo { get; set; } = "CT.tiff";

        /// <summary>
        /// 图像宽度（像素）
        /// </summary>
        public int Width { get; set; } = 638;

        /// <summary>
        /// 图像高度（像素）
        /// </summary>
        public int Height { get; set; } = 844;
    }
}
