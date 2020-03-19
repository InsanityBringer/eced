using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eced.ResourceFiles.Images
{
    public class VSwapWallCodec : ImageCodec
    {
        public override BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette)
        {
            BasicImage image = new BasicImage();
            image.x = 64; image.y = 64;
            image.data = new int[64 * 64];
            int index;

            //basically a simple transpose
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    index = data[x * 64 + y];
                    image.data[y * 64 + x] = BinaryHelper.getInt32(palette[index * 3 + 2], palette[index * 3 + 1], palette[index * 3 + 0], 255);
                }
            }

            return image;
        }
    }
}
