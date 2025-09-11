using Company.Application.Axis.Models;
using Company.Hardware.ControlCard;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;

namespace Company.Application.Axis.ViewModels
{
    public class AxisViewModel : ReactiveObject
    {
        private IControlCard ControlCard { get; }

        [Reactive]
        public bool IsMoving { get; set; }

        /// <summary>
        /// 开始持续移动某个轴
        /// </summary>
        public ICommand StartMoveCommand { get; private set; }
        /// <summary>
        /// 结束移动某个轴
        /// </summary>
        public ICommand EndMoveCommand { get; private set; }
        /// <summary>
        /// 停止所有运动轴
        /// </summary>
        public ICommand StopMoveCommand { get; private set; }

        public AxisViewModel(IControlCard controlCard)
        {
            ControlCard = controlCard;

            StartMoveCommand = ReactiveCommand.Create<AxisMoveParameter>(StartMove);
            EndMoveCommand = ReactiveCommand.Create<AxisMoveParameter>(EndMove);
            StopMoveCommand = ReactiveCommand.Create(StopAllMove);
        }

        private void StartMove(AxisMoveParameter axisMoveParameter)
        {
            IsMoving = ControlCard.Move(axisMoveParameter.AxisType, axisMoveParameter.Direction, out _);
        }

        private void EndMove(AxisMoveParameter axisMoveParameter)
        {
            if (!IsMoving)
                return;

            ControlCard.Stop(axisMoveParameter.AxisType);
            IsMoving = false;
        }

        /// <summary>
        /// 停止所有运动轴
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void StopAllMove()
        {
            ControlCard.Stop(null);
        }
    }
}
