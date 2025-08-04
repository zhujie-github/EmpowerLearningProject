using Company.Core.Helpers;
using Company.Core.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace Company.Hardware.Camera.None
{
    public class NoneCamera : CameraBase
    {
        private UnmanagedArray2D<ColorBGRA>? _unmanagedArray;

        protected override bool DoInit(out string? errMsg)
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

            _unmanagedArray = new UnmanagedArray2D<ColorBGRA>(Config!.Width, Config.Height);
            var bitmap = ImageHelper.Load(filePath);
            if (bitmap == null)
            {
                errMsg = $"Failed to load image from file: {filePath}";
                return false;
            }
            if (_unmanagedArray.Width != bitmap.Width || _unmanagedArray.Height != bitmap.Height)
            {
                errMsg = $"系统设置中的相机尺寸({_unmanagedArray.Width}*{_unmanagedArray.Height})与实际相机分辨率({bitmap.Width}*{bitmap.Height})不一致";
                return false;
            }

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            MemoryHelper.CopyMemory(_unmanagedArray.Header, data.Scan0, _unmanagedArray.Length);
            bitmap.UnlockBits(data);

            Thread.Sleep(1000);
            return true;
        }

        protected override bool DoClose(out string? errMsg)
        {
            errMsg = "";
            return true;
        }

        public override bool DoGrab(out string? errMsg)
        {
            errMsg = "";
            Task.Delay(50).ContinueWith(t =>
            {
                if (_unmanagedArray != null)
                {
                    InvokeOnGrabbed(new Photo(_unmanagedArray));
                }
            });
            return true;
        }
    }
}
