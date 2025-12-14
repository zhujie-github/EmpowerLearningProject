using Company.Core.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Company.Core.Models
{
    public class BitmapSourceGDI
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Graphics _graphics;
        private readonly WriteableBitmap _source;

        /// <summary>
        /// 用于绑定到WPF前端
        /// </summary>
        public System.Windows.Media.ImageSource ImageSource => _source;

        public BitmapSourceGDI(int width, int height)
        {
            _width = width;
            _height = height;
            _source = new WriteableBitmap(_width, _height, 96d, 96d, System.Windows.Media.PixelFormats.Pbgra32, null);
            var bitmap = new Bitmap(_source.PixelWidth,
                _source.PixelHeight,
                _source.BackBufferStride,
                PixelFormat.Format32bppArgb,
                _source.BackBuffer);
            _graphics = Graphics.FromImage(bitmap);
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
            var int32Rect = new Int32Rect(rect.X - x, rect.Y - y, rect.Width, rect.Height);
            var bitmapData = bitmap.LockBits();
            _source.WritePixels(
                int32Rect,
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
            var int32Rect = new Int32Rect(rect.X - x, rect.Y - y, rect.Width, rect.Height);
            _source.WritePixels(
                int32Rect,
                bitmap.Header,
                (int)bitmap.Length,
                bitmap.Stride,
                rect.X,
                rect.Y);
        }

        public BitmapSourceDrawProvider Create(Color color)
        {
            return BitmapSourceDrawProvider.Create(_graphics, _source, color);
        }

        public void Clear(Color color)
        {
            using (Create(color)) { }
        }
    }
}
