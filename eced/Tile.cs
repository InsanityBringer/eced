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
        public string NorthTex { get; private set; } = "-";
        public string SouthTex { get; private set; } = "-";
        public string EastTex { get; private set; } = "-";
        public string WestTex { get; private set; } = "-";
        public string MapTex { get; private set; } = "-";
        public bool HorizOffset { get; private set; } = false;
        public bool VerticalOffset { get; private set; } = false;
        public bool NorthBlock { get; private set; } = true;
        public bool SouthBlock { get; private set; } = true;
        public bool EastBlock { get; private set; } = true;
        public bool WestBlock { get; private set; } = true;
        public string Name { get; private set; } = "-";
        public string SoundSequence { get; private set; } = "";

        public Tile()
        {
        }

        public Tile(Tile other)
        {
            NorthTex = other.NorthTex;
            SouthTex = other.SouthTex;
            EastTex = other.EastTex;
            WestTex = other.WestTex;
            MapTex = other.MapTex;

            HorizOffset = other.HorizOffset;
            VerticalOffset = other.VerticalOffset;

            NorthBlock = other.NorthBlock;
            SouthBlock = other.SouthBlock;
            EastBlock = other.EastBlock;
            WestBlock = other.WestBlock;

            SoundSequence = other.SoundSequence;
        }

        public override int GetHashCode()
        {
            //TODO: godawful hack, fix
            StringBuilder hack = new StringBuilder();
            hack.Append(NorthTex); hack.Append(SouthTex); hack.Append(EastTex); hack.Append(WestTex);
            return hack.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tile))
                return false;

            Tile other = (Tile)obj;

            bool truth = other.EastBlock == EastBlock;
            truth = truth && other.NorthBlock == NorthBlock;
            truth = truth && other.SouthBlock == SouthBlock;
            truth = truth && other.WestBlock == WestBlock;

            truth = truth && other.HorizOffset == HorizOffset;
            truth = truth && other.VerticalOffset == VerticalOffset;

            truth = truth && other.EastTex == EastTex;
            truth = truth && other.NorthTex == NorthTex;
            truth = truth && other.SouthTex == SouthTex;
            truth = truth && other.WestTex == WestTex;
            
            return truth;
        }

        public static Tile FromXMLContainer(XContainer container)
        {
            Tile newTile = new Tile();

            foreach (XElement elem in container.Elements())
            {
                //Console.WriteLine(elem.Name.LocalName);
                switch (elem.Name.LocalName)
                {
                    case "name":
                        newTile.Name = (string)elem;
                        break;
                    case "texn":
                        newTile.NorthTex = (string)elem;
                        break;
                    case "texs":
                        newTile.SouthTex = (string)elem;
                        break;
                    case "texe":
                        newTile.EastTex = (string)elem;
                        break;
                    case "texw":
                        newTile.WestTex = (string)elem;
                        break;
                    case "maptex":
                        newTile.MapTex = (string)elem;
                        break;
                    case "offh":
                        newTile.HorizOffset = (bool)elem;
                        break;
                    case "offv":
                        newTile.VerticalOffset = (bool)elem;
                        break;
                    case "blockn":
                        newTile.NorthBlock = (bool)elem;
                        break;
                    case "blocks":
                        newTile.SouthBlock = (bool)elem;
                        break;
                    case "blocke":
                        newTile.EastBlock = (bool)elem;
                        break;
                    case "blockw":
                        newTile.WestBlock = (bool)elem;
                        break;
                    case "sndseq":
                        newTile.SoundSequence = (string)elem;
                        break;
                }
            }

            return newTile;
        }

        /// <summary>
        /// Creates a string containing the uwmf data for this object
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            StringBuilder stringmaker = new StringBuilder();
            stringmaker.Append("tile\n{\n");
            stringmaker.Append("\ttexturenorth = \""); stringmaker.Append(NorthTex); stringmaker.Append("\";");
            stringmaker.Append("\n\ttexturesouth = \""); stringmaker.Append(SouthTex); stringmaker.Append("\";");
            stringmaker.Append("\n\ttextureeast = \""); stringmaker.Append(EastTex); stringmaker.Append("\";");
            stringmaker.Append("\n\ttexturewest = \""); stringmaker.Append(WestTex); stringmaker.Append("\";");
            if (MapTex != "-")
            {
                stringmaker.Append("\n\ttextureoverhead = \""); stringmaker.Append(MapTex); stringmaker.Append("\";");
            }

            stringmaker.Append("\n\toffsetvertical = "); stringmaker.Append(VerticalOffset.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\toffsethorizontal = "); stringmaker.Append(HorizOffset.ToString().ToLower()); stringmaker.Append(";");

            stringmaker.Append("\n\tblockingnorth = "); stringmaker.Append(NorthBlock.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tblockingsouth = "); stringmaker.Append(SouthBlock.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tblockingeast = "); stringmaker.Append(EastBlock.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tblockingwest = "); stringmaker.Append(WestBlock.ToString().ToLower()); stringmaker.Append(";");

            if (SoundSequence != "")
            {
                stringmaker.Append("\n\tsoundsequence = \""); stringmaker.Append(SoundSequence); stringmaker.Append("\";");
            }

            stringmaker.Append("\n}");

            return stringmaker.ToString();
        }

        public static Tile Deserialize(UniversalCollection data)
        {
            Tile tile = new Tile();

            tile.NorthTex = UWMFSearch.getStringTag(data, "texturenorth", "#FF0000");
            tile.SouthTex = UWMFSearch.getStringTag(data, "texturesouth", "#FF0000");
            tile.EastTex = UWMFSearch.getStringTag(data, "textureeast", "#A00000");
            tile.WestTex = UWMFSearch.getStringTag(data, "texturewest", "#A00000");

            tile.HorizOffset = UWMFSearch.getBoolTag(data, "offsethorizontal", false);
            tile.VerticalOffset = UWMFSearch.getBoolTag(data, "offsetvertical", false);

            tile.NorthBlock = UWMFSearch.getBoolTag(data, "blockingnorth", true);
            tile.SouthBlock = UWMFSearch.getBoolTag(data, "blockingsouth", true);
            tile.EastBlock = UWMFSearch.getBoolTag(data, "blockingeast", true);
            tile.WestBlock = UWMFSearch.getBoolTag(data, "blockingwest", true);

            return tile;
        }

        public Tile ChangeTextures(string north, string south, string east, string west, string map)
        {
            Tile newTile = new Tile(this);

            newTile.NorthTex = north;
            newTile.SouthTex = south;
            newTile.EastTex = east;
            newTile.WestTex = west;
            newTile.MapTex = map;

            return newTile;
        }

        public Tile ChangeSoundSequence(string sndSeq)
        {
            Tile newTile = new Tile(this);
            newTile.SoundSequence = sndSeq;
            return newTile;
        }

        public Tile ChangeBlocking(bool north, bool south, bool east, bool west)
        {
            Tile newTile = new Tile(this);

            newTile.NorthBlock = north;
            newTile.SouthBlock = south;
            newTile.EastBlock = east;
            newTile.WestBlock = west;

            return newTile;
        }

        public Tile ChangeInset(bool horizontal, bool vertical)
        {
            Tile newTile = new Tile(this);

            newTile.HorizOffset = horizontal;
            newTile.VerticalOffset = vertical;

            return newTile;
        }
    }
}
