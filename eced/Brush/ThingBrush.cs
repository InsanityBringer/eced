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

        private PickResult lastMovePos;
        private bool moving = false;

        public int gridGranularity = 4;

        public ThingBrush(EditorState state)
            : base(state)
        {
            this.Repeatable = false;
            this.Interpolated = false;
        }

        public override void ApplyToTile(PickResult pos, Level level, int button)
        {
            if (moving)
            {
                float sx = lastMovePos.x + lastMovePos.xf;
                float sy = lastMovePos.y + lastMovePos.yf;

                float ex = pos.x + pos.xf;
                float ey = pos.y + pos.yf;

                float dx = (ex - sx);
                float dy = (ey - sy);

                //float dx = ((int)((ex - sx) * gridGranularity)) / gridGranularity;
                //float dy = ((int)((ey - sy) * gridGranularity)) / gridGranularity;

                foreach (Thing thing in state.SelectedThings)
                {
                    thing.x += dx;
                    thing.y += dy;
                }
                lastMovePos = pos;
            }
            else
            {
                if (button == 0)
                {
                    if (state.HighlightedThing == null)
                    {
                        Thing lthing = new Thing();
                        lthing.type = thing.Type;

                        lthing.x = pos.x + .5f;
                        lthing.y = pos.y + .5f;

                        lthing.angle = flags.angle;
                        lthing.ambush = flags.ambush;
                        lthing.patrol = flags.patrol;
                        lthing.skill1 = flags.skill1;
                        lthing.skill2 = flags.skill2;
                        lthing.skill3 = flags.skill3;
                        lthing.skill4 = flags.skill4;

                        level.AddThing(lthing);
                    }
                    else
                    {
                        state.ToggleSelectedThing(state.HighlightedThing);
                    }
                }
                else if (button == 1)
                {
                    if (state.SelectedThings.Count > 0)
                    {
                        //a hack aaaa
                        moving = true;
                        Repeatable = true;
                        lastMovePos = pos;
                    }
                    else if (state.HighlightedThing != null)
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
        }

        public override void EndBrush(Level level)
        {
            if (moving)
            {
                foreach (Thing thing in state.SelectedThings)
                {
                    thing.x = (float)Math.Round(thing.x * gridGranularity, MidpointRounding.AwayFromZero) / (float)gridGranularity;
                    thing.y = (float)Math.Round(thing.y * gridGranularity, MidpointRounding.AwayFromZero) / (float)gridGranularity;
                }
            }
            moving = false;
            this.Repeatable = false;
        }
    }
}
