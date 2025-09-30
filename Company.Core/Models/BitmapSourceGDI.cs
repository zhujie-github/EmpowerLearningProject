using Company.Core.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Company.Core.Models
{
    public class BitmapSourceGDI
    {
        public int _width;
        public int _height;
        public Bitmap _bitmap;
        public Graphics _graphics;
        public WriteableBitmap _source;

        /// <summary>
        /// 用于绑定到WPF前端
        /// </summary>
        public System.Windows.Media.ImageSource ImageSource => _source;

        public BitmapSourceGDI(int width, int height)
        {
            _width = width;
            _height = height;
            _source = new WriteableBitmap(_width, _height, 96d, 96d, System.Windows.Media.PixelFormats.Pbgra32, null);
            _bitmap = new Bitmap(_source.PixelWidth,
                _source.PixelHeight,
                _source.BackBufferStride,
                PixelFormat.Format32bppArgb,
                _source.BackBuffer);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // 抗锯齿
        }

        public void WritePixels(Bitmap bitmap, int x, int y)
        {
            if (!bitmap.IsBitmap32Bit())
            {
                throw new Exception("只支持32位位图");
            }

            var rect = new Rectangle(x, y, bitmap.Width, bitmap.Height);
            rect.Intersect(new Rectangle(0, 0, _width, _height));
            var int32rect = new Int32Rect(rect.X - x, rect.Y - y, rect.Width, rect.Height);
            var bitmapData = bitmap.LockBits();
            _source.WritePixels(
                int32rect,
                bitmapData.Scan0,
                bitmapData.Stride * bitmap.Height,
                bitmapData.Stride,
                rect.X,
                rect.Y);
            bitmap.UnlockBits(bitmapData);
        }

        /// <summary>
        /// 将图像绘制到当前画布指定位置
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void WritePixels(UnmanagedArray2D<ColorBGRA> bitmap, int x, int y)
        {
            var rect = new Rectangle(x, y, bitmap.Width, bitmap.Height);
            rect.Intersect(new Rectangle(0, 0, _width, _height));
            var int32rect = new Int32Rect(rect.X - x, rect.Y - y, rect.Width, rect.Height);
            _source.WritePixels(
                int32rect,
                bitmap.Header,
                (int)bitmap.Length,
                bitmap.Stride,
                rect.X,
                rect.Y);
        }
    }
}
