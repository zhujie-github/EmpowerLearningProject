using System.Runtime.InteropServices;

namespace Company.Algorithm.Unwrapper
{
    /// <summary>
    /// 调用C++的静态类
    /// </summary>
    public static class CppMethods
    {
        private const string DllName = "Company.Algorithm.Wrapper.dll";

        [DllImport(DllName, EntryPoint = "#1", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Hello();

        /// <summary>
        /// 遍历整张图像，并给每个像素点增加一个V
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="v"></param>
        [DllImport(DllName, EntryPoint = "#2", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CppTest(CppImage16UC1 src, CppImage16UC1 dst, ushort v);

        /// <summary>
        /// 索贝尔算法
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="v"></param>
        [DllImport(DllName, EntryPoint = "#3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CppSobel(CppImage16UC1 src, CppImage16UC1 dst, int v);
    }
}
