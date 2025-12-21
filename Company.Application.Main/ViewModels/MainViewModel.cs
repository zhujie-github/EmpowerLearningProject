using Company.Application.Main.Models;
using Company.Application.Share.Draw;
using Company.Application.Share.Image;
using Company.Core.Dialogs;
using Company.Core.Enums;
using Company.Core.Helpers;
using Company.Hardware.Camera;
using Company.Hardware.Detector;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using Company.Core.Extensions;

namespace Company.Application.Main.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private ICamera Camera { get; }
        private IDetector Detector { get; }
        public MainModel MainModel { get; }
        public IDetectorDisplayModel DetectorDisplayModel { get; private set; }
        public IDrawToolProvider DrawToolProvider { get; private set; }
        public ICommand LoadedCommand { get; }
        public ICommand GrabPhotoCommand { get; }
        public ICommand MouseWorkModeCommand { get; }
        public ICommand SaveGray16Command { get; }
        public ICommand SaveBmpCommand { get; }

        public MainViewModel(
            IDetector detector,
            ICamera camera, MainModel mainModel,
            IDetectorDisplayModel detectorDisplayModel,
            IDrawToolProvider drawToolProvider)
        {
            Camera = camera;
            Detector = detector;
            MainModel = mainModel;
            DetectorDisplayModel = detectorDisplayModel;
            DrawToolProvider = drawToolProvider;
            LoadedCommand = new DelegateCommand(Loaded);
            GrabPhotoCommand = new DelegateCommand(GrabPhoto);
            MouseWorkModeCommand = new DelegateCommand<CheckBox>(GetMouseWorkMode);
            SaveGray16Command = new DelegateCommand(SaveGray16);
            SaveBmpCommand = new DelegateCommand(SaveBmp);
        }

        /// <summary>
        /// 保存PNG图像
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveBmp()
        {
            if (DetectorDisplayModel.Photo != null)
            {
                if (FileHelper.SaveFileDialog("保存图像", "bmp Files|*.bmp", ".bmp", null, out string fullname))
                {
                    Bitmap bitmap = new Bitmap(DetectorDisplayModel.Photo.Width, DetectorDisplayModel.Photo.Height);
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.DrawImageUnscaled(DetectorDisplayModel.Photo.ToGray8Bitmap(), 0, 0);
                        DrawToolProvider.Draw(g);
                    }
                    bitmap.Save(fullname + ".bmp");
                    bitmap.Dispose();
                    PopupBox.Show("图像保存成功");
                }
            }
        }

        /// <summary>
        /// 保存16进制灰度图像
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SaveGray16()
        {
            if (DetectorDisplayModel.Photo != null)
            {
                if (FileHelper.SaveFileDialog("保存图像", "Tif Files|*.tif", ".tif", null, out string fullname))
                {
                    DetectorDisplayModel.Photo.Save(fullname);
                    PopupBox.Show("图像保存成功");
                }
            }
        }

        private async void Loaded()
        {
            await Task.Delay(100).ContinueWith(task =>
            {
                GrabPhoto();
            });
        }

        private void GrabPhoto()
        {
            Camera.Grab();
            Detector.Grab();
        }

        private void GetMouseWorkMode(CheckBox? checkBox)
        {
            if (checkBox == null || !checkBox.IsChecked.HasValue) return;

            MainModel.MouseWorkMode = checkBox.IsChecked.Value ? MouseWorkMode.图片查看 : MouseWorkMode.默认操作;
        }
    }
}
