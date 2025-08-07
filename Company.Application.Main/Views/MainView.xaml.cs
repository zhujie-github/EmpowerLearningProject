using Company.Core.Enums;
using Company.Core.Extensions;
using System.Windows.Controls;

namespace Company.Application.Main.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            comboBox.Binding<ZoomMode>();
        }
    }
}
