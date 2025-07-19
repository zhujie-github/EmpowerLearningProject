using Company.Core.Helpers;
using Company.Core.Models;
using System.IO;

namespace Company.Hardware.Detector.None
{
    public class NoneDetector : DetectorBase
    {
        private UnmanagedArray2D<ushort>? _unmanagedArray;
        private Task? _task;

        protected override bool DoInit(out string errMsg)
        {
            errMsg = "";
            if (Config == null)
            {
                errMsg = $"{nameof(Config)} cannot be null.";
                return false;
            }

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", Config!.Photo);
            if (!File.Exists(filePath))
            {
                errMsg = $"Image file not found: {filePath}";
                return false;
            }

            _unmanagedArray = ImageHelper.LoadTiff(filePath);
            _unmanagedArray ??= new UnmanagedArray2D<ushort>(Config.Width, Config.Height);
            if (_unmanagedArray.Width != Config.Width || _unmanagedArray.Height != Config.Height)
            {
                errMsg = $"系统设置中的FPD尺寸({Config.Width}*{Config.Height})与实际的FPD分辨率({_unmanagedArray.Width}*{_unmanagedArray.Height})不一致";
                return false;
            }
            Thread.Sleep(1000); // 模拟延时，实际应用中可能需要更复杂的逻辑
            return true;
        }

        protected override bool DoClose(out string errMsg)
        {
            errMsg = "";
            return true;
        }

        public override bool DoCapture(out string errMsg)
        {
            errMsg = "";

            if (_task == null || _task.IsCompleted)
            {
                IsCapturing = true;
                _task = Task.Factory.StartNew(RunCapture, TaskCreationOptions.LongRunning);
            }

            return true;
        }

        private void RunCapture()
        {
            if (_unmanagedArray == null)
                return;

            while (IsCapturing)
            {
                //定时输出图像
                using (var temp = _unmanagedArray.DeepClone())
                {
                    OnImageCaptured(temp);
                }

                Thread.Sleep(50); // 模拟延时，实际应用中可能需要更复杂的逻辑
            }
        }
    }
}
