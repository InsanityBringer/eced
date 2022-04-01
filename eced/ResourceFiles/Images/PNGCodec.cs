/*  ---------------------------------------------------------------------
 *  Copyright (c) 2022 ISB
 *
 *  eced is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *   eced is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with eced.  If not, see <http://www.gnu.org/licenses/>.
 *  -------------------------------------------------------------------*/

using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace eced.ResourceFiles.Images
{
    public class PNGCodec : ImageCodec
    {
        public override BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette)
        {
            MemoryStream stream = new MemoryStream(data);
            Bitmap bitmap = new Bitmap(stream); //godawful hack #1

            BasicImage image = new BasicImage(bitmap.Width, bitmap.Height);

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(bits.Scan0, image.Data, 0, bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bits);
            bitmap.Dispose();

            return image;
        }

        //Hack #38572984572489572498572: Convenience member for getting BasicImage, since texture code only uses BasicImages atm
        public static BasicImage BasicImageFromBitmap(Bitmap bitmap)
        {
            BasicImage image = new BasicImage(bitmap.Width, bitmap.Height);
            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(bits.Scan0, image.Data, 0, bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bits);

            return image;
        }
    }
}
