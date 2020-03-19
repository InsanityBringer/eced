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

namespace eced.ResourceFiles.Images
{
    public class DoomPatchCodec : ImageCodec
    {
        public override BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette)
        {
            //Current offset in the patch
            int offset = 8;

            short w = BinaryHelper.getInt16(data[0], data[1]);
            short h = BinaryHelper.getInt16(data[2], data[3]);

            int[] output = new int[w * h];

            //Load the image data for each patch
            for (int i = 0; i < w; i++)
            {
                int pointer = BinaryHelper.getInt32(data[offset], data[offset + 1], data[offset + 2], data[offset + 3]);
                offset += 4;

                //Load the offset and length of the patch
                byte yoffs = data[pointer]; pointer++;
                while (yoffs != 255)
                {
                    byte len = data[pointer]; pointer++;
                    pointer++; //Garbage byte
                    //Use the current palette to build a 32bit patch
                    for (int p = 0; p < len; p++)
                    {
                        byte patchbyte = data[pointer + p];
                        output[i + (yoffs + p) * w] = BinaryHelper.getInt32(palette[patchbyte * 3 + 2], palette[patchbyte * 3 + 1], palette[patchbyte * 3 + 0], 255);
                    }
                    pointer += len + 1;

                    yoffs = data[pointer]; pointer++;
                }
            }

            BasicImage img = new BasicImage();
            img.x = w; img.y = h; img.data = output;
            return img;
        }
    }
}
