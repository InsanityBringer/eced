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

        public static void getBytes(int num, ref byte[] res)
        {
            res[0] = (byte)(num & 255);
            res[1] = (byte)((num >> 8) & 255);
            res[2] = (byte)((num >> 16) & 255);
            res[3] = (byte)((num >> 24) & 255);
        }
    }
}
