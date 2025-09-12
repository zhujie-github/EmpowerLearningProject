using System.Globalization;
using System.Windows.Data;

namespace Company.UI.Converters
{
    /// <summary>
    /// ture表示轴在运动中
    /// </summary>
    internal class BoolToMovingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(bool.TryParse(value?.ToString(), out bool result))
            {
                return result ? "正在移动" : "停止移动";
            }
            else
            {
                return "未知状态";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
