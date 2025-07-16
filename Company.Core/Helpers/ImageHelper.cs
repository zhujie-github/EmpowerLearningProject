using System.Drawing;
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
    }
}
