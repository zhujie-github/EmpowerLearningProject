using System.Runtime.CompilerServices;

namespace Company.Core.Helpers
{
    public static class Asserts
    {
        public static void NotNull<T>(T? obj, [CallerMemberName]string? memberName = null)
        {
            if (obj is null)
            {
                throw new Exception($"断言错误：{memberName}方法中的{typeof(T)}不能为空");
            }
        }
    }
}
