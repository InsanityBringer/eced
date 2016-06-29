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
using CodeImp.DoomBuilder.IO;

namespace eced
{
    public class Tile
    {
        public string texn = "-", texs = "-", texe = "-", texw = "-";
        public string maptex = "-";
        public bool offh = false, offv = false;
        public bool blockn = true, blocks = true, blocke = true, blockw = true;

        public int id;

        public Tile(int id)
        {
            this.id = id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tile))
                return false;

            Tile other = (Tile)obj;

            bool truth = other.blocke == blocke;
            truth = truth && other.blockn == blockn;
            truth = truth && other.blocks == blocks;
            truth = truth && other.blockw == blockw;

            truth = truth && other.offh == offh;
            truth = truth && other.offv == offv;

            truth = truth && other.texe == texe;
            truth = truth && other.texn == texn;
            truth = truth && other.texs == texs;
            truth = truth && other.texw == texw;
            
            return truth;
        }

        /// <summary>
        /// Processes data from the XML tile configuration
        /// </summary>
        /// <param name="tex1">The north texture name</param>
        /// <param name="tex2">The south texture name</param>
        /// <param name="tex3">The east texture name</param>
        /// <param name="tex4">The west texture name</param>
        /// <param name="offh">Whether or not the tile is offset horizontally</param>
        /// <param name="offv">Whether or not the tile is offset vertically</param>
        /// <param name="block1">Block from the north side</param>
        /// <param name="block2">Block from the south side</param>
        /// <param name="block3">Block from the east side</param>
        /// <param name="block4">Block from the west side</param>
        public void processData(string tex1, string tex2, string tex3, string tex4, string offh
            , string offv, string block1, string block2, string block3, string block4)
        {
            if (tex1 != null)
                texn = tex1;
            if (tex2 != null)
                texs = tex2;
            if (tex3 != null)
                texe = tex3;
            if (tex4 != null)
                texw = tex4;

            if (offh != null)
                this.offh = Boolean.Parse(offh);

            if (offv != null)
                this.offv = Boolean.Parse(offv);

            if (block1 != null)
                this.blockn = Boolean.Parse(block1);
            if (block2 != null)
                this.blocks = Boolean.Parse(block1);
            if (block3 != null)
                this.blocke = Boolean.Parse(block1);
            if (block4 != null)
                this.blockw = Boolean.Parse(block1);
        }

        /// <summary>
        /// Creates a string containing the uwmf data for this object
        /// </summary>
        /// <returns></returns>
        public string getUWMFString()
        {
            StringBuilder stringmaker = new StringBuilder();
            stringmaker.Append("tile\n{\n");
            stringmaker.Append("\ttexturenorth = \""); stringmaker.Append(texn); stringmaker.Append("\";");
            stringmaker.Append("\n\ttexturesouth = \""); stringmaker.Append(texs); stringmaker.Append("\";");
            stringmaker.Append("\n\ttextureeast = \""); stringmaker.Append(texe); stringmaker.Append("\";");
            stringmaker.Append("\n\ttexturewest = \""); stringmaker.Append(texw); stringmaker.Append("\";");

            stringmaker.Append("\n\toffsetvertical = "); stringmaker.Append(offv.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\toffsethorizontal = "); stringmaker.Append(offh.ToString().ToLower()); stringmaker.Append(";");

            stringmaker.Append("\n\tblockingnorth = "); stringmaker.Append(blockn.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tblockingsouth = "); stringmaker.Append(blocks.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tblockingeast = "); stringmaker.Append(blocke.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tblockingwest = "); stringmaker.Append(blockw.ToString().ToLower()); stringmaker.Append(";");

            stringmaker.Append("\n}");

            return stringmaker.ToString();
        }

        public static Tile Reconstruct(UniversalCollection data)
        {
            Tile tile = new Tile(0);

            tile.texn = UWMFSearch.getStringTag(data, "texturenorth", "#FF0000");
            tile.texs = UWMFSearch.getStringTag(data, "texturesouth", "#FF0000");
            tile.texe = UWMFSearch.getStringTag(data, "textureeast", "#A00000");
            tile.texw = UWMFSearch.getStringTag(data, "texturewest", "#A00000");

            tile.offh = UWMFSearch.getBoolTag(data, "offsethorizontal", false);
            tile.offv = UWMFSearch.getBoolTag(data, "offsetvertical", false);

            tile.blockn = UWMFSearch.getBoolTag(data, "blockingnorth", true);
            tile.blocks = UWMFSearch.getBoolTag(data, "blockingsouth", true);
            tile.blocke = UWMFSearch.getBoolTag(data, "blockingeast", true);
            tile.blockw = UWMFSearch.getBoolTag(data, "blockingwest", true);

            return tile;
        }
    }
}
