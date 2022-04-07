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
    public enum BrushMode
    {
        Tiles,
        Things,
        Triggers
    }
    public class EditorBrush
    {
        public bool Repeatable { get; protected set; } = true;
        public bool Interpolated { get; protected set; } = true;
        public Tile normalTile;
        protected EditorState state;
        public EditorBrush(EditorState state)
        {
            this.state = state;
        }

        public virtual void StartBrush(PickResult pos, Level level, int button)
        {
        }

        public virtual void ApplyToTile(PickResult pos, Level level, int button)
        {
        }

        public virtual void EndBrush(Level level)
        {
        }

        public virtual BrushMode GetMode()
        {
            return BrushMode.Tiles;
        }
    }
}
