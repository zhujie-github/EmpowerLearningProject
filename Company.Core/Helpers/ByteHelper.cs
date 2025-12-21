using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Core.Helper
{
    public class ByteHelper
    {
        public static byte Check(byte[] data)
        {
            //对同一数组内数据进行异或           
            int i;
            byte x;
            x = 0;
            for (i = 0; i < data.Length; i++)
            {
                x ^= data[i];

            }
            return x;
        }
    }
}
