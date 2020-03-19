using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace eced.ResourceFiles.Images
{
    public class PNGCodec : ImageCodec
    {
        public override BasicImage DecodeImage(ResourceFile lump, byte[] data, byte[] palette)
        {
            MemoryStream stream = new MemoryStream(data);
            Bitmap bitmap = new Bitmap(stream); //godawful hack #1

            //godawful hack #2: So I need this in a managed array. Because I like slow things with no perf
            int[] output = new int[bitmap.Width * bitmap.Height];

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(bits.Scan0, output, 0, bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bits);

            BasicImage image = new BasicImage();
            image.x = bitmap.Width; image.y = bitmap.Height; image.data = output;
            return image;
        }

        //Hack #38572984572489572498572: Convenience member for getting BasicImage, since texture code only uses BasicImages atm
        public static BasicImage BasicImageFromBitmap(Bitmap bitmap)
        {
            int[] output = new int[bitmap.Width * bitmap.Height];

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(bits.Scan0, output, 0, bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bits);

            BasicImage image = new BasicImage();
            image.x = bitmap.Width; image.y = bitmap.Height; image.data = output;
            return image;
        }
    }
}
