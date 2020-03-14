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
    public class ThingBrush : Brush
    {
        public ThingDefinition thing = null;
        public ThingFlags flags = new ThingFlags();
        public ThingManager thinglist;

        public ThingBrush()
            : base()
        {
            this.repeatable = false;
        }

        //public override void ApplyToTile(int x, int y, int z, int tilsize, Level level, int button)
        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            if (button == 0)
            {
                if (level.highlighted == null)
                {
                    //int lx = (int)((double)x / ((double)tilsize / 64d));
                    //int ly = (int)((double)y / ((double)tilsize / 64d));

                    /*int lx = (x / tilsize);
                    int ly = (y / tilsize);

                    lx *= tilsize * 2;
                    ly *= tilsize * 2;

                    lx += tilsize; ly += tilsize;*/

                    //int lx = x / tilsize;
                    //int ly = y / tilsize;

                    //Console.WriteLine("placing thing at {0}, {1}", lx, ly);

                    Thing lthing = new Thing();
                    lthing.typeid = thing.id;

                    lthing.x = (int)pos.X + .5f;
                    lthing.y = (int)pos.Y + .5f;

                    lthing.flags = flags.getFlags();
                    lthing.angle = flags.angle;

                    level.AddThing(lthing);

                    Console.WriteLine("adding thing at {0}, {1}", lthing.x, lthing.y);
                }
                else
                {
                    level.highlighted.moving = true;
                    level.highlighted.x = pos.X;
                    level.highlighted.y = pos.Y;

                    this.repeatable = true;
                }
            }
            else if (button == 1)
            {
                if (level.highlighted != null)
                {
                    ThingEditor editor = new ThingEditor(level.highlighted, thinglist);
                    if (editor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Thing oldthing = level.highlighted;
                        level.highlighted = editor.thing;
                        level.ReplaceThing(oldthing, editor.thing);
                    }
                    editor.Dispose();
                }
            }
        }

        public override void EndBrush(Level level)
        {
            if (level.highlighted != null)
            {
                level.highlighted.moving = false;
                level.highlighted.x = ((int)(level.highlighted.x)) + .5f;
                level.highlighted.y = ((int)(level.highlighted.y)) + .5f;
            }
            this.repeatable = false;
        }
    }
}
