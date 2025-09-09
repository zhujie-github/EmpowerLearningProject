namespace Company.Application.Filter.Attributes
{
    /// <summary>
    /// 标注的对象应该为滤波算法的实体模型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FilterAttribute : Attribute
    {
        /// <summary>
        /// 用于排序的索引
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
