using Company.Core.Helpers;
using Company.Core.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

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
            else if (bitmapSource.Format == System.Windows.Media.PixelFormats.Bgra32)
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

        /// <summary>
        /// 是否为32位位图
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static bool IsBitmap32Bit(this Bitmap bitmap)
        {
            return bitmap.PixelFormat == PixelFormat.Format32bppArgb ||
                bitmap.PixelFormat == PixelFormat.Format32bppPArgb ||
                bitmap.PixelFormat == PixelFormat.Format32bppRgb;
        }

        public static System.Windows.Media.Imaging.BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            Assert.NotNull(bitmap);
            System.Windows.Media.PixelFormat format;
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    format = System.Windows.Media.PixelFormats.Gray8;
                    break;
                case PixelFormat.Format24bppRgb:
                    format = System.Windows.Media.PixelFormats.Bgr24;
                    break;
                case PixelFormat.Format32bppArgb:
                    format = System.Windows.Media.PixelFormats.Bgr32;
                    break;
                case PixelFormat.Format16bppGrayScale:
                    format = System.Windows.Media.PixelFormats.Gray16;
                    break;
                default: throw new ArgumentException($"未实现Bitmap像素格式{bitmap.PixelFormat}");
            }

            var data = bitmap.LockBits();
            var bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(
                data.Width,
                data.Height,
                bitmap.HorizontalResolution,
                bitmap.VerticalResolution,
                format,
                null,
                data.Scan0,
                data.Stride * data.Height,
                data.Stride);
            bitmap.UnlockBits(data);
            return bitmapSource;

        }

        public static void Save(this UnmanagedArray2D<ushort> unmanagedArray2D, string filename)
        {
            try
            {
                Bitmap bitmap = new Bitmap(unmanagedArray2D.Width, unmanagedArray2D.Height, PixelFormat.Format16bppGrayScale);
                var data = bitmap.LockBits();
                MemoryHelper.CopyMemory(data.Scan0, unmanagedArray2D.Header, unmanagedArray2D.Length);
                bitmap.UnlockBits(data);
                var stream = new FileStream(filename, FileMode.Create);
                var encoder = new TiffBitmapEncoder();
                encoder.Compression = TiffCompressOption.Zip;
                encoder.Frames.Add(BitmapFrame.Create(bitmap.ToBitmapSource()));
                encoder.Save(stream);
                stream.Close();
                stream.Dispose();
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
