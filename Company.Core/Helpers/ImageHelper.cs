using Company.Core.Extensions;
using Company.Core.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Company.Core.Helpers
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// 读本地图片转成位图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap Load(string path)
        {
            byte[] bytes = FileHelper.Load(path);
            using(MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms) as Bitmap;
            }
        }

        /// <summary>
        /// 读本地TIFF图像数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UnmanagedArray2D<ushort> LoadTiff(string path)
        {
            if(!File.Exists(path)) return null;
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];
            int stride = bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel / 8;
            ushort[] pixels = new ushort[bitmapSource.PixelWidth * bitmapSource.PixelHeight];
            bitmapSource.CopyPixels(pixels, stride, 0);
            var bitmap = bitmapSource.ToBitmap();//转换为位图
            var data = bitmap.LockBits();
            UnmanagedArray2D<ushort> image = new UnmanagedArray2D<ushort>(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            MemoryHelper.CopyMemory(image.Header, data.Scan0, image.Length);
            bitmap.UnlockBits(data);
            return image;
        }

        public static System.Drawing.Imaging.ColorPalette CreateGray8Palette()
        {
            System.Drawing.Imaging.ColorPalette palette;
            using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
            {
                palette = bmp.Palette;
            }

            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
            }

            return palette;
        }

    }
}
