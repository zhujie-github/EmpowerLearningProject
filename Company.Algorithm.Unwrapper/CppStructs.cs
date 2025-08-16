using Company.Core.Models;

namespace Company.Algorithm.Unwrapper
{
    /// <summary>
    /// 8位1通道图像
    /// </summary>
    public struct CppImage8UC1
    {
        public IntPtr Header;
        public int Width;
        public int Height;

        public CppImage8UC1(UnmanagedArray2D<byte> image)
        {
            Header = image.Header;
            Width = image.Width;
            Height = image.Height;
        }

        public CppImage8UC1(IntPtr header, int width, int height)
        {
            Header = header;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// 8位3通道图像
    /// </summary>
    public struct CppImage8UC3
    {
        public IntPtr Header;
        public int Width;
        public int Height;

        public CppImage8UC3(UnmanagedArray2D<byte> image)
        {
            Header = image.Header;
            Width = image.Width;
            Height = image.Height;
        }

        public CppImage8UC3(IntPtr header, int width, int height)
        {
            Header = header;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// 8位4通道图像
    /// </summary>
    public struct CppImage8UC4
    {
        public IntPtr Header;
        public int Width;
        public int Height;

        public CppImage8UC4(UnmanagedArray2D<byte> image)
        {
            Header = image.Header;
            Width = image.Width;
            Height = image.Height;
        }

        public CppImage8UC4(IntPtr header, int width, int height)
        {
            Header = header;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// 16位1通道图像
    /// </summary>
    public struct CppImage16UC1
    {
        public IntPtr Header;
        public int Width;
        public int Height;

        public CppImage16UC1(UnmanagedArray2D<ushort> image)
        {
            Header = image.Header;
            Width = image.Width;
            Height = image.Height;
        }

        public CppImage16UC1(IntPtr header, int width, int height)
        {
            Header = header;
            Width = width;
            Height = height;
        }
    }
}
