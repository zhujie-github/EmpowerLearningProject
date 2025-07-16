using Company.Core.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Company.Core.Helpers
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImageHelper
    {
        public static Bitmap? Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("Image file not found.", filePath);
            }

            var bytes = FileHelper.Load(filePath);
            using (var memoryStream = new MemoryStream(bytes))
            {
                return Image.FromStream(memoryStream) as Bitmap;
            }
        }

        public static UnmanagedArray2D<ushort>? LoadTiff(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("TIFF image file not found.", filePath);
            }
            var bytes = FileHelper.Load(filePath);
            using var memoryStream = new MemoryStream(bytes);
            if (Image.FromStream(memoryStream) is not Bitmap bitmap)
            {
                return null;
            }
            var unmanagedArray = new UnmanagedArray2D<ushort>(bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            MemoryHelper.CopyMemory(unmanagedArray.Header, data.Scan0, unmanagedArray.Length);
            bitmap.UnlockBits(data);
            return unmanagedArray;
        }
    }
}
