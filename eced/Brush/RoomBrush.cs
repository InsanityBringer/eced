﻿/*  ---------------------------------------------------------------------
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
        public RoomBrush(EditorState state) : base(state) { }

        public override void StartBrush(PickResult pos, Level level, int button)
        {
            ApplyToTile(pos, level, button);
        }
        public override void ApplyToTile(PickResult pos, Level level, int button)
        {
            level.SetTile(pos.x, pos.y, pos.z, null);
            if (button != 1)
            {
                if (level.GetTile(pos.x - 1, pos.y, pos.z) != null)
                    level.SetTile(pos.x - 1, pos.y, pos.z, normalTile);
                if (level.GetTile(pos.x + 1, pos.y, pos.z) != null)
                    level.SetTile(pos.x + 1, pos.y, pos.z, normalTile);
                if (level.GetTile(pos.x, pos.y + 1, pos.z) != null)
                    level.SetTile(pos.x, pos.y + 1, pos.z, normalTile);
                if (level.GetTile(pos.x, pos.y - 1, pos.z) != null)
                    level.SetTile(pos.x, pos.y - 1, pos.z, normalTile);
            }
        }
    }
}
