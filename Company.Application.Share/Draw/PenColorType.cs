using System.Drawing;

namespace Company.Application.Share.Draw
{
    /// <summary>
    /// 画笔颜色
    /// </summary>
    public enum PenColorType
    {
        Red,
        Orange,
        Yellow,
        Green,
        Cyan,
        Blue,
        Purple,
        White,
        Black,
    }

    public static class PenExtension
    {
        public static Brush GetBrush(this PenColorType colorType)
        {
            return colorType switch
            {
                PenColorType.Red => Brushes.Red,
                PenColorType.Orange => Brushes.Orange,
                PenColorType.Yellow => Brushes.Yellow,
                PenColorType.Green => Brushes.Green,
                PenColorType.Cyan => Brushes.Cyan,
                PenColorType.Blue => Brushes.Blue,
                PenColorType.Purple => Brushes.Purple,
                PenColorType.White => Brushes.White,
                _ => Brushes.Black
            };
        }

        public static System.Windows.Media.Brush GetWindowsBrush(this PenColorType colorType)
        {
            return colorType switch
            {
                PenColorType.Red => System.Windows.Media.Brushes.Red,
                PenColorType.Orange => System.Windows.Media.Brushes.Orange,
                PenColorType.Yellow => System.Windows.Media.Brushes.Yellow,
                PenColorType.Green => System.Windows.Media.Brushes.Green,
                PenColorType.Cyan => System.Windows.Media.Brushes.Cyan,
                PenColorType.Blue => System.Windows.Media.Brushes.Blue,
                PenColorType.Purple => System.Windows.Media.Brushes.Purple,
                PenColorType.White => System.Windows.Media.Brushes.White,
                _ => System.Windows.Media.Brushes.Black
            };
        }
    }
}