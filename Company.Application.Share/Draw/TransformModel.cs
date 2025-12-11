using System.Windows.Media;

namespace Company.Application.Share.Draw
{
    /// <summary>
    /// 坐标变换模型
    /// </summary>
    public readonly struct TransformModel(TransformGroup group)
    {
        public TransformGroup TransformGroup { get; } = group;

        /// <summary>
        /// 当前缩放对象
        /// </summary>
        public ScaleTransform? ScaleTransform
        {
            get
            {
                if (TransformGroup != null && TransformGroup.Children.Count == 2 && TransformGroup.Children[0] is ScaleTransform)
                {
                    return TransformGroup.Children[0] as ScaleTransform;
                }

                return null;
            }
        }

        /// <summary>
        /// 当前平移对象
        /// </summary>
        public TranslateTransform? TranslateTransform
        {
            get
            {
                if (TransformGroup != null && TransformGroup.Children.Count == 2 && TransformGroup.Children[1] is TranslateTransform)
                {
                    return TransformGroup.Children[1] as TranslateTransform;
                }

                return null;
            }
        }
    }
}