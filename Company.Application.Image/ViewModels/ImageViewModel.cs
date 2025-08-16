using Company.Application.Share.Configs;
using Company.Application.Share.Main;
using Company.Application.Share.Process;
using Company.Core.Enums;
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
        private ScaleTransform ScaleTransform { get; } = new();
        private TranslateTransform TranslateTransform { get; } = new();
        private MouseWorkMode MouseWorkMode { get; set; }
        private ZoomMode ZoomMode { get; set; }
        private bool MousePressed { get; set; }

        public Gray16ImageSource Gray16ImageSource { get; set; }
        /// <summary>
        /// 十字架所引用的点位
        /// </summary>
        [Reactive]
        public Point MousePoint { get; private set; } = new Point(-1, -1);
        /// <summary>
        /// 鼠标按下的位置
        /// </summary>
        [Reactive]
        public Point MouseDownPoint { get; private set; } = new Point(-1, -1);
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
        public ICommand MouseWheelCommand { get; }

        public ImageViewModel(ISystemConfigProvider systemConfigProvider, IDetectorProcessModel detectorProcessModel,
            IMouseWorkModeProvider mouseWorkModeProvider, IZoomModeProvider zoomModeProvider)
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

            mouseWorkModeProvider.MouseWorkModeObservable.Subscribe(source =>
            {
                MouseWorkMode = source;
            });

            zoomModeProvider.ZoomModeObservable.Subscribe(source =>
            {
                ZoomModeChanged(source);
            });

            TransformGroup.Children.Add(ScaleTransform);
            TransformGroup.Children.Add(TranslateTransform);

            ViewportLoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(ViewportLoaded);
            MouseMoveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseMove);
            MouseLeaveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeave);
            MouseLeftButtonDownCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeftButtonDown);
            MouseLeftButtonUpCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeftButtonUp);
            MouseRightButtonDownCommand = ReactiveCommand.Create<MouseEventArgs>(MouseRightButtonDown);
            MouseRightButtonUpCommand = ReactiveCommand.Create<MouseEventArgs>(MouseRightButtonUp);
            SizeChangedCommand = ReactiveCommand.Create<SizeChangedEventArgs>(ViewportSizeChanged);
            MouseWheelCommand = ReactiveCommand.Create<MouseWheelEventArgs>(MouseWheel);
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

        private double GetScaleByZoomMode(ZoomMode mode)
        {
            return mode switch
            {
                ZoomMode.Uniform => Math.Min(1.0 * ViewportWidth / ImageBoxWidth, 1.0 * ViewportHeight / ImageBoxHeight),
                _ => (int)mode,
            };
        }

        /// <summary>
        /// 根据Viewport尺寸，设置图像缩放平移
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetTransform()
        {
            var scale = GetScaleByZoomMode(ZoomMode);
            ScaleTransform.ScaleX = scale;
            ScaleTransform.ScaleY = scale;
            TranslateTransform.X = (ViewportWidth - ImageBoxWidth * scale) / 2;
            TranslateTransform.Y = (ViewportHeight - ImageBoxHeight * scale) / 2;
        }

        private void MouseMove(MouseEventArgs e)
        {
            MousePoint = e.GetPosition(Viewport);
            if (MousePressed)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    if (MouseWorkMode == MouseWorkMode.图片查看)
                    {
                        TranslateTransform.X += MousePoint.X - MouseDownPoint.X;
                        TranslateTransform.Y += MousePoint.Y - MouseDownPoint.Y;
                    }
                }

                MouseDownPoint = MousePoint;
            }
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
            MousePressed = true;
            MouseDownPoint = e.GetPosition(Viewport);
            Viewport?.CaptureMouse(); //立刻触发Move事件
        }

        private void MouseRightButtonUp(MouseEventArgs e)
        {
            MousePressed = false;
            MouseDownPoint = e.GetPosition(Viewport);
            Viewport?.ReleaseMouseCapture();
        }

        private void ViewportSizeChanged(SizeChangedEventArgs e)
        {
        }

        private void MouseWheel(MouseWheelEventArgs e)
        {
            if (MouseWorkMode != MouseWorkMode.图片查看)
                return;

            var scale = e.Delta * 0.0005;
            if (ScaleTransform.ScaleX + scale < 0.2)
                return;

            var point = e.GetPosition(Viewport);
            var inversed = TransformGroup.Inverse.Transform(point);

            ScaleTransform.ScaleX += scale;
            ScaleTransform.ScaleY += scale;

            TranslateTransform.X = -(inversed.X * ScaleTransform.ScaleX - point.X);
            TranslateTransform.Y = -(inversed.Y * ScaleTransform.ScaleY - point.Y);
        }

        /// <summary>
        /// 图像缩放模式发生改变时触发该方法
        /// </summary>
        /// <param name="mode"></param>
        private void ZoomModeChanged(ZoomMode mode)
        {
            ZoomMode = mode;

            if (MouseWorkMode != MouseWorkMode.图片查看)
                return;

            SetTransform();
        }
    }
}
