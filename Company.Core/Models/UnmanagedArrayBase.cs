using Company.Core.Helpers;
using System.Runtime.InteropServices;

namespace Company.Core.Models
{
    /// <summary>
    /// 非托管数组基类
    /// </summary>
    public abstract class UnmanagedArrayBase : IDisposable
    {
        /// <summary>
        /// 图像数据的指针
        /// </summary>
        public IntPtr Header { get; private set; }

        /// <summary>
        /// 元素的数量
        /// </summary>
        public long Count { get; }

        /// <summary>
        /// 单个元素的大小（字节数）
        /// </summary>
        private readonly int Size;

        /// <summary>
        /// 数组总长度（字节数）
        /// </summary>
        public long Length => Count * Size;

        protected UnmanagedArrayBase(long count, int size, bool isResetMemory)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }
            if (size <= 0) {
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than zero.");
            }
            Count = count;
            Size = size;
            Header = Marshal.AllocHGlobal(new IntPtr(Length));
            if (isResetMemory)
            {
                // 初始化内存为零
                MemoryHelper.ZeroMemory(Header, Length);
            }
        }

        ~UnmanagedArrayBase()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // 释放托管资源
            }

            // 释放非托管资源
            if (Header != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Header);
                Header = IntPtr.Zero;
            }

            _disposed = true;
        }
    }
}
