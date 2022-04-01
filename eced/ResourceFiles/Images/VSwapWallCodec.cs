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

namespace eced.ResourceFiles.Images
{
    public class VSwapWallCodec : ImageCodec
    {
        public override BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette)
        {
            BasicImage image = new BasicImage(64, 64);
            int index;

            //basically a simple transpose
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    index = data[x * 64 + y];
                    image.Data[y * 64 + x] = BinaryHelper.getInt32(palette[index * 3 + 2], palette[index * 3 + 1], palette[index * 3 + 0], 255);
                }
            }

            return image;
        }
    }
}
