using Company.Application.Share.Draw;
using Company.Core.Ioc;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.Image.Models
{
    /// <summary>
    /// 表示图像缩放移动的业务实体
    /// </summary>
    [ExposedService(types:typeof(ITransformProvider))]
    public class ImageTransformModel : ReactiveObject, ITransformProvider
    {
        public ImageTransformModel()
        {
            TransformObservable = this.WhenAnyValue(p => p.Transform);
        }
        public TransformModel Transform { get; set; }

        public IObservable<TransformModel> TransformObservable { get; }
    }
}
