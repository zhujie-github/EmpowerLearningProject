using Company.Core.Helpers;
using System.Runtime.InteropServices;

namespace Company.Core.Models
{
    public sealed class UnmanagedArray2D<T> : UnmanagedArrayBase where T : struct
    {
        /// <summary>
        /// 相机宽度
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 相机高度
        /// </summary>
        public int Height { get; }

        public int Stride => Width * Marshal.SizeOf<T>();

        public UnmanagedArray2D(int width, int height, bool isResetMemory = true) : base(width * height, Marshal.SizeOf(typeof(T)), isResetMemory)
        {
            Width = width;
            Height = height;
        }

        public UnmanagedArray2D<T> DeepClone()
        {
            var result = new UnmanagedArray2D<T>(Width, Height);
            MemoryHelper.CopyMemory(result.Header, Header, Length);
            return result;
        }
    }
}
