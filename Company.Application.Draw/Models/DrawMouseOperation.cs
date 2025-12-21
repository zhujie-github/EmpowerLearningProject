using Company.Application.Share.Draw;
using Company.Application.Share.Mouse;
using Company.Core.Enums;
using Company.Core.Extensions;
using Company.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Company.Application.Draw.Models
{
    /// <summary>
    /// 绘制鼠标操作
    /// </summary>
    public class DrawMouseOperation : MouseOperationBase
    {
        private List<System.Windows.Point> _points = new List<Point>();
        private DrawToolModel DrawToolModel { get; }

        public DrawMouseOperation(DrawToolModel drawToolModel,
            MouseOperationType mouseOperationType,
            BitmapSourceGDI bitmap,
            ITransformProvider transformGroup)
            : base(mouseOperationType, bitmap, transformGroup)
        {
            if (!mouseOperationType.IsDrawOperationType())
            {
                throw new Exception("当前操作不是绘制鼠标类型");
            }

            DrawToolModel = drawToolModel;
        }

        /// <summary>
        /// 鼠标按下并移动 =  绘图
        /// </summary>
        /// <param name="point"></param>
        /// <param name="cursor"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void MouseLeftButtonDownAndMove(Point point, ref System.Windows.Input.Cursor cursor)
        {
            using (BitmapSourceDrawProvider provider = PreviewBitmap.Create(System.Drawing.Color.Transparent))
            {
                System.Drawing.Pen pen = new System.Drawing.Pen(DrawToolModel.PenColorType.GetBrush(), DrawToolModel.PenWidthType.GetWidth());
                Rect viewRect = new Rect(MouseDownPoint, point);
                switch (MouseOperationType)
                {
                    case MouseOperationType.DrawRectangle:
                        provider.Graphics.DrawRectangles(pen, new System.Drawing.RectangleF[] { viewRect.ToRectangleF() });
                        cursor = System.Windows.Input.Cursors.Cross;
                        break;
                    case MouseOperationType.DrawEllipse:
                        provider.Graphics.DrawEllipse(pen,viewRect.ToRectangleF());
                        break;
                    case MouseOperationType.DrawArrow:
                        provider.Graphics.DrawLine(pen,MouseDownPoint.ToDrawingPointF(),point.ToDrawingPointF());
                        break;
                    case MouseOperationType.DrawPen:
                        _points.Add(point);
                        for (int i = 0; i < _points.Count -1; i++)
                        {
                            provider.Graphics.DrawLine(pen, _points[i].ToDrawingPointF(), _points[i + 1].ToDrawingPointF());
                        }
                        cursor = System.Windows.Input.Cursors.Pen;
                        break;
                    case MouseOperationType.DrawText:
                        break;
                    case MouseOperationType.DrawSelect:
                        break;
                    default:
                        break;
                }
            }
        }

        public override void MouseLeftButtonUp(Point point)
        {
            Rect viewRect = new Rect(MouseDownPoint, point);
            Rect srcRect = viewRect;

            DrawElementBase drawElement = null;
            switch (MouseOperationType)
            {
                case MouseOperationType.DrawRectangle:
                    if (viewRect.Width > 0 && viewRect.Height > 0)
                    {
                        drawElement = new DrawRectangleElement(srcRect, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawEllipse:
                    if (viewRect.Width > 0 && viewRect.Height > 0)
                    {
                        drawElement = new DrawEllipseElement(srcRect, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawArrow:
                    if (viewRect.Width > 0 && viewRect.Height > 0)
                    {
                        drawElement = new DrawArrowElement(MouseDownPoint,point, DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawPen:
                    if (_points.Count > 2)
                    {
                        drawElement = new DrawPenElement(_points.DeepClone(), DrawToolModel.PenWidthType, DrawToolModel.PenColorType);
                    }
                    break;
                case MouseOperationType.DrawText:
                    break;
                case MouseOperationType.DrawSelect:
                    break;
                default:
                    break;
            }

            if(drawElement!= null)
            {
                DrawToolModel.DrawElements.Add(drawElement);
            }

            //每次预览图层画完后要清空，因为在实际画多个元素的图层里，会把所有元素重新绘一遍。
            PreviewBitmap.Clear(System.Drawing.Color.Transparent);
        }

        public override void MouseMove(Point point, ref System.Windows.Input.Cursor cursor)
        {
            if(MouseOperationType== MouseOperationType.DrawText)
            {
                return;
            }

            switch (MouseOperationType)
            {
                case MouseOperationType.DrawArrow:
                case MouseOperationType.DrawEllipse:
                case MouseOperationType.DrawRectangle:
                    cursor = System.Windows.Input.Cursors.Cross;
                    break;
                case MouseOperationType.DrawPen:
                    cursor = System.Windows.Input.Cursors.Pen;
                    break;
                default: break;
            }
        }
    }
}
