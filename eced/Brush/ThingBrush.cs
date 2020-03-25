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
using eced.GameConfig;

namespace eced.Brushes
{
    public class ThingBrush : EditorBrush
    {
        public ThingDefinition thing = null;
        public ThingFlags flags = new ThingFlags();
        public ThingManager thinglist;

        public ThingBrush(EditorState state)
            : base(state)
        {
            this.repeatable = false;
        }

        //public override void ApplyToTile(int x, int y, int z, int tilsize, Level level, int button)
        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            if (button == 0)
            {
                if (state.HighlightedThing == null)
                {
                    Thing lthing = new Thing();
                    lthing.type = thing.Type;

                    lthing.x = (int)pos.X + .5f;
                    lthing.y = (int)pos.Y + .5f;

                    lthing.flags = flags.getFlags();
                    lthing.angle = flags.angle;

                    level.AddThing(lthing);

                    Console.WriteLine("adding thing at {0}, {1}", lthing.x, lthing.y);
                }
                else
                {
                    state.ToggleSelectedThing(state.HighlightedThing);
                }
            }
            else if (button == 1)
            {
                if (state.HighlightedThing != null)
                {
                    //TODO: needs to be rethunk
                    /*ThingEditor editor = new ThingEditor(state.HighlightedThing, thinglist);
                    if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Thing oldthing = state.HighlightedThing;
                        state.HighlightedThing = editor.thing;
                        level.ReplaceThing(oldthing, editor.thing);
                    }
                    editor.Dispose();*/
                }
            }
        }

        public override void EndBrush(Level level)
        {
            if (state.HighlightedThing != null)
            {
                state.HighlightedThing.moving = false;
                state.HighlightedThing.x = ((int)(state.HighlightedThing.x)) + .5f;
                state.HighlightedThing.y = ((int)(state.HighlightedThing.y)) + .5f;
            }
            this.repeatable = false;
        }
    }
}
