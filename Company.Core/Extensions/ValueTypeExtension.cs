namespace Company.Core.Extensions
{
    public static class ValueTypeExtension
    {
        /// <summary>
        /// 获取值类型的完全限定名称+扩展名
        /// </summary>
        /// <param name="valueType"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetFullName(this ValueType valueType, string? extension = null)
        {
            var ext = extension ?? $".{valueType}";
            return $"{valueType.GetType().FullName}{ext}";
        }
    }
}
