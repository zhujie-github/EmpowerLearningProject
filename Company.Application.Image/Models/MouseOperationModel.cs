using Company.Application.Share.Events;
using Company.Application.Share.Mouse;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using System.Windows.Input;

namespace Company.Application.Image.Models
{
    /// <summary>
    /// 各种鼠标操作模型，这里调用MouseOperationBase基类的成员
    /// </summary>
    [ExposedService]
    public class MouseOperationModel : ReactiveObject
    {
        /// <summary>
        /// 鼠标操作的预览图层
        /// </summary>
        [Reactive]
        public BitmapSourceGDI? PreviewBitmap { get; private set; }

        /// <summary>
        /// 当前鼠标操作的实例
        /// </summary>
        private MouseOperationBase? Current { get; set; }

        public bool IsEnabled => Current != null;

        public void Update(MouseOperationBase? mouse)
        {
            Current?.Before();
            Current = mouse;
            PrismProvider.EventAggregator.GetEvent<MouseOperationChangedEvent>().Publish(mouse);
        }

        public void InitBitmapSourceGdi(int width, int height)
        {
            PreviewBitmap = new BitmapSourceGDI(width, height);
        }

        public void MouseLeftButtonDown(Point point)
        {
            Current?.MouseLeftButtonDown(point);
        }

        public void MouseLeftButtonUp(Point point)
        {
            Current?.MouseLeftButtonUp(point);
        }

        public void MouseLeftButtonDownAndMove(Point point, ref Cursor cursor)
        {
            Current?.MouseLeftButtonDownAndMove(point, ref cursor);
        }

        public void MouseMove(Point point, ref Cursor cursor)
        {
            Current?.MouseMove(point, ref cursor);
        }
    }
}
