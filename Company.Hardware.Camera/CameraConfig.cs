﻿namespace Company.Hardware.Camera
{
    public class CameraConfig
    {
        /// <summary>
        /// 默认图像
        /// </summary>
        public string Photo { get; set; } = "PCB主板.jpg";

        /// <summary>
        /// 图像宽度（像素）
        /// </summary>
        public int Width { get; set; } = 500;

        /// <summary>
        /// 图像高度（像素）
        /// </summary>
        public int Height { get; set; } = 386;
    }
}
