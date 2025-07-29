using Company.Hardware.ControlCard.AdTech.Api;

namespace Company.Hardware.ControlCard.AdTech
{
    public class AdTechControlCard : ControlCardBase
    {
        private static int CardNo { get; set; }

        private Bit 急停 { get; set; } = new Bit(2, 1);

        public override bool DoGetAxisEnabled(AxisType axisType)
        {
            var axis = (int)axisType;
            var result = adt_card_632xe.adt_get_axis_enable(CardNo, axis, out var enabled);
            return result == 0 && enabled == 1;
        }

        public override bool DoSetAxisEnabled(AxisType axisType, bool enabled = true)
        {
            var axis = (int)axisType;
            var result = adt_card_632xe.adt_set_axis_enable(CardNo, axis, Convert.ToInt32(enabled));
            return result == 0;
        }

        public override bool DoGetAxisStopped(AxisType axisType)
        {
            var axis = (int)axisType;
            var result = adt_card_632xe.adt_get_stopdata(CardNo, axis, out var stopped);
            return result == 0 && stopped == 0;
        }

        public override bool DoGoHome(out string? errMsg)
        {
            errMsg = null;

            foreach (var axisType in AxisTypes)
            {
                var axis = (int)axisType;
                var result = adt_card_632xe.adt_set_servo_home(CardNo, axis, 1);
                if (result != 0)
                {
                    errMsg = adt_global.adt_decode_error_code(result);
                    return false;
                }
            }

            return true;
        }

        protected override bool DoClose(out string? errMsg)
        {
            errMsg = null;

            var result = adt_card_632xe.adt_ecat_stop(CardNo);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            result = adt_card_632xe.adt_close_card();
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            return true;
        }

        protected override bool DoConfigure(out string? errMsg)
        {
            errMsg = null;

            var result = 0;
            var board = 0;
            var logic = 0;

            result += adt_card_632xe.adt_set_axis_io_map(CardNo, 0, 4, 急停.Board, 急停.Port);
            result += adt_card_632xe.adt_set_hardlimit_mode(CardNo, 0, 4, 1, 1, 1);

            foreach (var axisType in AxisTypes)
            {
                var axis = (int)axisType;

                //正限位
                result += adt_card_632xe.adt_set_axis_io_map(CardNo, axis, 0, board, 0);
                result += adt_card_632xe.adt_set_hardlimit_mode(CardNo, axis, 0, 1, logic, 1);

                //负限位
                result += adt_card_632xe.adt_set_axis_io_map(CardNo, axis, 1, board, 1);
                result += adt_card_632xe.adt_set_hardlimit_mode(CardNo, axis, 1, 1, logic, 1);

                //原点信号
                result += adt_card_632xe.adt_set_axis_io_map(CardNo, axis, 2, board, 2);
                result += adt_card_632xe.adt_set_hardlimit_mode(CardNo, axis, 2, 1, logic, 0);

                //脉冲当量&编程模式
                result += adt_card_632xe.adt_set_unit_mode(CardNo, axis, 0); //编程模式
                result += adt_card_632xe.adt_set_pulse_equiv(CardNo, axis, 1000); //脉冲当量
            }

            //创建插补坐标系
            result += adt_card_632xe.adt_set_inp_coordinate(CardNo, 63, 3, [3, 4]);

            //编程模式
            result += adt_card_632xe.adt_set_unit_mode(CardNo, 63, 0);

            //启用软限位
            foreach (var axisType in AxisTypes)
            {
                var axis = (int)axisType;
                var ppos = .0;
                var npos = .0;
                result += adt_card_632xe.adt_set_softlimit_mode(CardNo, axis, 1, ppos, npos, (int)AxisStopMode.减速停止);
            }

            if (result != 0)
            {
                errMsg = $"{nameof(DoConfigure)}失败，错误码聚合值：{result}";
                return false;
            }

            return true;
        }

        protected override bool DoInit(out string? errMsg)
        {
            try
            {
                var result = adt_card_632xe.adt_initial(out int cardCount, 0);
                if (result != 0)
                {
                    errMsg = adt_global.adt_decode_error_code(result);
                    return false;
                }
                if (cardCount <= 0)
                {
                    errMsg = "未检测到运动控制卡，请检查卡是否安装正确或驱动是否安装成功。";
                    return false;
                }

                int[] indexes = new int[cardCount];
                result = adt_card_632xe.adt_get_card_index(out cardCount, indexes);
                if (result != 0)
                {
                    errMsg = adt_global.adt_decode_error_code(result);
                    return false;
                }
                CardNo = indexes[0];

                if (!Start(out errMsg))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 启动运动控制卡
        /// </summary>
        /// <returns></returns>
        private static bool Start(out string? errMsg)
        {
            errMsg = null;

            var result = adt_card_632xe.adt_ecat_stop(CardNo);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            result = adt_card_632xe.adt_ecat_load_flash_cfg(CardNo);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            result = adt_card_632xe.adt_ecat_start(CardNo);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            return true;
        }

        protected override bool DoMoveAxis(AxisType axisType, double um, out string? errMsg)
        {
            var axis = (int)axisType;
            errMsg = null;

            var result = adt_card_632xe.adt_pmove_unit(CardNo, axis, um, 0);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            return true;
        }

        protected override bool DoMoveContinue(AxisType axisType, MoveDirection moveDirection, out string? errMsg)
        {
            var axis = (int)axisType;
            errMsg = null;

            var result = adt_card_632xe.adt_continue_move(CardNo, axis, (int)moveDirection);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            return true;
        }

        protected override bool DoStop(AxisType? axisType, AxisStopMode axisStopMode, out string? errMsg)
        {
            errMsg = null;

            var axis = axisType.HasValue ? (int)axisType.Value : 0;
            var result = adt_card_632xe.adt_set_axis_stop(CardNo, axis, (int)axisStopMode);
            if (result != 0)
            {
                errMsg = adt_global.adt_decode_error_code(result);
                return false;
            }

            return true;
        }
    }
}
