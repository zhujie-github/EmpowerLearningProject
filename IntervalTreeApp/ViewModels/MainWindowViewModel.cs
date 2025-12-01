using Jamarino.IntervalTree;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace IntervalTreeApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ICommand Button1Command { get; private set; }

        public MainWindowViewModel()
        {
            Button1Command = new DelegateCommand(OnButton1Clicked);
        }

        private void OnButton1Clicked()
        {
            //throw new NotImplementedException();
            MessageBox.Show("todo");

            var tree = new LightIntervalTree<int, short>();

            // add intervals (from, to, value)
            tree.Add(10, 30, 1);
            tree.Add(20, 40, 2);
            tree.Add(25, 35, 3);

            // query
            var result = tree.Query(11); // result is {1}
            result = tree.Query(32); // result is {2, 3}
            result = tree.Query(27); // result is {1, 2, 3}
            result = tree.Query(5); // result is {}

            // query range
            result = tree.Query(5, 20); // result is {1, 2}
            result = tree.Query(26, 28); // result is {1, 2, 3}
            result = tree.Query(1, 50); // result is {1, 2, 3}

            // note: result order is not guaranteed
            var regexA = Regex.Replace("-10.12mm", @"[^\d.\d]", "");
            var regexB = Regex.Replace("-10.12mm", @"[^\d\.\-]", "");
            MessageBox.Show($"{regexA == regexB}");
        }
    }
}
