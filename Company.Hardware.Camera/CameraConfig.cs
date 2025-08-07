namespace Company.Hardware.Camera
{
    public class CameraConfig
    {
        /// <summary>
        /// 默认图像
        /// </summary>
        public string Photo { get; set; } = "1.jpg";

        /// <summary>
        /// 图像宽度（像素）
        /// </summary>
        public int Width { get; set; } = 2644;

        /// <summary>
        /// 图像高度（像素）
        /// </summary>
        public int Height { get; set; } = 2041;
    }
}
