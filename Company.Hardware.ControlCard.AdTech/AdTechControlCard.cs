namespace Company.Hardware.ControlCard.AdTech
{
    internal class AdTechControlCard : ControlCardBase
    {
        public override bool DoGetAxisEnabled(AxisType axisType)
        {
            throw new NotImplementedException();
        }

        public override bool DoGetAxisStopped(AxisType axisType)
        {
            throw new NotImplementedException();
        }

        public override bool DoGoHome(out string? errMsg)
        {
            throw new NotImplementedException();
        }

        protected override bool DoClose(out string? errMsg)
        {
            throw new NotImplementedException();
        }

        protected override bool DoConfigure(out string? errMsg)
        {
            throw new NotImplementedException();
        }

        protected override bool DoInit(out string? errMsg)
        {
            throw new NotImplementedException();
        }

        protected override bool DoMoveAxis(AxisType axisType, double um, out string? errMsg)
        {
            throw new NotImplementedException();
        }

        protected override bool DoMoveContinue(AxisType axisType, MoveDirection moveDirection, out string? errMsg)
        {
            throw new NotImplementedException();
        }

        protected override bool DoStop(AxisType? axisType, AxisStopMode axisStopMode, out string? errMsg)
        {
            throw new NotImplementedException();
        }
    }
}
