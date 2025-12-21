using Company.Application.Share.Draw;
using Company.Application.Share.Events;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Core.Extensions;

namespace Company.Application.Image.Models
{
    /// <summary>
    /// 这里绘制鼠标画下的所有元素
    /// </summary>
    [ExposedService(Lifetime.Singleton)]
    public class DrawElementModel : ReactiveObject
    {
        private readonly List<IDrawToolProvider> drawToolProviders = new List<IDrawToolProvider>();
        public DrawElementModel(IDrawToolProvider drawToolProvider) 
        {
            drawToolProviders.Add(drawToolProvider);

            PrismProvider.EventAggregator.GetEvent<MouseDrawEvent>().Subscribe(DoDraw, Prism.Events.ThreadOption.UIThread);
        }

        public BitmapSourceGDI ElementBitmap { get;private set; }

        /// <summary>
        /// 初始化画布
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InitBitmapSourceGDI(int width, int height) 
        {
            ElementBitmap = new BitmapSourceGDI(width, height);
        }

        private void DoDraw()
        {
            if (ElementBitmap != null)
            {
                using(BitmapSourceDrawProvider provider = ElementBitmap.Create(System.Drawing.Color.Transparent))
                {
                    foreach (var item in drawToolProviders)
                    {
                        item.Draw(provider.Graphics);
                    }
                }
            }
        }
    }
}
