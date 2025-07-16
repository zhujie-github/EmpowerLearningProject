namespace Company.Hardware.Detector
{
    /// <summary>
    /// FPD平板探测器的配置类
    /// </summary>
    public class DetectorConfig
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string Photo { get; set; } = "1.tiff";
    }
}
