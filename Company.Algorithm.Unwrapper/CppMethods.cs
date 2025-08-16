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
    }
}
