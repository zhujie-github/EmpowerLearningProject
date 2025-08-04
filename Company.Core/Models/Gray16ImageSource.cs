using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Company.Core.Models
{
    /// <summary>
    /// 16位灰度位图，用于WPF显示
    /// </summary>
    public class Gray16ImageSource
    {
        private readonly int _width;

        private readonly int _height;

        private readonly WriteableBitmap _bitmap;

        public ImageSource ImageSource => _bitmap;

        public Gray16ImageSource(int width, int height)
        {
            _width = width;
            _height = height;
            _bitmap = new WriteableBitmap(width, height, 96d, 96d, PixelFormats.Gray16, null);
        }

        /// <summary>
        /// 写入图像到WriteableBitmap，前端会刷新
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isCenter"></param>
        public void Write(UnmanagedArray2D<ushort>? data, bool isCenter = true)
        {
            if (data == null) return;

            if (isCenter)
            {
                WriteToCenter(data);
            }
            else
            {
                WriteToWhole(data);
            }
        }

        private void WriteToWhole(UnmanagedArray2D<ushort> data)
        {
            if (data.Width != _width || data.Height != _height)
            {
                throw new Exception("图像尺寸不一致");
            }

            var rect = new Int32Rect(0, 0, data.Width, data.Height);
            _bitmap.WritePixels(rect, data.Header, (int)data.Length, data.Width * sizeof(ushort));
        }

        private void WriteToCenter(UnmanagedArray2D<ushort> data)
        {
            var x0 = (data.Width - _width) / 2;
            var y0 = (data.Height - _height) / 2;
            var x = Math.Max(x0, 0);
            var y = Math.Max(y0, 0);
            var w = Math.Min(data.Width, _width);
            var h = Math.Min(data.Height, _height);
            var destX = x0 > 0 ? 0 : -x0;
            var destY = y0 > 0 ? 0 : -y0;

            var rect = new Int32Rect(x, y, w, h);
            _bitmap.WritePixels(rect, data.Header, (int)data.Length, data.Width * sizeof(ushort), destX, destY);
        }
    }
}
