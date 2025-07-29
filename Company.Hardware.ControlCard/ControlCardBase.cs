using Company.Logger;

namespace Company.Hardware.ControlCard
{
    /// <summary>
    /// 控制卡的抽象基类，定义了控制卡的基本操作接口，具体实现由子类完成。
    /// </summary>
    public abstract class ControlCardBase : IControlCard
    {
        public bool Initialized { get; private set; }

        public bool IsReady => Initialized && IsAxisHomed;

        /// <summary>
        /// 运动轴已回零
        /// </summary>
        public bool IsAxisHomed { get; set; }

        /// <summary>
        /// 运动轴正在回零
        /// </summary>
        public bool IsAxisHoming { get; set; }

        protected SpeedMode SpeedMode { get; private set; }

        /// <summary>
        /// 控制卡配置
        /// </summary>
        public ControlCardConfig? Config { get; private set; }

        /// <summary>
        /// AxisType枚举的元素集合
        /// </summary>
        protected IEnumerable<AxisType> AxisTypes { get; private set; } = Enum.GetValues(typeof(AxisType)).Cast<AxisType>();

        public void Close()
        {
            if (Initialized)
            {
                if (!DoStop(null, AxisStopMode.减速停止, out string? errMsg))
                {
                    NLogger.Error($"Control card stop failed: {errMsg}");
                    return;
                }

                if (!DoClose(out errMsg))
                {
                    NLogger.Error($"Control card close failed: {errMsg}");
                    return;
                }

                Initialized = false;
            }
        }

        public (bool, string?) Init(ControlCardConfig controlCardConfig)
        {
            Config = controlCardConfig;
            string? msg;

            if (Initialized)
            {
                msg = "Control card is already initialized.";
                return (false, msg);
            }

            try
            {
                if (!DoInit(out msg))
                {
                    return (false, msg);
                }

                if (!DoConfigure(out msg))
                {
                    return (false, msg);
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                return (false, msg);
            }

            Initialized = true;
            return (true, msg);
        }

        public bool Move(AxisType axisType, double um, out string? errMsg)
        {
            errMsg = null;
            if (!IsReady)
            {
                errMsg = "Control card is not ready. Please ensure it is initialized and the axis is homed.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移动轴
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="um"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool MoveAxis(AxisType axisType, double um, out string? errMsg)
        {
            //判断当前轴是否开启使能
            if (!DoGetAxisEnabled(axisType))
            {
                errMsg = $"Axis {axisType} is not enabled.";
                return false;
            }

            //判断当前轴是否正在运动
            if (!DoGetAxisStopped(axisType))
            {
                errMsg = $"Axis {axisType} is already moving.";
                return false;
            }

            if (!DoMoveAxis(axisType, um, out errMsg))
            {
                return false;
            }

            while (true)
            {
                if (DoGetAxisStopped(axisType))
                {
                    break; // 运动完成
                }
                Thread.Sleep(1000); // 等待一段时间后再次检查轴状态
            }

            return true;
        }

        /// <summary>
        /// 轴的连续运动
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="moveDirection"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Move(AxisType axisType, MoveDirection moveDirection, out string? errMsg)
        {
            if (!IsReady)
            {
                errMsg = "Control card is not ready. Please ensure it is initialized and the axis is homed.";
                return false;
            }

            //判断当前轴是否开启使能
            if (!DoGetAxisEnabled(axisType))
            {
                errMsg = $"Axis {axisType} is not enabled.";
                return false;
            }

            //判断当前轴是否正在运动
            if (!DoGetAxisStopped(axisType))
            {
                errMsg = $"Axis {axisType} is already moving.";
                return false;
            }

            return DoMoveContinue(axisType, moveDirection, out errMsg);
        }

        public void SetSpeedMode(SpeedMode speedMode)
        {
            SpeedMode = speedMode;
        }

        public void Stop(AxisType? axisType)
        {
            if (!DoStop(axisType, AxisStopMode.减速停止, out var errMsg))
            {
                NLogger.Error($"Control card stop failed: {errMsg}");
                return;
            }
        }

        public bool GoHome(out string? errMsg)
        {
            if (!Initialized)
            {
                errMsg = "Control card is not initialized. Please call Init() first.";
                return false;
            }

            if (!DoStop(null, AxisStopMode.减速停止, out errMsg))
            {
                return false;
            }
            Thread.Sleep(100);

            IsAxisHomed = false;
            IsAxisHoming = true;
            try
            {
                if (!DoGoHome(out errMsg))
                    return false;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            finally
            {
                IsAxisHoming = false;
            }

            IsAxisHomed = true;
            return true;
        }

        /// <summary>
        /// 初始化控制卡的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoInit(out string? errMsg);

        /// <summary>
        /// 配置控制卡的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoConfigure(out string? errMsg);

        /// <summary>
        /// 轴停止的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoStop(AxisType? axisType, AxisStopMode axisStopMode, out string? errMsg);

        /// <summary>
        /// 控制卡关闭的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoClose(out string? errMsg);

        /// <summary>
        /// 获取轴是否停止的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public abstract bool DoGetAxisStopped(AxisType axisType);

        /// <summary>
        /// 获取轴是否使能的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="axisType"></param>
        /// <returns></returns>
        public abstract bool DoGetAxisEnabled(AxisType axisType);

        /// <summary>
        /// 设置轴是否使能的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public abstract bool DoSetAxisEnabled(AxisType axisType, bool enabled = true);

        /// <summary>
        /// 移动轴的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="um"></param>
        protected abstract bool DoMoveAxis(AxisType axisType, double um, out string? errMsg);

        /// <summary>
        /// 轴的连续运动实现方法，由子类实现。
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="moveDirection"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected abstract bool DoMoveContinue(AxisType axisType, MoveDirection moveDirection, out string? errMsg);

        /// <summary>
        /// 轴回零的具体实现方法，由子类实现。
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public abstract bool DoGoHome(out string? errMsg);
    }
}
