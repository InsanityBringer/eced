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

namespace eced.Brushes
{
    public class RoomBrush : EditorBrush
    {
        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;
            level.SetTile(tx, ty, z, null);
            if (level.GetTile(tx - 1, ty, z) != null)
                level.SetTile(tx - 1, ty, z, normalTile);
            if (level.GetTile(tx + 1, ty, z) != null)
                level.SetTile(tx + 1, ty, z, normalTile);
            if (level.GetTile(tx, ty + 1, z) != null)
                level.SetTile(tx, ty + 1, z, normalTile);
            if (level.GetTile(tx, ty - 1, z) != null)
                level.SetTile(tx, ty - 1, z, normalTile);
        }
    }
}
