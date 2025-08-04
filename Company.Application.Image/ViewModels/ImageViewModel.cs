using Company.Application.Share.Process;
using Company.Core.Models;
using Company.Hardware.Detector;
using ReactiveUI;

namespace Company.Application.Image.ViewModels
{
    public class ImageViewModel : ReactiveObject
    {
        public Gray16ImageSource Gray16ImageSource { get; set; } = new Gray16ImageSource(638, 844);

        public ImageViewModel(IDetectorProcessModel detectorProcessModel, IDetector detector)
        {
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
