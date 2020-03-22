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
using System.Xml.Linq;

namespace eced
{
    public enum ThingIcon
    {
        Arrow,
        Ammo,
        Weapon,
        Cross,
        Key,
        Treasure,
        Powerup,
        Decoration
    }
    public class ThingDefinition
    {
        public int Radius { get; private set; } = 16;
        public int Height { get; private set; } = 64;

        public int R { get; private set; } = 0;
        public int G { get; private set; } = 0;
        public int B { get; private set; } = 0;

        public string Name { get; private set; } = "Dummy";

        public string Type { get; private set; } = "";
        public ThingIcon Icon { get; private set; } = 0;

        public ThingDefinition()
        {
        }

        public static ThingDefinition FromXContainer(XContainer container)
        {
            ThingDefinition def = new ThingDefinition();

            foreach (XElement elem in container.Elements())
            {
                //Console.WriteLine(elem.Name.LocalName);
                switch (elem.Name.LocalName)
                {
                    case "radius":
                        def.Radius = (int)elem;
                        break;
                    case "height":
                        def.Height = (int)elem;
                        break;
                    case "name":
                        def.Name = (string)elem;
                        break;
                    case "id":
                        def.Type = (string)elem;
                        break;
                    case "r":
                        def.R = (int)elem;
                        break;
                    case "g":
                        def.G = (int)elem;
                        break;
                    case "b":
                        def.B = (int)elem;
                        break;
                    case "icon":
                        def.Icon = (ThingIcon)((int)elem);
                        break;
                }
            }

            return def;
        }

        public void SetData(string radius, string height, string name, string id, int r, int g, int b)
        {
            if (radius != null)
                this.Radius = Int32.Parse(radius);
            if (height != null)
                this.Height = Int32.Parse(height);

            if (name != null)
                this.Name = name;

            if (id != null)
                this.Type = id;

            this.R = r; this.G = g; this.B = b;
        }
    }
}
