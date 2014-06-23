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
    class Brush
    {
        public bool repeatable = true;
        public Tile normalTile = TileManager.tile1;
        public Brush()
        {
        }

        public virtual void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;
            level.setTile(tx, ty, z, null);
            if (level.getTile(tx - 1, ty, z) != null)
                level.setTile(tx - 1, ty, z, normalTile);
            if (level.getTile(tx + 1, ty, z) != null)
                level.setTile(tx + 1, ty, z, normalTile);
            if (level.getTile(tx, ty + 1, z) != null)
                level.setTile(tx, ty + 1, z, normalTile);
            if (level.getTile(tx, ty - 1, z) != null)
                level.setTile(tx, ty - 1, z, normalTile);
        }

        /*public virtual void ApplyToTile(int x, int y, int z, int tilsize, Level level, int button)
        {
            ApplyToTile(x, y, z, tilsize, level);
        }

        public virtual void ApplyToTile(int x, int y, int z, int tilsize, Level level)
        {
            int tx = x / tilsize;
            int ty = y / tilsize;
            level.setTile(tx, ty, z, null);
            if (level.getTile(tx-1, ty, z) != null)
                level.setTile(tx-1, ty, z, normalTile);
            if (level.getTile(tx + 1, ty, z) != null)
                level.setTile(tx + 1, ty, z, normalTile);
            if (level.getTile(tx, ty + 1, z) != null)
                level.setTile(tx, ty + 1, z, normalTile);
            if (level.getTile(tx, ty - 1, z) != null)
                level.setTile(tx, ty - 1, z, normalTile);
        }*/

        public virtual void EndBrush(Level level)
        {
        }
    }
}
