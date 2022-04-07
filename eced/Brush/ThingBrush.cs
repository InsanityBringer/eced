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
using eced.GameConfig;

namespace eced.Brushes
{
    public class ThingBrush : EditorBrush
    {
        //public ThingDefinition thing = null;
        public string ThingType { get; set; } = "$Player1Start";
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

        public override void StartBrush(PickResult pos, Level level, int button)
        {
            base.StartBrush(pos, level, button);
            if (button == 0)
            {
                if (state.HighlightedThing != null)
                {
                    state.ToggleSelectedThing(state.HighlightedThing);
                }
                else
                {
                    state.ClearSelectedThings();
                }
            }
            else if (button == 1)
            {
                if (state.SelectedThings.Count == 0 && state.HighlightedThing == null)
                {
                    Thing lthing = new Thing();
                    lthing.type = ThingType;

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
                else if (state.HighlightedThing != null)
                {
                    Repeatable = true; //Summon the editor at release if the highlighted thing is selected
                    lastMovePos = pos;
                }
            }
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
                float sx = lastMovePos.x + lastMovePos.xf;
                float sy = lastMovePos.y + lastMovePos.yf;

                float ex = pos.x + pos.xf;
                float ey = pos.y + pos.yf;

                float dx = (ex - sx);
                float dy = (ey - sy);

                if (Math.Abs(dx) > 0.2 || Math.Abs(dy) > 0.2)
                    moving = true;
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
            else if (Repeatable) //I need to do some changes to the brush API
            {
                List<Thing> editThings = null;
                if (state.SelectedThings.Count > 0)
                {
                    editThings = state.SelectedThings;
                }
                else if (state.HighlightedThing != null)
                {
                    editThings = new List<Thing>();
                    editThings.Add(state.HighlightedThing);
                }
                if (editThings != null)
                {
                    ThingEditor editor = new ThingEditor(state, editThings);
                    if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        editor.ApplyChanges();
                    editor.Dispose();
                }
            }
            moving = false;
            Repeatable = false;
        }

        public override BrushMode GetMode()
        {
            return BrushMode.Things;
        }
    }
}
