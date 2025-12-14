using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Company.Core.Extensions
{
    /// <summary>
    /// BitmapSource 绘制提供者
    /// </summary>
    public class BitmapSourceDrawProvider(WriteableBitmap bitmap, Graphics graphics) : IDisposable
    {
        private readonly WriteableBitmap _bitmap = bitmap;
        public Graphics Graphics { get; private set; } = graphics;

        public static BitmapSourceDrawProvider Create(Graphics graphics, WriteableBitmap source, Color? color)
        {
            source.Lock();
            if (color.HasValue)
            {
                graphics.Clear(color.Value);
            }

            return new BitmapSourceDrawProvider(source, graphics);
        }

        public void Dispose()
        {
            Graphics.Flush();
            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
            _bitmap.Unlock();
        }
    }
}