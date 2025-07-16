namespace Company.Core.Models
{
    /// <summary>
    /// 32位颜色结构体
    /// </summary>
    public struct ColorBGRA
    {
        public byte B { get; private set; }
        public byte G { get; private set; }
        public byte R { get; private set; }
        public byte A { get; private set; }

        public ColorBGRA(byte b, byte g, byte r, byte a)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }
    }
}
