using Company.Application.Share.Draw;
using Company.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Company.Application.Draw.Models
{
    public abstract class DrawElementBase
    {
        protected PenWidthType PenWidthType { get; }
        protected PenColorType PenColorType { get; }

        public DrawElementBase(PenWidthType penWidthType, PenColorType penColorType)
        {
            PenWidthType = penWidthType;
            PenColorType = penColorType;
        }

        public abstract void Draw(Graphics graphics, SolidBrush brush = null);
    }

    /// <summary>
    /// 画矩形
    /// </summary>
    public class DrawRectangleElement : DrawElementBase
    {
        private Rect Rect { get; }

        public DrawRectangleElement(Rect rect, PenWidthType penWidthType, PenColorType penColorType) : base(penWidthType, penColorType)
        {
            Rect = rect;
        }

        public override void Draw(Graphics graphics, SolidBrush brush = null)
        {
            Pen pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawRectangles(pen, new RectangleF[] { Rect.ToRectangleF() });
        }
    }

    /// <summary>
    /// 画椭圆
    /// </summary>
    public class DrawEllipseElement : DrawElementBase
    {
        private Rect Rect { get; }
        public DrawEllipseElement(Rect rect, PenWidthType penWidthType, PenColorType penColorType) : base(penWidthType, penColorType)
        {
            Rect = rect;
        }

        public override void Draw(Graphics graphics, SolidBrush brush = null)
        {
            Pen pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawEllipse(pen, Rect.ToRectangleF());
        }
    }

    /// <summary>
    /// 画箭头
    /// </summary>
    public class DrawArrowElement : DrawElementBase
    {
        private System.Windows.Point StartPoint { get; }
        private System.Windows.Point EndPoint { get; }

        public DrawArrowElement(System.Windows.Point startPoint, System.Windows.Point endPoint,PenWidthType penWidthType, PenColorType penColorType) : base(penWidthType, penColorType)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override void Draw(Graphics graphics, SolidBrush brush = null)
        {
            Pen pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawLine(pen, StartPoint.ToDrawingPointF(), EndPoint.ToDrawingPointF());
        }
    }

    /// <summary>
    /// 画线段
    /// </summary>
    public class DrawPenElement : DrawElementBase
    {
        private List<System.Windows.Point> Points { get; }

        public DrawPenElement(List<System.Windows.Point> points, PenWidthType penWidthType, PenColorType penColorType) : base(penWidthType, penColorType)
        {
            Points = points;
        }

        public override void Draw(Graphics graphics, SolidBrush brush = null)
        {
            System.Drawing.PointF[] points = Points.Select(p=>p.ToDrawingPointF()).ToArray();
            Pen pen = new Pen(brush ?? PenColorType.GetBrush(), PenWidthType.GetWidth());
            graphics.DrawLines(pen, points);
        }
    }

    /// <summary>
    /// 绘制文字
    /// </summary>
    public class DrawTextElement :  DrawElementBase
    {
        private string Text { get; }

        private System.Windows.Point Point { get; }

        private System.Windows.Size ViewSize { get; }
        public DrawTextElement(string text, System.Windows.Point point, System.Windows.Size viewSize, PenWidthType penWidthType, PenColorType penColorType) : base(penWidthType, penColorType)
        {
            Text = text;
            Point = point;
            ViewSize = viewSize;
        }

        public override void Draw(Graphics graphics, SolidBrush brush = null)
        {
            var font = new Font("Microsoft YaHei UI", PenWidthType.GetFontSize(), GraphicsUnit.Pixel);
            graphics.TextRenderingHint =  System.Drawing.Text.TextRenderingHint.AntiAlias;//消除锯齿
            graphics.DrawString(Text, font, brush ?? PenColorType.GetBrush(), Point.ToDrawingPointF());
        }
    }
}
