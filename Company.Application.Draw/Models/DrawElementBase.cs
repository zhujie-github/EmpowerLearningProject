using Company.Application.Share.Draw;
using Company.Core.Extensions;
using System.Drawing;
using System.Windows;

namespace Company.Application.Draw.Models
{
    public abstract class DrawElementBase(PenWidthType penWidthType, PenColorType penColorType)
    {
        protected PenWidthType PenWidthType { get; } = penWidthType;
        protected PenColorType PenColorType { get; } = penColorType;

        public abstract void Draw(Graphics graphics, SolidBrush? brush = null);
    }

    /// <summary>
    /// 画矩形
    /// </summary>
    public class DrawRectangleElement(Rect rect, PenWidthType penWidthType, PenColorType penColorType)
        : DrawElementBase(penWidthType, penColorType)
    {
        private Rect Rect { get; } = rect;

        public override void Draw(Graphics graphics, SolidBrush? brush = null)
        {
            var pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawRectangles(pen, [Rect.ToRectangleF()]);
        }
    }

    /// <summary>
    /// 画椭圆
    /// </summary>
    public class DrawEllipseElement(Rect rect, PenWidthType penWidthType, PenColorType penColorType)
        : DrawElementBase(penWidthType, penColorType)
    {
        private Rect Rect { get; } = rect;

        public override void Draw(Graphics graphics, SolidBrush? brush = null)
        {
            var pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawEllipse(pen, Rect.ToRectangleF());
        }
    }

    /// <summary>
    /// 画箭头
    /// </summary>
    public class DrawArrowElement(
        System.Windows.Point startPoint,
        System.Windows.Point endPoint,
        PenWidthType penWidthType,
        PenColorType penColorType)
        : DrawElementBase(penWidthType, penColorType)
    {
        private System.Windows.Point StartPoint { get; } = startPoint;
        private System.Windows.Point EndPoint { get; } = endPoint;

        public override void Draw(Graphics graphics, SolidBrush? brush = null)
        {
            var pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawLine(pen, StartPoint.ToDrawingPointF(), EndPoint.ToDrawingPointF());
        }
    }

    /// <summary>
    /// 画线段
    /// </summary>
    public class DrawPenElement(List<System.Windows.Point> points, PenWidthType penWidthType, PenColorType penColorType)
        : DrawElementBase(penWidthType, penColorType)
    {
        private List<System.Windows.Point> Points { get; } = points;

        public override void Draw(Graphics graphics, SolidBrush? brush = null)
        {
            var points = Points.Select(p => p.ToDrawingPointF()).ToArray();
            var pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawLines(pen, points);
        }
    }

    /// <summary>
    /// 绘制文字
    /// </summary>
    public class DrawTextElement(
        string text,
        System.Windows.Point point,
        System.Windows.Size viewSize,
        PenWidthType penWidthType,
        PenColorType penColorType)
        : DrawElementBase(penWidthType, penColorType)
    {
        private string Text { get; } = text;

        private System.Windows.Point Point { get; } = point;

        private System.Windows.Size ViewSize { get; } = viewSize;

        public override void Draw(Graphics graphics, SolidBrush? brush = null)
        {
            var font = new Font("Microsoft YaHei UI", PenWidthType.GetFontSize(), GraphicsUnit.Pixel);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //消除锯齿
            graphics.DrawString(Text, font, brush ?? PenColorType.GetBrush(), Point.ToDrawingPointF());
        }
    }
}