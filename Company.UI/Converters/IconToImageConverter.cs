using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Company.UI.Converters
{
    /// <summary>
    /// Icon名称=>Bitmap对象转换器
    /// </summary>
    public class IconToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (BitmapImage)System.Windows.Application.Current.Resources[(string)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
