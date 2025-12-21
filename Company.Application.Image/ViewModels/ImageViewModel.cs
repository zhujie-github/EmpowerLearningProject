using Company.Algorithm.Unwrapper;
using Company.Application.Image.Models;
using Company.Application.Image.Services;
using Company.Application.Share.Configs;
using Company.Application.Share.Main;
using Company.Application.Share.Mouse;
using Company.Application.Share.Process;
using Company.Core.Enums;
using Company.Core.Models;
using Company.Hardware.Detector;
//using Company.Hardware.Scanner;
//using Company.Hardware.Temp;
//using ControlzEx.Standard;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.Application.Image.ViewModels
{
    internal class ImageViewModel : ReactiveObject
    {
        [Reactive]
        public string ScannerContent { get; private set; } = string.Empty;
        [Reactive]
        public TransformGroup TransformGroup { get; private set; } = new TransformGroup();
        private ScaleTransform _scaleTransform = new ScaleTransform();//缩放
        private TranslateTransform _translateTransform = new TranslateTransform();//平移
        private MouseWorkMode mouseWorkMode = MouseWorkMode.默认操作;
        private IZoomModeProvider zoomModeProvider;

        public Grid viewport { get; private set; }
        [Reactive]
        public int ViewportWidth { get; private set; }
        [Reactive]
        public int ViewportHeight { get;private set; }

        public Grid imagebox { get; private set; }

        [Reactive]
        public int ImageboxWidth { get; private set; }
        [Reactive]
        public int ImageboxHeight { get; private set; }

        /// <summary>
        /// 十字架所引用的点位
        /// </summary>
        [Reactive]
        public Point MousePoint { get;private set; } = new Point(-1, -1);
        /// <summary>
        /// 鼠标按下位置 
        /// </summary>
        [Reactive]
        public Point MouseDownPoint { get; private set; }=new Point(0, 0);
        [Reactive]
        public Visibility LineVisibility { get; set; } = Visibility.Visible;
        public Gray16ImageSource Gray16Image { get; set; } 
        /// <summary>
        /// 鼠标操作的业务实体
        /// </summary>
        public MouseOperationModel MouseOperationModel { get; private set; }
        /// <summary>
        ///  鼠标操作类型的提供者
        /// </summary>
        private IMouseOperationProvider MouseOperationProvider { get; }
        /// <summary>
        /// 生成绘图元素的工厂
        /// </summary>
        private IMouseOperationDrawFactory MouseOperationDrawFactory { get; }
        /// <summary>
        /// 图像的缩放平移
        /// </summary>
        private ImageTransformModel ImageTransformModel { get; }

        public DrawElementModel DrawElementModel { get;private set; }
        /// <summary>
        /// 温度采集器
        /// </summary>
        //public ITempReader TempReader { get; }
        //public IScanner Scanner { get; }
        public ICommand MouseMoveCommand { get; }
        public ICommand ViewportLoadedCommand { get; private set; }
        public ICommand ViewportSizeChangedCommand { get; private set; }
        
        public ICommand MouseLeaveCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand MouseRightButtonDownCommand { get; }
        public ICommand MouseRightButtonUpCommand { get; }
        public ICommand MouseWheelCommand { get; }

        public ImageViewModel(
            ISystemConfigProvider systemConfigProvider,
            IDetectorProcessModel detectorProcessModel, 
            IDetector detector,
            //IScanner scanner,
            //ITempReader tempReader,
            MouseOperationModel mouseOperationModel,
            DetectorDisplayModel detectorDisplayModel,
            ImageTransformModel imageTransformModel,
            DrawElementModel drawElementModel,
            IMouseWorkModeProvider mouseWorkModeProvider, 
            IMouseOperationProvider mouseOperationProvider,
            IMouseOperationDrawFactory mouseOperationDrawFactory,
            IZoomModeProvider zoomModeProvider)
        {
            //TempReader = tempReader;
            //Scanner= scanner;
            this.zoomModeProvider = zoomModeProvider;
            zoomModeProvider.ZoomModeObservable.Subscribe(ZoomModeChanged);//实名函数
            //mouseWorkModeProvider.MouseWorkObservable.Subscribe(p => mouseWorkMode = p);//匿名函数
            mouseWorkModeProvider.MouseWorkModeObservable.Subscribe(MouseWorkModeChanged);//实名函数
            DrawElementModel= drawElementModel;
            ImageTransformModel = imageTransformModel;
            MouseOperationModel = mouseOperationModel;
            MouseOperationProvider = mouseOperationProvider;
            MouseOperationDrawFactory = mouseOperationDrawFactory;

            ViewportLoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(ViewportLoaded);
            ViewportSizeChangedCommand = ReactiveCommand.Create<SizeChangedEventArgs>(ViewportSizeChanged);
            MouseMoveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseMove);
            MouseLeaveCommand = ReactiveCommand.Create<MouseEventArgs>(MouseLeave);
            MouseLeftButtonDownCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseLeftButtonDown);
            MouseLeftButtonUpCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseLeftButtonUp);
            MouseRightButtonDownCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseRightButtonDown);
            MouseRightButtonUpCommand = ReactiveCommand.Create<MouseButtonEventArgs>(MouseRightButtonUp);
            MouseWheelCommand = ReactiveCommand.Create<MouseWheelEventArgs>(MouseWheel);

            TransformGroup.Children.Add(_scaleTransform);
            TransformGroup.Children.Add(_translateTransform);
            Gray16Image = new Gray16ImageSource(systemConfigProvider.DetectorConfig.Width, systemConfigProvider.DetectorConfig.Height);
            MouseOperationModel.InitBitmapSourceGDI(systemConfigProvider.DetectorConfig.Width, systemConfigProvider.DetectorConfig.Height);
            DrawElementModel.InitBitmapSourceGDI(systemConfigProvider.DetectorConfig.Width, systemConfigProvider.DetectorConfig.Height);
            detectorDisplayModel.Observable.Skip(1).ObserveOn(RxApp.MainThreadScheduler).Subscribe(TargetPhotoChanged);
            mouseOperationProvider.Observable.ObserveOn(RxApp.MainThreadScheduler).Subscribe(MouseOperationChanged);
            //detectorProcessModel.SourceObservable.Subscribe(image =>
            //{
            //    if (image == null) return;
            //    System.Windows.Application.Current?.Dispatcher.Invoke(() =>
            //    {
            //        using(UnmanagedArray2D<ushort> temp = image.DeepClone())
            //        {
            //            var cppImage16UC1 = new CppImage16UC1(temp);
            //            CppMethod.CppTest(cppImage16UC1, cppImage16UC1, 5000);
            //            Gray16Image.Write(temp);

            //        }

            //    });
            //});

            //观察二维码扫描仪结果变化
            //Scanner.ContentObservable.Subscribe(p => 
            //{
            //    ScannerContent = p;
            //});

        }

        private void TargetPhotoChanged(UnmanagedArray2D<ushort> temp)
        {
            Gray16Image.Write(temp);
        }

        private void MouseOperationChanged(MouseOperationType? type)
        {
            if (type.HasValue)
            {
                MouseOperationBase mouseOperationBase = MouseOperationDrawFactory.CreateMouseOperation(type.Value, MouseOperationModel.PreviewBitmap, ImageTransformModel);
                MouseOperationModel.Update(mouseOperationBase);

                LineVisibility = Visibility.Collapsed;
            }
            else
            {
                MouseOperationModel.Update(null);

                LineVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 图像缩放模式切换时
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ZoomModeChanged(ZoomMode obj)
        {
            if(mouseWorkMode == MouseWorkMode.图片查看)
            {
                var mode = zoomModeProvider.ZoomMode;
                double scale = GetScaleByZoomMode(mode);
                Point point = MouseDownPoint;
                
                _scaleTransform.ScaleX= scale; 
                _scaleTransform.ScaleY= scale;

                var translateX = (ViewportWidth - ImageboxWidth * scale) / 2;
                var translateY = (ViewportHeight - ImageboxHeight * scale) / 2;

                _translateTransform.X= translateX;
                _translateTransform.Y= translateY;
            }
        }

        private double GetScaleByZoomMode(ZoomMode mode)
        {
            double scale = 0;
            switch (mode)
            {
                case ZoomMode.Uniform:
                    scale = Math.Min(1.0 * ViewportWidth / ImageboxWidth, 1.0 * ViewportHeight / ImageboxHeight);
                    break;
                default:
                    scale = (int)mode;
                    break;
            }
            return scale;
        }

        private void MouseWorkModeChanged(MouseWorkMode p)
        {
            this.mouseWorkMode= p;
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

        private void ViewportSizeChanged(SizeChangedEventArgs e)
        {
            if (ViewportLoadedCommand != null)
                return;
            if (System.Windows.Application.Current?.MainWindow.WindowState == WindowState.Minimized)
                return;
        }

        /// <summary>
        /// 根据viewport控件尺寸，设置图像缩放平移
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SetTransform()
        {
            var scale = Math.Min(1.0 * ViewportWidth / ImageboxWidth, 1.0 * ViewportHeight / ImageboxHeight);
            _scaleTransform.ScaleX= scale;
            _scaleTransform.ScaleY= scale;

            var translateX = (ViewportWidth - ImageboxWidth * scale) / 2;
            var translateY = (ViewportHeight - ImageboxHeight * scale) / 2;
            _translateTransform.X = translateX;
            _translateTransform.Y = translateY;
        }


        #region Mouse操作


        private bool mousePressed;

        

        private void MouseLeave(MouseEventArgs e)
        {
            MousePoint = new Point(-1,-1);//更新鼠标位置           
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="e"></param>
        private void MouseLeftButtonDown(MouseButtonEventArgs e)
        {
            mousePressed = true;
            MouseDownPoint = e.GetPosition(viewport);

            if (MouseOperationModel.IsEnable)
            {
                MouseOperationModel.MouseLeftButtonDown(MouseDownPoint);
            }

            viewport.CaptureMouse();
        }

        private void MouseLeftButtonUp(MouseButtonEventArgs e)
        {
            mousePressed = false;
            MouseDownPoint = e.GetPosition(viewport);

            if (MouseOperationModel.IsEnable)
            {
                MouseOperationModel.MouseLeftButtonUp(MouseDownPoint);
            }

            viewport.ReleaseMouseCapture();
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

        private void MouseMove(MouseEventArgs e)
        {
            Point point = e.GetPosition(viewport);
            MousePoint = point;//更新鼠标位置
            System.Windows.Input.Cursor cursor = System.Windows.Input.Cursors.Arrow;
            if (mousePressed)
            {
                //左键接下才绘制
                if(e.LeftButton== MouseButtonState.Pressed)
                {
                    if (MouseOperationModel.IsEnable)
                    {
                        MouseOperationModel.MouseLeftButtonDownAndMove(point, ref cursor);
                    }
                }

                //右键接下就移动
                else if(e.RightButton == MouseButtonState.Pressed)
                {
                    if(this.mouseWorkMode == MouseWorkMode.图片查看)
                    {
                        _translateTransform.X += point.X - MouseDownPoint.X;
                        _translateTransform.Y += point.Y - MouseDownPoint.Y;
                    }
                }

                MouseDownPoint = point;
            }
        }

        private void MouseWheel(MouseWheelEventArgs e)
        {
            if(this.mouseWorkMode== MouseWorkMode.图片查看)
            {
                double scale = e.Delta * 0.0005;
                if (_scaleTransform.ScaleX + scale < 0.2)
                    return;

                Point point = e.GetPosition(viewport);
                Point inverse = TransformGroup.Inverse.Transform(point);

                _scaleTransform.ScaleX += scale;
                _scaleTransform.ScaleY += scale;

                _translateTransform.X = -1 * (inverse.X * _scaleTransform.ScaleX - point.X);
                _translateTransform.Y = -1 * (inverse.Y * _scaleTransform.ScaleY - point.Y);

            }
        }

        #endregion
    }
}
