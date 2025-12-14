using System.Drawing;
using System.Windows;
using Company.Application.Share.Draw;
using Company.Application.Share.Mouse;
using Company.Core.Enums;
using Company.Core.Extensions;
using Company.Core.Models;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace Company.Application.Draw.Models
{
    public class DrawMouseOperation : MouseOperationBase
    {
        private readonly List<Point> _points = [];
        private DrawToolModel DrawToolModel { get; }

        public DrawMouseOperation(
            DrawToolModel drawToolModel,
            MouseOperationType mouseOperationType,
            BitmapSourceGDI previewBitmap,
            ITransformProvider transformGroup) : base(mouseOperationType, previewBitmap, transformGroup)
        {
            if (!mouseOperationType.IsDrawOperationType())
                throw new ArgumentException($"当前鼠标操作不是绘制", nameof(mouseOperationType));
            DrawToolModel = drawToolModel;
        }

        /// <summary>
        /// 鼠标左键按下并移动
        /// </summary>
        /// <param name="point"></param>
        /// <param name="cursor"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void MouseLeftButtonDownAndMove(Point point, ref Cursor cursor)
        {
            using var provider = PreviewBitmap.Create(Color.Transparent);
            var pen = new Pen(DrawToolModel.PenColorType.GetBrush(), DrawToolModel.PenWidthType.GetWidth());
            var viewRect = new Rect(MouseDownPoint, point);
            switch (MouseOperationType)
            {
                case MouseOperationType.DrawRectangle:
                    provider.Graphics.DrawRectangles(pen, viewRect.ToRectangleF());
                    cursor = Cursors.Cross;
                    break;
                case MouseOperationType.DrawEllipse:
                    provider.Graphics.DrawEllipse(pen, viewRect.ToRectangleF());
                    break;
                case MouseOperationType.DrawArrow:
                    provider.Graphics.DrawLine(pen, MouseDownPoint.ToDrawingPointF(), point.ToDrawingPointF());
                    break;
                case MouseOperationType.DrawPen:
                    _points.Add(point);
                    for (var i = 0; i < _points.Count - 1; i++)
                    {
                        provider.Graphics.DrawLine(pen, _points[i].ToDrawingPointF(), _points[i + 1].ToDrawingPointF());
                    }

                    cursor = Cursors.Pen;
                    break;
                case MouseOperationType.DrawText:
                case MouseOperationType.DrawSelect:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        /// <param name="point"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void MouseLeftButtonUp(Point point)
        {
            var viewRect = new Rect(MouseDownPoint, point);

            DrawElementBase? drawElement = null;
            switch (MouseOperationType)
            {
                case MouseOperationType.DrawRectangle:
                    if (viewRect is { Width: > 0, Height: > 0 })
                    {
                        drawElement = new DrawRectangleElement(viewRect, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawEllipse:
                    if (viewRect is { Width: > 0, Height: > 0 })
                    {
                        drawElement = new DrawEllipseElement(viewRect, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawArrow:
                    if (viewRect is { Width: > 0, Height: > 0 })
                    {
                        drawElement = new DrawArrowElement(MouseDownPoint, point, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawPen:
                    var copy = _points.DeepClone();
                    if (copy?.Count > 2)
                    {
                        drawElement = new DrawPenElement(copy, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawText:
                case MouseOperationType.DrawSelect:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (drawElement != null)
            {
                DrawToolModel.DrawElements.Add(drawElement);
            }

            //每次预览图层画完后要清空，因为在实际画多个元素的图层里，会把所有元素重新绘一遍。
            PreviewBitmap.Clear(Color.Transparent);
        }

        public override void MouseMove(Point point, ref Cursor cursor)
        {
            cursor = MouseOperationType switch
            {
                MouseOperationType.DrawRectangle or MouseOperationType.DrawEllipse
                    or MouseOperationType.DrawArrow => Cursors.Cross,
                MouseOperationType.DrawPen => Cursors.Pen,
                _ => cursor
            };
        }
    }
}