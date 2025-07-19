using System.Windows;

namespace Company.UI.Controls
{
    /// <summary>
    /// WpfPropertyGrid.xaml 的交互逻辑
    /// </summary>
    public partial class WpfPropertyGrid : System.Windows.Controls.UserControl
    {
        public WpfPropertyGrid()
        {
            InitializeComponent();
        }

        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        public static readonly DependencyProperty SelectedObjectProperty =
            DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(WpfPropertyGrid), new FrameworkPropertyMetadata(SelectedObjectPropertyCallback));

        private static void SelectedObjectPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WpfPropertyGrid wpfPropertyGrid)
            {
                wpfPropertyGrid.propertyGrid.SelectedObject = e.NewValue;
            }
        }
    }
}
