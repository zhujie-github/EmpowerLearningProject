
namespace Company.Hardware.Camera.None
{
    public class NoneCamera : CameraBase
    {
        public override bool Trigger()
        {
            return true;
        }

        protected override void DoClose()
        {

        }

        protected override bool DoInit()
        {
            return true;
        }
    }
}
