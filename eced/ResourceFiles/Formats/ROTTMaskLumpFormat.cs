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

namespace eced.ResourceFiles.Formats
{
    public class ROTTMaskLumpFormat : LumpFormat
    {
        public override bool Classify(Lump header, byte[] data)
        {
            //Needs to be in special namespace
            if ((header.@namespace & LumpNamespace.Rott) == 0) return false;
            //Grab width needs to be <= to actual width
            short grabWidth = BinaryHelper.getInt16(data[0], data[1]);
            short width = BinaryHelper.getInt16(data[2], data[3]);

            int headerLength = 2 * width + 10;

            if (grabWidth > width) return false;

            //Validate the transparency value isn't a pointer
            short trans = BinaryHelper.getInt16(data[10], data[11]);
            if (trans >= headerLength) return false;

            int offset = 12;
            short ptr;
            //Validate all the pointers
            for (int i = 0; i < width; i++)
            {
                ptr = BinaryHelper.getInt16(data[offset], data[offset + 1]);
                offset += 2;
                if (ptr >= data.Length || ptr < headerLength) return false;
            }

            return true;
        }
    }
}
