using Company.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Company.Application.Login.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                CN.Checked += OnChecked;
                TW.Checked += OnChecked;
                US.Checked += OnChecked;
            };
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton button)
            {
                switch (button.Name)
                {
                    case "CN":
                        PrismProvider.LanguageManager?.Set(Core.Enums.LanguageType.CN);
                        break;
                    case "TW":
                        PrismProvider.LanguageManager?.Set(Core.Enums.LanguageType.TW);
                        break;
                    case "US":
                        PrismProvider.LanguageManager?.Set(Core.Enums.LanguageType.US);
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported language: {button.Name}");
                }
            }
        }
    }
}
