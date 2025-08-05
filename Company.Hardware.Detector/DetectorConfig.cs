using System.ComponentModel;

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
        [DisplayName("默认图像")]
        public string Photo { get; set; } = "CT.tiff";

        /// <summary>
        /// 图像宽度（像素）
        /// </summary>
        [DisplayName("图像宽度")]
        public int Width { get; set; } = 1500;

        /// <summary>
        /// 图像高度（像素）
        /// </summary>
        [DisplayName("图像高度")]
        public int Height { get; set; } = 1500;
    }
}
