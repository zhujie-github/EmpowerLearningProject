using DynamicData.Diagnostics;
using System.Runtime.InteropServices;

namespace Company.Core.Helpers
{
    /// <summary>
    /// 内存帮助类
    /// </summary>
    public static class MemoryHelper
    {
        /// <summary>
        /// 清零内存
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="length"></param>
        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory", CharSet = CharSet.Ansi)]
        public static extern void ZeroMemory(IntPtr ptr, long length);

        /// <summary>
        /// 复制内存
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="length"></param>
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, long length);

        /// <summary>
        /// 填充内存
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        [DllImport("kernel32.dll", EntryPoint = "RtlFillMemory", CharSet = CharSet.Ansi)]
        public static extern void FillMemory(IntPtr dest, long length, byte value);
    }
}
