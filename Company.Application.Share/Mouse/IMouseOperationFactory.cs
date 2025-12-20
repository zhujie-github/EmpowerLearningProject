using Company.Application.Share.Draw;
using Company.Core.Enums;
using Company.Core.Models;

namespace Company.Application.Share.Mouse
{
    /// <summary>
    /// 生成鼠标操作模型的工厂（基接口）
    /// </summary>
    public interface IMouseOperationFactory
    {
        /// <summary>
        /// 创建鼠标操作
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="bitmap">画布</param>
        /// <param name="transformProvider">缩放移动</param>
        /// <returns></returns>
        MouseOperationBase CreateMouseOperation(MouseOperationType type, BitmapSourceGDI bitmap, ITransformProvider transformProvider);
    }
}