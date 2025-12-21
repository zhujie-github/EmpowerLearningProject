using Company.Application.Share.Draw;
using Company.Application.Share.Events;
using Company.Application.Share.Mouse;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.Draw.Models
{
    [ExposedService(types:typeof(IDrawToolProvider))]
    public class DrawToolModel : ReactiveObject, IDrawToolProvider
    {
        /// <summary>
        /// 画笔宽度
        /// </summary>
        [Reactive]
        public PenWidthType PenWidthType { get; set; } = PenWidthType.Small;

        // <summary>
        /// 画笔颜色
        /// </summary>
        [Reactive]
        public PenColorType PenColorType { get; set; } = PenColorType.Red;
        /// <summary>
        /// 要绘制的元素集合
        /// </summary>
        public ObservableCollection<DrawElementBase> DrawElements { get; set; }=new ObservableCollection<DrawElementBase>();

        [Reactive]
        public bool IsVisible { get; set; } = true;

        private IMouseOperationProvider MouseOperationProvider { get; }

        public DrawToolModel(IMouseOperationProvider mouseOperationProvider)
        {
            MouseOperationProvider = mouseOperationProvider;

            this.WhenAnyValue(p => p.DrawElements.Count, p => p.IsVisible).Skip(1).Subscribe(p => Publish());//触发绘制图形
        }

        private void Publish()
        {
            PrismProvider.EventAggregator.GetEvent<MouseDrawEvent>().Publish();
        }

        public void Draw(Graphics graphics)
        {
            foreach (var item in DrawElements)
            {
                item.Draw(graphics);
            }
        }

        public void DeleteAll()
        {
            DrawElements.Clear();
        }
    }
}
