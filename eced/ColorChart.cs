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

namespace eced
{
    public class ColorChart
    {
        public int[] Colors { get; } = new int[4096];

        public void Init()
        {
            int r, g, b;

            int i2 = 0;
            for (int i = 0; i < 4096; i++)
            {
                i2 += 2037;
                i2 = i2 % 4096;
                r = ((i2 >> 8) & 15) * 8 + 64;
                g = ((i2 >> 4) & 15) * 8 + 64;
                b = (i2 & 15) * 8 + 64;

                Colors[i] = (r & 255) + ((g & 255) << 8) + ((b & 255) << 16) + (255 << 24);
            }

            /*Random random = new Random(54074);
            int choice;
            for (int i = 0; i < 4096; i++)
            {
                choice = random.Next(4096 - i);
                Colors[i] = localColors[choice];
                localColors[choice] = localColors[4095 - i];
            }*/
        }
    }
}
