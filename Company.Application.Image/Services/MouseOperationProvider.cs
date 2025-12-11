using Company.Application.Share.Mouse;
using Company.Core.Enums;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Company.Application.Image.Services
{
    /// <summary>
    /// 鼠标操作类型提供者
    /// </summary>
    [ExposedService(Lifetime.Singleton, true, typeof(IMouseOperationProvider))]
    public class MouseOperationProvider : ReactiveObject, IMouseOperationProvider
    {
        [Reactive] public MouseOperationType? MouseOperationType { get; set; }

        public IObservable<MouseOperationType?> Observable { get; }

        public void Cancel(MouseOperationType? mouseOperationType = null)
        {
            if (MouseOperationType != null && mouseOperationType.HasValue)
            {
                return;
            }

            MouseOperationType = null;
        }

        public MouseOperationProvider()
        {
            Observable = this.WhenAnyValue(p => p.MouseOperationType);
        }
    }
}