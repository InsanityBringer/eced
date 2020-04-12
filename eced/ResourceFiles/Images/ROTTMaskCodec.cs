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
    public class ROTTMaskCodec : ImageCodec
    {
        public override BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette)
        {
            //Current offset in the patch
            int offset = 12;

            short w = BinaryHelper.getInt16(data[2], data[3]);
            short h = BinaryHelper.getInt16(data[4], data[5]);
            short alpha = BinaryHelper.getInt16(data[10], data[11]);

            int[] output = new int[w * h];

            //Load the image data for each patch
            for (int i = 0; i < w; i++)
            {
                int pointer = BinaryHelper.getInt16(data[offset], data[offset + 1]);
                offset += 2;

                //Load the offset and length of the patch
                byte yoffs = data[pointer]; pointer++;
                while (yoffs != 255)
                {
                    byte len = data[pointer]; pointer++;
                    byte test = data[pointer];
                    if (test == 0xFE)
                    {
                        for (int p = 0; p < len; p++)
                        {
                            output[i + (yoffs + p) * w] = BinaryHelper.getInt32(0, 0, 0, 128);
                        }
                        pointer++;
                    }
                    else
                    {
                        //Use the current palette to build a 32bit patch
                        for (int p = 0; p < len; p++)
                        {
                            byte patchbyte = data[pointer + p];
                            output[i + (yoffs + p) * w] = BinaryHelper.getInt32(palette[patchbyte * 3 + 2], palette[patchbyte * 3 + 1], palette[patchbyte * 3 + 0], 255);
                        }
                        pointer += len;
                    }

                    yoffs = data[pointer]; pointer++;
                }
            }

            BasicImage img = new BasicImage();
            img.x = w; img.y = h; img.data = output;
            return img;
        }
    }
}
