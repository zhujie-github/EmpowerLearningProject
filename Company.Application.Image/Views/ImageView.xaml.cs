using Company.Algorithm.Unwrapper;
using System.Windows.Controls;

namespace Company.Application.Image.Views
{
    /// <summary>
    /// ImageView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();

            CppMethods.Hello();
        }
    }
}
