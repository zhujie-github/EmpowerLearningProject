using Company.Application.Draw.Models;
using Company.Application.Share.Draw;
using Company.Application.Share.Mouse;
using Company.Core.Enums;
using Company.Core.Ioc;
using Company.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.Draw.Services
{
    /// <summary>
    /// 生成绘图元素的工厂
    /// </summary>
    [ExposedService(types:typeof(IMouseOperationDrawFactory))]
    public class MouseOperationDrawFactory : IMouseOperationDrawFactory
    {
        private DrawToolModel DrawToolModel { get; }

        public MouseOperationDrawFactory(DrawToolModel drawToolModel)
        {
            DrawToolModel = drawToolModel;
        }

        public MouseOperationBase CreateMouseOperation(MouseOperationType type, BitmapSourceGDI bitmap, ITransformProvider transformProvider)
        {
            return new DrawMouseOperation(DrawToolModel, type, bitmap, transformProvider);
        }
    }
}
