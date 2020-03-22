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

using System.Text;
using CodeImp.DoomBuilder.IO;

namespace eced
{
    public class Sector
    {
        public string FloorTexture { get; private set; } = "#717171";
        public string CeilingTexture { get; private set; } = "#383838";
        public int Light { get; private set; } = 255;

        public Sector() { }

        public Sector(Sector other)
        {
            FloorTexture = other.FloorTexture;
            CeilingTexture = other.CeilingTexture;
            Light = other.Light;
        }

        public string Serialize()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("sector\n{\n");
            stringBuilder.AppendFormat("\ttexturefloor = \"{0}\";\n",FloorTexture);
            stringBuilder.AppendFormat("\ttextureceiling = \"{0}\";\n", CeilingTexture);
            stringBuilder.AppendFormat("\tlight = \"{0}\";\n", Light);
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public static Sector Deserialize(UniversalCollection data)
        {
            Sector newSector = new Sector();

            newSector.FloorTexture = UWMFSearch.getStringTag(data, "texturefloor", "#717171");
            newSector.CeilingTexture = UWMFSearch.getStringTag(data, "textureceiling", "#383838");
            
            return newSector;
        }

        public Sector ChangeTextures(string floor, string ceil)
        {
            Sector newSector = new Sector(this);

            newSector.FloorTexture = floor;
            newSector.CeilingTexture = ceil;

            return newSector;
        }

        public Sector ChangeLight(int light)
        {
            Sector newSector = new Sector(this);
            newSector.Light = light;

            return newSector;
        }

        public override int GetHashCode()
        {
            //TODO: OPTIMIZE
            StringBuilder hack = new StringBuilder();
            hack.Append(FloorTexture); hack.Append(CeilingTexture);
            return hack.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Sector t = (Sector)obj;

            return t.FloorTexture == this.FloorTexture && t.CeilingTexture == this.CeilingTexture && t.Light == Light;
        }
    }
}
