using Company.Application.Share.Configs;
using Company.Application.Share.Process;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.Application.Image.ViewModels
{
    public class ImageViewModel : ReactiveObject
    {
        private ISystemConfigProvider SystemConfigProvider { get; }
        private Grid? Viewport { get; set; }
        private Grid? ImageBox { get; set; }
        private readonly ScaleTransform _scaleTransform = new();
        private readonly TranslateTransform _translateTransform = new();

        public Gray16ImageSource Gray16ImageSource { get; set; }
        [Reactive]
        public Point MousePoint { get; private set; } = new Point(-1, -1);
        [Reactive]
        public Visibility LineVisibility { get; set; } = Visibility.Visible;
        [Reactive]
        public TransformGroup TransformGroup { get; set; } = new TransformGroup();
        [Reactive]
        public int ViewportWidth { get; set; }
        [Reactive]
        public int ViewportHeight { get; set; }
        [Reactive]
        public int ImageBoxWidth { get; set; }
        [Reactive]
        public int ImageBoxHeight { get; set; }

        public ICommand? ViewportLoadedCommand { get; private set; }
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseLeaveCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand MouseRightButtonDownCommand { get; }
        public ICommand MouseRightButtonUpCommand { get; }
        public ICommand SizeChangedCommand { get; }

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

            TransformGroup.Children.Add(_scaleTransform);
            TransformGroup.Children.Add(_translateTransform);

            ViewportLoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(ViewportLoaded);
            MouseMoveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseMove);
            MouseLeaveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeave);
            MouseLeftButtonDownCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeftButtonDown);
            MouseLeftButtonUpCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeftButtonUp);
            MouseRightButtonDownCommand = ReactiveCommand.Create<MouseEventArgs>(MouseRightButtonDown);
            MouseRightButtonUpCommand = ReactiveCommand.Create<MouseEventArgs>(MouseRightButtonUp);
            SizeChangedCommand = ReactiveCommand.Create<SizeChangedEventArgs>(ViewportSizeChanged);
        }

        private void ViewportLoaded(RoutedEventArgs e)
        {
            Viewport = (Grid)e.Source;
            ViewportWidth = (int)Viewport.ActualWidth;
            ViewportHeight = (int)Viewport.ActualHeight;
            ImageBox = Viewport.Tag as Grid;
            ImageBoxWidth = (int)(ImageBox?.ActualWidth ?? 0);
            ImageBoxHeight = (int)(ImageBox?.ActualHeight ?? 0);
            SetTransform();
            ViewportLoadedCommand = null; //只执行一次
        }

        /// <summary>
        /// 根据Viewport尺寸，设置图像缩放平移
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetTransform()
        {
            var scale = Math.Min(1.0 * ViewportWidth / ImageBoxWidth, 1.0 * ViewportHeight / ImageBoxHeight);
            _scaleTransform.ScaleX = scale;
            _scaleTransform.ScaleY = scale;
            var translateX = (ViewportWidth - ImageBoxWidth * scale) / 2;
            var translateY = (ViewportHeight - ImageBoxHeight * scale) / 2;
            _translateTransform.X = translateX;
            _translateTransform.Y = translateY;
        }

        private void MouseMove(MouseEventArgs e)
        {
            MousePoint = e.GetPosition(Viewport);
        }

        private void MouseLeave(MouseEventArgs e)
        {
            MousePoint = new Point(-1, -1);
        }

        private void MouseLeftButtonDown(MouseEventArgs e)
        {
            
        }

        private void MouseLeftButtonUp(MouseEventArgs e)
        {
            
        }

        private void MouseRightButtonDown(MouseEventArgs e)
        {
            
        }

        private void MouseRightButtonUp(MouseEventArgs e)
        {

        }

        private void ViewportSizeChanged(SizeChangedEventArgs e)
        {
        }
    }
}
