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
    class TriggerBrush : Brush
    {
        public Trigger trigger = new Trigger();
        public TriggerBrush()
            : base()
        {
            this.repeatable = false;
        }

        public override void ApplyToTile(int x, int y, int z, int tilsize, Level level)
        {
            int lx = x / tilsize;
            int ly = y / tilsize;

            Trigger triggertoput = new Trigger(trigger);

            triggertoput.x = lx;
            triggertoput.y = ly;
            triggertoput.z = 0;

            level.addTrigger(lx, ly, z, triggertoput);
        }
    }
}
