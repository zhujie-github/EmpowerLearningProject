using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Company.Core.Dialogs
{
    /// <summary>
    /// PopupBox.xaml 的交互逻辑
    /// </summary>
    public partial class PopupBox : Popup
    {
        public PopupBox()
        {
            InitializeComponent();
        }

        public new double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public static new readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(PopupBox), new PropertyMetadata(1.0, new PropertyChangedCallback(OnOpacityPropertyChanged)));

        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PopupBox popupBox) return;
            if (e.NewValue is not double newValue) return;
            popupBox.Border.Opacity = newValue;
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(PopupBox), new PropertyMetadata(""));

        private static readonly PopupBox popupBox = new();

        private static readonly DispatcherTimer timer = new();

        private const int DefaultDurationSeconds = 3;
        private int _durationSeconds = DefaultDurationSeconds;

        public static void Show(string message, Window? owner = null, int durationSeconds = DefaultDurationSeconds)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                owner ??= Application.Current.MainWindow;
                popupBox.Message = message;
                popupBox._durationSeconds = durationSeconds;
                popupBox.AllowsTransparency = true;
                popupBox.IsOpen = true;
                popupBox.Opacity = 1.0;
                popupBox.Placement = PlacementMode.Center;
                popupBox.PlacementTarget = owner;
                popupBox.StaysOpen = true;
                popupBox.VerticalOffset = owner.ActualHeight / 3.0;

                timer.Tick += Timer_Tick;
                timer.Start();
            });
        }

        private static void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();
            Task.Run(() =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // 逐渐减少透明度
                while (stopWatch.Elapsed.TotalSeconds < popupBox._durationSeconds)
                {
                    Thread.Sleep(5);
                    if (Application.Current == null) return;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        popupBox.Opacity -= 0.001;
                    });
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    popupBox.IsOpen = false;
                    popupBox.Message = "";
                });
            });
        }
    }
}
