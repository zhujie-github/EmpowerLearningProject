using Company.Application.Share.Configs;
using Company.Application.Share.Process;
using Company.Core.Models;
using Company.Hardware.Detector;
using ReactiveUI;

namespace Company.Application.Image.ViewModels
{
    public class ImageViewModel : ReactiveObject
    {
        private ISystemConfigProvider SystemConfigProvider { get; }

        public Gray16ImageSource Gray16ImageSource { get; set; }

        public ImageViewModel(ISystemConfigProvider systemConfigProvider, IDetectorProcessModel detectorProcessModel,
            IDetector detector)
        {
            SystemConfigProvider = systemConfigProvider;
            Gray16ImageSource = new Gray16ImageSource(SystemConfigProvider.DetectorConfig.Width, SystemConfigProvider.DetectorConfig.Height);
            detectorProcessModel.SourceObservable.Subscribe(source =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Gray16ImageSource.Write(source);
                });
            });
            detector.Grab();
        }
    }
}
