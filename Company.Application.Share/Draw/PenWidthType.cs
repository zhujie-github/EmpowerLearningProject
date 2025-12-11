namespace Company.Application.Share.Draw
{
    /// <summary>
    /// 画笔宽度
    /// </summary>
    public enum PenWidthType
    {
        Small,
        Medium,
        Large
    }

    public static class PenWidthTypeExtension
    {
        /// <summary>
        /// 画笔宽度
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetWidth(this PenWidthType type)
        {
            return type switch
            {
                PenWidthType.Small => 1,
                PenWidthType.Medium => 2,
                PenWidthType.Large => 3,
                _ => 2,
            };
        }

        /// <summary>
        /// 获取字体大小
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetFontSize(this PenWidthType type)
        {
            return type switch
            {
                PenWidthType.Small => 16,
                PenWidthType.Medium => 22,
                PenWidthType.Large => 28,
                _ => 16,
            };
        }
    }
}