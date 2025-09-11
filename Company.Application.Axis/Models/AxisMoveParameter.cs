using Company.Hardware.ControlCard;

namespace Company.Application.Axis.Models
{
    /// <summary>
    /// 运动轴控制参数
    /// </summary>
    public struct AxisMoveParameter
    {
        public AxisType AxisType { get; set; }
        public MoveDirection Direction { get; set; }
    }
}
