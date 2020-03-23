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
    public class TriggerBrush : EditorBrush
    {
        public Trigger trigger = new Trigger();
        public TriggerBrush(EditorState state)
            : base(state)
        {
            this.repeatable = false;
        }

        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int lx = (int)pos.X;
            int ly = (int)pos.Y;

            if (button == 0)
            {
                Trigger triggertoput = new Trigger(trigger);

                triggertoput.x = lx;
                triggertoput.y = ly;
                triggertoput.z = z;

                level.AddTrigger(lx, ly, z, triggertoput);
            }
            else
            {
                TriggerList list = state.CurrentLevel.GetTriggers(lx, ly, z);
                if (list != null)
                {
                    TriggerEditor editor = new TriggerEditor(list, state.TriggerList);
                    editor.ShowDialog();
                    editor.Dispose();
                }
            }
        }
    }
}
