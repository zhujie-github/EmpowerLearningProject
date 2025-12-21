using Company.Core.Helper;
using Company.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Company.Core.Helpers;

namespace Company.Core.Extensions
{
    public static class UnmanagedArray2DExtension
    {
        public static Bitmap ToGray8Bitmap(this UnmanagedArray2D<ushort> source)
        {
            var bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format8bppIndexed);
            var bmpdata = bmp.LockBits();
            var offset = bmpdata.Stride - source.Width;//跳过的像素

            unsafe
            {
                byte* bmpPtr = (byte*)bmpdata.Scan0;
                byte* srcPtr = (byte*)source.Header;

                for (int i = 0; i < source.Height; i++)
                {
                    for (int j = 0; j < source.Width; j++)
                    {
                        *bmpPtr = *(srcPtr + 1);//取高8位
                        bmpPtr++;
                        srcPtr += 2;//16位每次跳过2个字节
                    }

                    bmpPtr += offset;//跳过无用的像素，因为Bitmap类型是按4字节排列的
                }
            }

            bmp.UnlockBits(bmpdata);
            bmp.Palette = ImageHelper.CreateGray8Palette();
            return bmp;
        }
    }
}
