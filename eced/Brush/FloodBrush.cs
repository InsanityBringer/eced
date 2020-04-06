/*  ---------------------------------------------------------------------
 *  Copyright (c) 2013 ISB
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

namespace eced.Brushes
{
    public class FloodBrush : EditorBrush
    {
        private Level level;
        private int code = 0;
        public int setCode = -1;
        public FloodBrush(EditorState state)
            : base(state)
        {
            this.Repeatable = false;
        }

        public override void StartBrush(PickResult pos, Level level, int button)
        {
            this.level = level;
            if (setCode < 0)
                code = level.GetUniqueZone();
            else code = setCode;

            FloodFloorCode(pos.x, pos.y, pos.z);
        }

        private void FloodFloorCode(int x, int y, int z)
        {
            if (level.GetTile(x, y, z) != null)
                return;

            if (level.CompareZoneID(x, y, z, code))
                return;

            level.SetZone(x, y, 0, code);

            FloodFloorCode(x - 1, y, z);
            FloodFloorCode(x + 1, y, z);
            FloodFloorCode(x, y - 1, z);
            FloodFloorCode(x, y + 1, z);
        }
    }
}
