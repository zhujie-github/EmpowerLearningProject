using Company.Application.Draw.Models;
using Company.Application.Share.Draw;
using Company.Application.Share.Mouse;
using Company.Core.Enums;
using Company.Core.Ioc;
using Company.Core.Models;

namespace Company.Application.Draw.Services
{
    /// <summary>
    /// 生成鼠标操作模型的工厂（绘制类）
    /// </summary>
    [ExposedService(Lifetime.Singleton, true, typeof(IMouseOperationProvider))]
    public class MouseOperationDrawFactory(DrawToolModel drawToolModel) : IMouseOperationDrawFactory
    {
        private DrawToolModel DrawToolModel { get; } = drawToolModel;

        public MouseOperationBase CreateMouseOperation(MouseOperationType type, BitmapSourceGDI bitmap, ITransformProvider transformProvider)
        {
            return new DrawMouseOperation(DrawToolModel, type, bitmap, transformProvider);
        }
    }
}
