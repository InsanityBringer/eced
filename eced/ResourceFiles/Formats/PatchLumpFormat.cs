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

namespace eced.ResourceFiles.Formats
{
    public class PatchLumpFormat : LumpFormat
    {
        //TODO: This test needs to be reworked if VSWAP sprites are allowed in non-VSWAP archives
        //since the simplicity of this check will include VSWAP sprites. 
        public override bool Classify(ResourceFile header, byte[] data)
        {
            //Attempt to detect patch through some heruistics
            if (data.Length < 8) return false;
            short w = BinaryHelper.getInt16(data[0], data[1]);
            short h = BinaryHelper.getInt16(data[2], data[3]);
            if (w > 0 && h > 0)
            {
                header.format = LumpFormatType.DoomPatch;
                return true;
            }
            return false;
        }
    }
}
