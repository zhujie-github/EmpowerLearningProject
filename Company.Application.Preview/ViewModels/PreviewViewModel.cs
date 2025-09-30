using Company.Application.Preview.Models;
using Company.Application.Share.Configs;
using Company.Core.Enums;
using Company.Core.Models;
using Company.Hardware.Camera;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.Application.Preview.ViewModels
{
    internal class PreviewViewModel : ReactiveObject
    {
        /// <summary>
        /// 绑定到前端显示相机的图像
        /// </summary>
        public CameraDisplayModel CameraDisplayModel { get;private set; }

        public BitmapSourceGDI BitmapSourceGDI { get; private set; }


        public Grid viewport { get; private set; }
        [Reactive]
        public int ViewportWidth { get; private set; }
        [Reactive]
        public int ViewportHeight { get; private set; }

        public Grid imagebox { get; private set; }

        [Reactive]
        public int ImageboxWidth { get; private set; }
        [Reactive]
        public int ImageboxHeight { get; private set; }

        [Reactive]
        public TransformGroup TransformGroup { get; private set; } = new TransformGroup();
        private ScaleTransform _scaleTransform = new ScaleTransform();//缩放
        private TranslateTransform _translateTransform = new TranslateTransform();//平移

        private MouseWorkMode mouseWorkMode = MouseWorkMode.图片查看;

        /// <summary>
        /// 十字架所引用的点位
        /// </summary>
        [Reactive]
        public Point MousePoint { get; private set; } = new Point(-1, -1);
        /// <summary>
        /// 鼠标按下位置 
        /// </summary>
        [Reactive]
        public Point MouseDownPoint { get; private set; } = new Point(0, 0);

        public ICommand MouseMoveCommand { get; }
        public ICommand ViewportLoadedCommand { get; private set; }
        public ICommand ViewportSizeChangedCommand { get; private set; }

        public ICommand MouseLeaveCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand MouseRightButtonDownCommand { get; }
        public ICommand MouseRightButtonUpCommand { get; }
        public ICommand MouseWheelCommand { get; }


        public PreviewViewModel(CameraDisplayModel cameraDisplayModel,ISystemConfigProvider systemConfigProvider,ICamera camera)
        {
            BitmapSourceGDI = new BitmapSourceGDI(systemConfigProvider.CameraConfig.Width, systemConfigProvider.CameraConfig.Height);
            CameraDisplayModel = cameraDisplayModel;
            TransformGroup.Children.Add(_scaleTransform);
            TransformGroup.Children.Add(_translateTransform);

            cameraDisplayModel.CameraObservable.ObserveOn(RxApp.MainThreadScheduler).Subscribe(PhotoChanged);

            ViewportLoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(ViewportLoaded);
            ViewportSizeChangedCommand = ReactiveCommand.Create<SizeChangedEventArgs>(ViewportSizeChanged);
            MouseMoveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseMove);
            MouseLeaveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeave);
            MouseLeftButtonDownCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseLeftButtonDown);
            MouseLeftButtonUpCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseLeftButtonUp);
            MouseRightButtonDownCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseRightButtonDown);
            MouseRightButtonUpCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseRightButtonUp);
            MouseWheelCommand = ReactiveCommand.Create<MouseWheelEventArgs>(MouseWheel);

        }

        private void PhotoChanged(UnmanagedArray2D<ColorBGRA>? obj)
        {
            if (obj == null) return;
            BitmapSourceGDI.WritePixels(obj, 0, 0);
        }


        private void ViewportLoaded(RoutedEventArgs e)
        {
            viewport = (Grid)e.Source;
            ViewportWidth = (int)viewport.ActualWidth;//1420
            ViewportHeight = (int)viewport.ActualHeight;//1002
            imagebox = viewport.Tag as Grid;
            ImageboxWidth = (int)imagebox.ActualWidth;
            ImageboxHeight = (int)imagebox.ActualHeight;
            SetTransform();
            ViewportLoadedCommand = null;//viewport的Loaded事件只执行一次
        }

        /// <summary>
        /// 根据viewport控件尺寸，设置图像缩放平移
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetTransform()
        {
            var scale = Math.Min(1.0 * ViewportWidth / ImageboxWidth, 1.0 * ViewportHeight / ImageboxHeight);
            _scaleTransform.ScaleX = scale;
            _scaleTransform.ScaleY = scale;

            var translateX = (ViewportWidth - ImageboxWidth * scale) / 2;
            var translateY = (ViewportHeight - ImageboxHeight * scale) / 2;
            _translateTransform.X = translateX;
            _translateTransform.Y = translateY;
        }


        private void ViewportSizeChanged(SizeChangedEventArgs e)
        {
            if (ViewportLoadedCommand != null)
                return;
            if (System.Windows.Application.Current?.MainWindow.WindowState == WindowState.Minimized)
                return;
        }

        private bool mousePressed;

        private void MouseMove(MouseEventArgs e)
        {
            Point point = e.GetPosition(viewport);
            MousePoint = point;//更新鼠标位置

            if (mousePressed)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    if (this.mouseWorkMode == MouseWorkMode.图片查看)
                    {
                        _translateTransform.X += point.X - MouseDownPoint.X;
                        _translateTransform.Y += point.Y - MouseDownPoint.Y;
                    }
                }

                MouseDownPoint = point;
            }
        }

        private void MouseLeave(MouseEventArgs e)
        {
            MousePoint = new Point(-1, -1);//更新鼠标位置           
        }

        private void MouseWheel(MouseWheelEventArgs e)
        {
            if (this.mouseWorkMode == MouseWorkMode.图片查看)
            {
                double scale = e.Delta * 0.0005;               

                Point point = e.GetPosition(viewport);
                Point inverse = TransformGroup.Inverse.Transform(point);

                _scaleTransform.ScaleX += scale;
                _scaleTransform.ScaleY += scale;

                _translateTransform.X = -1 * (inverse.X * _scaleTransform.ScaleX - point.X);
                _translateTransform.Y = -1 * (inverse.Y * _scaleTransform.ScaleY - point.Y);

            }
        }

        private void MouseLeftButtonDown(MouseButtonEventArgs e)
        {

        }

        private void MouseLeftButtonUp(MouseButtonEventArgs e)
        {

        }

        private void MouseRightButtonDown(MouseButtonEventArgs e)
        {
            mousePressed = true;
            MouseDownPoint = e.GetPosition(viewport);
            viewport.CaptureMouse();//立刻触发Move事件
        }

        private void MouseRightButtonUp(MouseButtonEventArgs e)
        {
            mousePressed = false;
            MouseDownPoint = e.GetPosition(viewport);
            viewport.ReleaseMouseCapture();//立刻释放鼠标
        }

    }
}
