using Company.Application.Share.Configs;
using Company.Application.Share.Process;
using Company.Core.Models;
using Company.Hardware.Detector;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Company.Application.Image.ViewModels
{
    public class ImageViewModel : ReactiveObject
    {
        private ISystemConfigProvider SystemConfigProvider { get; }
        private Grid? Viewport { get; set; }

        public Gray16ImageSource Gray16ImageSource { get; set; }
        [Reactive]
        public Point MousePoint { get; private set; } = new Point(-1, -1);
        [Reactive]
        public Visibility LineVisibility { get; set; } = Visibility.Visible;

        public ICommand? ViewportLoadedCommand { get; private set; }

        public ICommand MouseMoveCommand { get; }

        public ImageViewModel(ISystemConfigProvider systemConfigProvider, IDetectorProcessModel detectorProcessModel)
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
            ViewportLoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(ViewportLoaded);
            MouseMoveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseMove);
        }

        private void ViewportLoaded(RoutedEventArgs e)
        {
            Viewport = (Grid)e.Source;
            ViewportLoadedCommand = null; //只执行一次
        }

        private void MouseMove(MouseEventArgs e)
        {
            MousePoint = e.GetPosition(Viewport);
        }
    }
}
