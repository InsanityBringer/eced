using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eced.ResourceFiles.Images
{
    public class BasicImage
    {
        public int x, y;
        public int[] data; //rgba8 heh
    }

    //codec makes these classes sound more involved than they actually are, but I can't think of a better name when the thing that manages these is already ImageDecoder
    public abstract class ImageCodec
    {
        public abstract BasicImage DecodeImage(ResourceFile lump, byte[] data, byte[] palette);
    }
}
