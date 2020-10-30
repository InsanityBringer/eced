/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eced.ResourceFiles.Images
{
    //At least this isn't munged like doom alpha's pic format...
    //Used for the 128x128 flats. 
    public class ROTTPicCodec : ImageCodec
    {
        public override BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette)
        {
            BasicImage image = new BasicImage();

            short w = BinaryHelper.getInt16(data[0], data[1]);
            short h = BinaryHelper.getInt16(data[2], data[3]);

            image.x = w; image.y = h;
            image.data = new int[w * h];
            int index;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    index = data[y * w + x + 8];
                    image.data[y * w + x] = BinaryHelper.getInt32(palette[index * 3 + 2], palette[index * 3 + 1], palette[index * 3 + 0], 255);
                }
            }

            return image;
        }
    }
}
