using Company.Application.Process.Models;
using Company.Application.Share.Flow;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace Company.Application.Process.Services
{
    /// <summary>
    /// 16位FPD图像处理管理器
    /// </summary>
    [ExposedService]
    public class DetectorProcessManager
    {
        public IFlowProvider FlowProvider { get; }

        public DetectorProcessModel ProcessModel { get; }

        public DetectorProcessManager(IFlowProvider flowProvider, DetectorProcessModel processModel)
        {
            FlowProvider = flowProvider;
            ProcessModel = processModel;

            Subscribe();
        }

        private void Subscribe()
        {
            ProcessModel.SourceObservable.Subscribe(p => { Console.WriteLine("test..................................." + DateTime.Now); });

            var list = new List<IObservable<Unit>>
            {
                //当探测器的图像有更新时
                ProcessModel.SourceObservable.Sample(TimeSpan.FromMilliseconds(500)).Select(x => Unit.Default),

                //当图像算法集合被更新时
                FlowProvider.GetObservable()
            };
            list.CombineLatest(x => Unit.Default) //合并订阅
                .Throttle(TimeSpan.FromMilliseconds(500)) //节流，防止短时间内多次触发
                .ObserveOn(RxApp.TaskpoolScheduler) //切换到后台线程处理
                .Subscribe(_ => Process(ProcessModel.SourcePhoto)); //处理图像
        }

        private void Process(UnmanagedArray2D<ushort>? photo)
        {
            if (photo == null) return;

            using var temp = photo.DeepClone();
            if (FlowProvider.DoFilters(temp))
            {
                ProcessModel.Write(temp);
            }
            else
            {
                ProcessModel.Write(photo);
            }
        }
    }
}
