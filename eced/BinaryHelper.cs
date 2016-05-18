using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced
{
    class BinaryHelper
    {
        public static int getInt32(byte b1, byte b2, byte b3, byte b4)
        {
            return b1 + (b2 << 8) + (b3 << 16) + (b4 << 24);
        }

        public static short getInt16(byte b1, byte b2)
        {
            return (short)(b1 + (b2 << 8));
        }
    }
}
