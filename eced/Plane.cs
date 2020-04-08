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
using CodeImp.DoomBuilder.IO;

namespace eced
{
    public class Plane
    {

        public Cell[,] cells;


        public int Width { get; } = 64;
        public int Height { get; } = 64;
        public int Depth { get; set; } = 64; //fine to mutate depth
        public List<Thing> Things { get; } = new List<Thing>();

        public Plane(int w, int h)
        {
            this.Width = w;
            this.Height = h;

            cells = new Cell[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    cells[x, y].tile = 0;
                    cells[x, y].sector = 0;
                    cells[x, y].zone = -1;
                }
            }
        }

        public String Serialize()
        {
            String str = "plane\n{\n";
            str += "\tdepth = " + Depth.ToString() + ";\n";
            str += "}";

            return str;
        }

        public static Plane Deserialize(Level level, UniversalCollection data)
        {
            Plane plane = new Plane(level.Width, level.Height);
            plane.Depth = UWMFSearch.getIntTag(data, "depth", 64);
            return plane;
        }
    }
}
