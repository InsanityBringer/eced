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
        int height = 64;

        public Cell[,] cells;
        public List<OpenTK.Vector2> cellsWithTriggers = new List<OpenTK.Vector2>();

        int w = 64, h = 64;
        
        public Plane(int w, int h)
        {
            this.w = w;
            this.h = h;

            cells = new Cell[w, h];
        }

        public String Serialize()
        {
            String str = "plane\n{\n";
            str += "\tdepth = " + height.ToString() + ";\n";
            str += "}";

            return str;
        }

        public static Plane Reconstruct(Level level, UniversalCollection data)
        {
            Plane plane = new Plane(level.Width, level.Height);
            plane.height = UWMFSearch.getIntTag(data, "depth", 64);
            return plane;
        }
    }
}
