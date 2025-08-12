using Company.Hardware.Camera;
using Company.Hardware.Detector;
using System.Windows.Input;

namespace Company.Application.Main.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private ICamera Camera { get; }
        private IDetector Detector { get; }

        public ICommand GrabPhotoCommand { get; }

        public MainViewModel(ICamera camera, IDetector detector)
        {
            Camera = camera;
            Detector = detector;
            GrabPhotoCommand = new DelegateCommand(GrabPhoto);
        }

        private void GrabPhoto()
        {
            Camera.Grab();
            Detector.Grab();
        }
    }
}
