using Company.Application.Share.Mouse;

namespace Company.Application.Share.Events
{
    /// <summary>
    /// 更新鼠标操作类型时
    /// </summary>
    public class MouseOperationChangedEvent : PubSubEvent<MouseOperationBase>
    {
    }
}
