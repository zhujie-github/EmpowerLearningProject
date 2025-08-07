namespace Company.Core.Models
{
    public class BindableSourceItem<T>
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 值
        /// </summary>
        public T? Value { get; set; }

        public BindableSourceItem()
        {
        }

        public BindableSourceItem(string description, T? value)
        {
            Description = description;
            Value = value;
        }
    }
}
