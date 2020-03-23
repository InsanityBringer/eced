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

namespace eced
{
    //this should have existed years ago
    public struct TilePosition
    {
        public int x, y, z;

        public override bool Equals(object obj)
        {
            TilePosition pos = (TilePosition)obj;
            return pos.x == x && pos.y == y && pos.z == z;
        }

        public override int GetHashCode()
        {
            int res = x;
            int t = y >> 30;
            t |= (y << 2);
            res ^= t;
            t = z >> 28;
            t |= (y << 4);
            res ^= t;

            return res;
        }
    }
}
