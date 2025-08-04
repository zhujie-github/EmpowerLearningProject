using Company.Core.Helpers;
using Company.Core.Models;
using MvCamCtrl.NET;
using System.Runtime.InteropServices;

namespace Company.Hardware.Camera.HIK
{
    public class HikCamera : CameraBase
    {
        private MyCamera? Camera { get; set; }
        private MyCamera.cbOutputExdelegate? Callback { get; set; }
        private UnmanagedArray2D<ColorBGRA>? BufferBgra { get; set; }

        /// <summary>
        /// 初始化海康相机
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        protected override bool DoInit(out string? errMsg)
        {
            if (!FindDevice(out var device, out errMsg))
            {
                errMsg = $"查找海康相机失败：{errMsg}";
                return false;
            }

            Camera ??= new MyCamera();

            var result = Camera.MV_CC_CreateDevice_NET(ref device);
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"创建设备失败: {result}";
                return false;
            }

            result = Camera.MV_CC_OpenDevice_NET();
            if (result != MyCamera.MV_OK)
            {
                Camera.MV_CC_DestroyDevice_NET();
                errMsg = $"打开设备失败: {result}";
                return false;
            }

            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE) //GIGE相机
            {
                var packetSize = Camera.MV_CC_GetOptimalPacketSize_NET();
                if (packetSize > 0)
                {
                    result = Camera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)packetSize);
                    if (result != MyCamera.MV_OK)
                    {
                        errMsg = $"设置包大小失败: {result}";
                        return false;
                    }
                }
                else
                {
                    errMsg = $"获取最佳包大小失败: {packetSize}";
                    return false;
                }
            }

            result = Camera.MV_CC_SetEnumValue_NET("TriggerMode", 1); // 开启触发模式
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"开启触发模式失败: {result}";
                return false;
            }

            result = Camera.MV_CC_SetEnumValueByString_NET("TriggerSource", "Software"); // 设置触发源为软触发
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"设置软触发失败: {result}";
                return false;
            }

            Callback = new MyCamera.cbOutputExdelegate(OnCallback);
            result = Camera.MV_CC_RegisterImageCallBackEx_NET(Callback, IntPtr.Zero); // 注册回调函数
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"注册回调函数失败: {result}";
                return false;
            }

            result = Camera.MV_CC_StartGrabbing_NET();
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"开始抓取失败: {result}";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 相机回调函数
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnCallback(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, nint pUser)
        {
            BufferBgra ??= new UnmanagedArray2D<ColorBGRA>(pFrameInfo.nWidth, pFrameInfo.nHeight);
            MemoryHelper.CopyMemory(BufferBgra.Header, pData, BufferBgra.Length);
            InvokeOnGrabbed(new Photo(BufferBgra));
        }

        private static bool FindDevice(out MyCamera.MV_CC_DEVICE_INFO device, out string? errMsg)
        {
            device = default;
            errMsg = "";

            MyCamera.MV_CC_DEVICE_INFO_LIST list = default;
            var result = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref list);
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"Failed to enumerate devices";
                return false;
            }

            if (list.nDeviceNum <= 0)
            {
                errMsg = "No devices found";
                return false;
            }

            device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(list.pDeviceInfo[0], typeof(MyCamera.MV_CC_DEVICE_INFO_LIST));

            return true;
        }

        protected override bool DoClose(out string? errMsg)
        {
            errMsg = string.Empty;

            var result = Camera?.MV_CC_StopGrabbing_NET() ?? -1;
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"停止抓取失败: {result}";
                return false;
            }

            result = Camera?.MV_CC_CloseDevice_NET() ?? -1;
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"关闭设备失败: {result}";
                return false;
            }

            result = Camera?.MV_CC_DestroyDevice_NET() ?? -1;
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"销毁设备失败: {result}";
                return false;
            }

            return true;
        }

        public override bool DoCapture(out string? errMsg)
        {
            errMsg = string.Empty;
            var result = Camera?.MV_CC_SetCommandValue_NET("TriggerSoftware"); //软触发
            if (result != MyCamera.MV_OK)
            {
                errMsg = $"相机软触发失败:{result}";
                return false;
            }
            return true;
        }
    }
}
