namespace Company.Core.Models
{
    /// <summary>
    /// 24位颜色结构体
    /// </summary>
    public struct ColorBGR
    {
        public byte B { get; private set; }
        public byte G { get; private set; }
        public byte R { get; private set; }

        public ColorBGR(byte b, byte g, byte r)
        {
            B = b;
            G = g;
            R = r;
        }
    }
}
