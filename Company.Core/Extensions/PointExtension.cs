namespace Company.Core.Extensions
{
    public static class PointExtension
    {
        public static System.Drawing.RectangleF ToRectangleF(this System.Windows.Rect rect)
        {
            return new System.Drawing.RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        public static System.Drawing.PointF ToDrawingPointF(this System.Windows.Point pt)
        {
            return new System.Drawing.PointF((float)pt.X, (float)pt.Y);
        }
    }
}