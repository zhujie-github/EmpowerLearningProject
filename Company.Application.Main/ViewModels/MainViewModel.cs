using Company.Application.Main.Models;
using Company.Core.Enums;
using Company.Hardware.Camera;
using Company.Hardware.Detector;
using System.Windows.Controls;
using System.Windows.Input;

namespace Company.Application.Main.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private ICamera Camera { get; }
        private IDetector Detector { get; }
        public MainModel MainModel { get; }
        public ICommand GrabPhotoCommand { get; }
        public ICommand MouseWorkModeCommand { get; }

        public MainViewModel(ICamera camera, IDetector detector, MainModel mainModel)
        {
            Camera = camera;
            Detector = detector;
            MainModel = mainModel;
            GrabPhotoCommand = new DelegateCommand(GrabPhoto);
            MouseWorkModeCommand = new DelegateCommand<CheckBox>(GetMouseWorkMode);
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
