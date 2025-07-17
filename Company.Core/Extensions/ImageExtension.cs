using Company.Core.Helpers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;

namespace Company.Core.Extensions
{
    public static class ImageExtension
    {
        /// <summary>
        /// 将 System.Windows.Media.Imaging.BitmapSource 转换为 System.Drawing.Bitmap
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static Bitmap ToBitmap(this System.Windows.Media.Imaging.BitmapSource bitmapSource)
        {
            Assert.NotNull(bitmapSource);

            PixelFormat pixelFormat;
            if (bitmapSource.Format == System.Windows.Media.PixelFormats.Gray8)
            {
                pixelFormat = PixelFormat.Format8bppIndexed;
            }
            else if (bitmapSource.Format == System.Windows.Media.PixelFormats.Gray16)
            {
                pixelFormat = PixelFormat.Format16bppGrayScale;
            }
            else if (bitmapSource.Format == System.Windows.Media.PixelFormats.Bgr24)
            {
                pixelFormat = PixelFormat.Format24bppRgb;
            }
            else if (bitmapSource.Format == System.Windows.Media.PixelFormats.Bgr32)
            {
                pixelFormat = PixelFormat.Format32bppArgb;
            }
            else
            {
                throw new NotSupportedException($"Unsupported pixel format: {bitmapSource.Format}");
            }

            var bitmap = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, pixelFormat);
            var bitmapData = bitmap.LockBits(
                new Rectangle(System.Drawing.Point.Empty, bitmap.Size),
                ImageLockMode.WriteOnly,
                pixelFormat);
            bitmapSource.CopyPixels(
                Int32Rect.Empty,
                bitmapData.Scan0,
                bitmapData.Stride * bitmap.Height,
                bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// 将 System.Drawing.Bitmap 锁定为 BitmapData
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapData LockBits(this Bitmap bitmap)
        {
            var rect = new Rectangle(System.Drawing.Point.Empty, bitmap.Size);
            var bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            return bitmapData;
        }
    }
}
