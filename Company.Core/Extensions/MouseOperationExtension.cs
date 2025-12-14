using Company.Core.Enums;

namespace Company.Core.Extensions
{
    public static class MouseOperationExtension
    {
        extension(MouseOperationType type)
        {
            /// <summary>
            /// 判断是否为绘制操作类型
            /// </summary>
            /// <returns></returns>
            public bool IsDrawOperationType()
            {
                return type switch
                {
                    MouseOperationType.DrawRectangle or MouseOperationType.DrawEllipse or MouseOperationType.DrawArrow
                        or MouseOperationType.DrawPen or MouseOperationType.DrawText => true,
                    _ => false
                };
            }

            /// <summary>
            /// 判断是否为选择操作类型
            /// </summary>
            /// <returns></returns>
            public bool IsSelectOperationType()
            {
                return type switch
                {
                    MouseOperationType.DrawSelect => true,
                    _ => false
                };
            }
        }
    }
}