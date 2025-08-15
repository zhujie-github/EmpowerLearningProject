using Company.Application.Share.Main;
using Company.Core.Enums;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Company.Application.Main.Models
{
    /// <summary>
    /// 继承了一个带观察者的接口，其他模块可以从IoC中以接口的方式得到当前类型的实例
    /// </summary>
    [ExposedService(types:[typeof(IMouseWorkModeProvider)])]
    public class MainModel : ReactiveObject, IMouseWorkModeProvider
    {
        /// <summary>
        /// 鼠标工作模式
        /// </summary>
        [Reactive]
        public MouseWorkMode MouseWorkMode { get; set; } = MouseWorkMode.默认操作;

        /// <summary>
        /// 鼠标工作模式的观察者
        /// </summary>
        public IObservable<MouseWorkMode> MouseWorkModeObservable { get; }

        public MainModel()
        {
            MouseWorkModeObservable = this.WhenAnyValue(x => x.MouseWorkMode);
        }
    }
}
