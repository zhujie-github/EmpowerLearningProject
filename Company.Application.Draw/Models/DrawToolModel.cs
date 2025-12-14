using System.Collections.ObjectModel;
using Company.Application.Share.Draw;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Drawing;

namespace Company.Application.Draw.Models
{
    [ExposedService(Lifetime.Singleton, true, typeof(IDrawToolProvider))]
    public class DrawToolModel : ReactiveObject, IDrawToolProvider
    {
        /// <summary>
        /// 画笔宽度
        /// </summary>
        [Reactive]
        public PenWidthType PenWidthType { get; set; } = PenWidthType.Medium;

        /// <summary>
        /// 画笔颜色
        /// </summary>
        [Reactive]
        public PenColorType PenColorType { get; set; } = PenColorType.Red;

        /// <summary>
        /// 绘制的元素集合
        /// </summary>
        public ObservableCollection<DrawElementBase> DrawElements { get; set; } = [];

        public void Draw(Graphics graphics)
        {
            
        }
    }
}
