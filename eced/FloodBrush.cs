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

namespace eced
{
    class FloodBrush : Brush
    {
        private Level level;
        private int code = 0;
        public int setCode = -1;
        public FloodBrush()
            : base()
        {
            this.repeatable = false;
        }

        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;

            this.level = level;
            if (setCode < 0)
                code = level.getUniqueCode();
            else code = setCode;

            flood(tx, ty);

            this.level = null; //no need to let that linger
        }

        private void flood(int x, int y)
        {
            if (level.getTile(x, y, 0) != null)
                return;

            if (level.compareZoneID(x, y, 0, code))
                return;

            level.assignFloorCode(x, y, 0, code);

            flood(x - 1, y);
            flood(x + 1, y);
            flood(x, y - 1);
            flood(x, y + 1);
        }
    }
}
