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
    public class ThingDefinition
    {
        public int radius = 16, height = 64;

        public int r = 0, g = 0, b = 0;

        public string name = "Dummy";

        public int id = -1;

        public ThingDefinition()
        {
        }

        public void setData(string radius, string height, string name, string type, string id)
        {
            if (radius != null)
                this.radius = Int32.Parse(radius);
            if (height != null)
                this.height = Int32.Parse(height);

            if (name != null)
                this.name = name;

            if (id != null)
                this.id = Int32.Parse(id);

            if (type != null)
            {
                switch (type)
                {
                    case "Spawn":
                        r = 0;
                        g = 128;
                        b = 0;
                        break;
                    case "Internal":
                        r = 128;
                        g = 128;
                        b = 128;
                        break;
                    case "Enemy":
                        r = 128;
                        g = 0;
                        b = 0;
                        break;
                }
            }
        }
    }
}
