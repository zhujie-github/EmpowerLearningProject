using Company.Application.Share.Events;
using Company.Application.Share.Mouse;
using Company.Core.Helpers;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Core.Helpers;
using Company.Core.Models;

namespace Company.Application.Image.Models
{
    /// <summary>
    /// 各种鼠标操作业务，这里调用MouseOperationBase基类的成员
    /// </summary>
    [ExposedService(Lifetime.Singleton)]
    public class MouseOperationModel : ReactiveObject
    {
        /// <summary>
        /// 鼠标操作的预览图层
        /// </summary>
        [Reactive]
        public BitmapSourceGDI PreviewBitmap { get;private set; }
        /// <summary>
        /// 当前鼠标操作的实例
        /// </summary>
        private MouseOperationBase Current { get; set; }

        public bool IsEnable => Current != null;

        public void Update(MouseOperationBase mouse)
        {
            Current?.Before();
            Current = mouse;
            PrismProvider.EventAggregator.GetEvent<MouseOperationChangedEvent>().Publish(mouse);
        }

        /// <summary>
        /// 初始化鼠标绘图的画布
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InitBitmapSourceGDI(int width, int height)
        {
            PreviewBitmap = new BitmapSourceGDI(width, height);
        }

        public void MouseLeftButtonDown(System.Windows.Point point)
        {
            Assert.NotNull(Current);
            Current.MouseLeftButtonDown(point);
        }

        public void MouseLeftButtonUp(System.Windows.Point point)
        {
            Assert.NotNull(Current);
            Current.MouseLeftButtonUp(point);
        }

        public void MouseLeftButtonDownAndMove(System.Windows.Point point, ref System.Windows.Input.Cursor cursor)
        {
            Assert.NotNull(Current);
            Current.MouseLeftButtonDownAndMove(point, ref cursor);
        }

        public void MouseMove(System.Windows.Point point, ref System.Windows.Input.Cursor cursor)
        {
            Assert.NotNull(Current);
            Current.MouseMove(point, ref cursor);
        }
    } 
}
