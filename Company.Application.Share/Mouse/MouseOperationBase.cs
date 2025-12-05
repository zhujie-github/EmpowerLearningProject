using Company.Application.Share.Draw;
using Company.Core.Enums;
using Company.Core.Models;
using ReactiveUI;

namespace Company.Application.Share.Mouse
{
    /// <summary>
    /// 鼠标操作基类
    /// </summary>
    public abstract class MouseOperationBase : ReactiveObject
    {
        /// <summary>
        /// 鼠标操作类型
        /// </summary>
        public MouseOperationType MouseOperationType { get; }
        /// <summary>
        /// 预览图
        /// </summary>
        protected BitmapSourceGDI PreviewBitmap { get; }
        /// <summary>
        /// 缩放平移对象
        /// </summary>
        public ITransformProvider TransformGroup { get; }

        public MouseOperationBase(MouseOperationType mouseOperationType, BitmapSourceGDI previewBitmap, ITransformProvider transformGroup)
        {
            MouseOperationType = mouseOperationType;
            PreviewBitmap = previewBitmap;
            TransformGroup = transformGroup;
        }
        /// <summary>
        /// 当前鼠标按下时的坐标
        /// </summary>
        protected System.Windows.Point MouseDownPoint { get;private set; }

        /// <summary>
        /// 鼠标操作结束之前
        /// </summary>
        public virtual void Before()
        {

        }

        /// <summary>
        /// 鼠标操作结束之后
        /// </summary>
        public virtual void After()
        {

        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="point"></param>
        public virtual void MouseLeftButtonDown(System.Windows.Point point) 
        {
            MouseDownPoint = point;
        }

        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        /// <param name="point"></param>
        public abstract void MouseLeftButtonUp(System.Windows.Point point);

        /// <summary>
        /// 鼠标左键按下并移动
        /// </summary>
        /// <param name="point"></param>
        /// <param name="cursor"></param>
        public abstract void MouseLeftButtonDownAndMove(System.Windows.Point point,ref System.Windows.Input.Cursor cursor);

        /// <summary>
        /// 鼠标移动时
        /// </summary>
        /// <param name="point"></param>
        /// <param name="cursor"></param>
        public abstract void MouseMove(System.Windows.Point point, ref System.Windows.Input.Cursor cursor);
    }
}
