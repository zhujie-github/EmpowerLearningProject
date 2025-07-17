using Company.Core.Extensions;
using Company.Core.Models;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Company.Core.Helpers
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 读本地图片转成位图（默认32位）
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static Bitmap? Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Image file not found.", filePath);
            }

            var bytes = FileHelper.Load(filePath);
            using var memoryStream = new MemoryStream(bytes);
            return Image.FromStream(memoryStream) as Bitmap;
        }

        /// <summary>
        /// 读取本地TIFF图片转成无管理数组，当文件（filePath）不存在时，返回null
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static UnmanagedArray2D<ushort>? LoadTiff(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            var bitmapSource = decoder.Frames[0];
            var width = bitmapSource.PixelWidth;
            var height = bitmapSource.PixelHeight;
            var stride = width * bitmapSource.Format.BitsPerPixel / 8;
            var pixels = new ushort[stride * height];
            bitmapSource.CopyPixels(pixels, stride, 0);
            var bitmap = bitmapSource.ToBitmap();
            var bitmapData = bitmap.LockBits();
            var image = new UnmanagedArray2D<ushort>(width, height);
            MemoryHelper.CopyMemory(image.Header, bitmapData.Scan0, image.Length);
            bitmap.UnlockBits(bitmapData);
            return image;
        }
    }
}
