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
    public class PNGLumpFormat : LumpFormat
    {
        public override bool Classify(ResourceFile lump, byte[] data)
        {
            if (lump.size < 4) return false;
            if (data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71)
            {
                return true;
            }
            return false;
        }
    }
}
