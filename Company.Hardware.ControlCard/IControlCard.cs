namespace Company.Hardware.ControlCard
{
    public interface IControlCard
    {
        /// <summary>
        /// 控制卡是否初始化完成
        /// </summary>
        /// <returns></returns>
        bool Initialized { get; }

        /// <summary>
        /// 控制卡是否就绪
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// 运动轴是否回零
        /// </summary>
        bool IsAxisHomed { get; }

        /// <summary>
        /// 控制卡初始化
        /// </summary>
        /// <param name="controlCardConfig"></param>
        /// <returns></returns>
        (bool, string?) Init(ControlCardConfig controlCardConfig, bool isToGoHome = false);

        /// <summary>
        /// 控制卡关闭
        /// </summary>
        /// <returns></returns>
        void Close();

        /// <summary>
        /// 设置运动轴速度模式
        /// <paramref name="speedMode"/>
        /// </summary>
        void SetSpeedMode(SpeedMode speedMode);

        /// <summary>
        /// 控制卡运动轴回零
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        bool GoHome(out string? errMsg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="um"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        bool Move(AxisType axisType, double um, out string? errMsg);

        /// <summary>
        /// 连续运动
        /// </summary>
        /// <param name="axisType"></param>
        /// <param name="moveDirection"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        bool Move(AxisType axisType, MoveDirection moveDirection, out string? errMsg);

        /// <summary>
        /// 停止轴运动
        /// </summary>
        /// <param name="axisType"></param>
        void Stop(AxisType? axisType);
    }
}
