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

namespace eced.ResourceFiles.Formats
{
    public class FlatLumpFormat : LumpFormat
    {
        public override bool Classify(Lump header, byte[] data)
        {
            if (header.@namespace != LumpNamespace.Flat) return false;
            if (header.size != 4096) return false;
            return true;
        }
    }
}
