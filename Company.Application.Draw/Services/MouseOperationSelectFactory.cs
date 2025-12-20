using Company.Application.Share.Draw;
using Company.Application.Share.Mouse;
using Company.Core.Enums;
using Company.Core.Models;

namespace Company.Application.Draw.Services
{
    /// <summary>
    /// 生成鼠标操作模型的工厂（选择类）
    /// </summary>
    internal class MouseOperationSelectFactory : IMouseOperationSelectFactory
    {
        public MouseOperationBase CreateMouseOperation(MouseOperationType type, BitmapSourceGDI bitmap, ITransformProvider transformProvider)
        {
            throw new NotImplementedException();
        }
    }
}
