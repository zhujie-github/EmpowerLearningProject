using Company.Core.Events;
using Company.Core.Ioc;
using Company.Logger;

namespace Company.Hardware.ControlCard.None
{
    class NoneControlCard : ControlCardBase
    {
        public override bool DoGetAxisEnabled(AxisType axisType)
        {
            return true;
        }

        public override bool DoGetAxisStopped(AxisType axisType)
        {
            return true;
        }

        public override bool DoGoHome(out string? errMsg)
        {
            errMsg = null;
            return true;
        }

        protected override bool DoClose(out string? errMsg)
        {
            errMsg = null;
            return true;
        }

        protected override bool DoConfigure(out string? errMsg)
        {
            errMsg = null;
            return true;
        }

        protected override bool DoInit(out string? errMsg)
        {
            errMsg = null;
            PrismProvider.EventAggregator.GetEvent<ConfigChangedEvent>().Subscribe(OnConfigChanged);
            return true;
        }

        private void OnConfigChanged()
        {
            NLogger.Info("Control card configuration changed, but NoneControlCard does not support configuration changes.");
        }

        protected override bool DoMoveAxis(AxisType axisType, double um, out string? errMsg)
        {
            errMsg = null;
            Thread.Sleep(Convert.ToInt32(um)); // 模拟延时
            return true;
        }

        protected override bool DoMoveContinue(AxisType axisType, MoveDirection moveDirection, out string? errMsg)
        {
            errMsg = null;
            return true;
        }

        protected override bool DoStop(AxisType? axisType, AxisStopMode axisStopMode, out string? errMsg)
        {
            errMsg = null;

            if (axisType.HasValue)
            {
                StopAxis(axisType.Value);
            }
            else
            {
                foreach (var item in AxisTypes)
                {
                    StopAxis(item);
                }
            }

            return true;
        }

        private static void StopAxis(AxisType axisType)
        {
            Thread.Sleep(100); // 模拟延时
        }
    }
}
