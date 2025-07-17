namespace Company.Hardware.Camera
{
    public class CameraConfig
    {
        /// <summary>
        /// 照片名称
        /// </summary>
        public string Photo { get; set; } = "PCB主板.jpg";

        /// <summary>
        /// 相机宽度（像素）
        /// </summary>
        public int Width { get; set; } = 500;

        /// <summary>
        /// 相机高度（像素）
        /// </summary>
        public int Height { get; set; } = 386;
    }
}
