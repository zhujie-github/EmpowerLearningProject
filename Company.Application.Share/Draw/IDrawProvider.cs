using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.Share.Draw
{
    /// <summary>
    /// 绘制操作提供者
    /// </summary>
    public interface IDrawProvider
    {
        void Draw(Graphics graphics);
    }
}
