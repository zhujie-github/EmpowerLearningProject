using Company.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Company.Hardware.Camera
{
    /// <summary>
    /// 一张图像
    /// </summary>
    public struct Photo
    {
        /// <summary>
        /// 图像数据的指针
        /// </summary>
        public nint Header { get; }

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// 占据内存总长度（字节数）
        /// </summary>
        public readonly int Length => Width * Height * Marshal.SizeOf(typeof(ColorBGRA));

        public Photo(UnmanagedArray2D<ColorBGRA> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data), "图像数据不能为空");
            }
            if (data.Width <= 0 || data.Height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "图像的宽度和高度必须大于零");
            }
            Header = data.Header;
            Width = data.Width;
            Height = data.Height;
            if (Header == nint.Zero)
            {
                throw new InvalidOperationException("图像数据的指针无效");
            }
        }
    }
}
