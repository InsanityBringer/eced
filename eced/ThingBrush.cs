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
    class ThingBrush : Brush
    {
        public ThingDefinition thing = null;
        public ThingFlags flags = new ThingFlags();
        public ThingManager thinglist;

        public ThingBrush()
            : base()
        {
            this.repeatable = false;
        }

        public override void ApplyToTile(int x, int y, int z, int tilsize, Level level, int button)
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

                    int lx = x / tilsize;
                    int ly = y / tilsize;

                    //Console.WriteLine("placing thing at {0}, {1}", lx, ly);

                    Thing lthing = new Thing();
                    lthing.typeid = thing.id;

                    lthing.x = lx;
                    lthing.y = ly;

                    lthing.flags = flags.getFlags();
                    lthing.angle = flags.angle;

                    level.addThing(lthing);
                }
                else
                {
                    int lx = x / tilsize;
                    int ly = y / tilsize;

                    int oldchunkx = (int)level.highlighted.x / 16;
                    int oldchunky = (int)level.highlighted.y / 16;

                    level.highlighted.moving = true;
                    level.highlighted.x = lx;
                    level.highlighted.y = ly;

                    this.repeatable = true;

                    level.markChunkDirty(lx / 16, ly / 16);
                    level.markChunkDirty(oldchunkx, oldchunky);
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
                        level.replaceThing(oldthing, editor.thing);
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
            }
            this.repeatable = false;
        }
    }
}
