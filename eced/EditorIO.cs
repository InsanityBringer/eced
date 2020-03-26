/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
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
using System.Threading.Tasks;

using eced.ResourceFiles;

namespace eced
{
    public class EditorIO
    {
        private EditorState state;
        public bool HasSavedBefore { get; private set; } = false;
        public string LastFilename { get; private set; } = "Untitled";

        public EditorIO(EditorState state)
        {
            this.state = state;
        }

        /// <summary>
        /// Called when creating a new level, 
        /// </summary>
        public void InitNewLevel()
        {
            HasSavedBefore = false;
            LastFilename = "";
        }

        public bool SaveMapToFile(string filename, bool saveinto)
        {
            if (filename == "")
            {
                if (HasSavedBefore)
                {
                    filename = LastFilename;
                }
                else
                {
                    return false; //Tried to save to the previous file, but it doesn't exist
                }
            }
            else
                LastFilename = filename;

            //TODO: Temp
            bool destArchiveLoaded = state.CurrentMapInfo.ContainsResource(filename);

            //init it off the bat because Visual Studio is being lovely about detecting whether or not it was loaded
            WADArchive savearchive = new WADArchive();

            //make sure the save into archive actually exists
            if (saveinto && !System.IO.File.Exists(filename))
            {
                //if it doesn't turn off save into
                saveinto = false;
            }

            if (destArchiveLoaded)
            {
                //find the resource file for this map
                bool found = false;
                foreach (Archive file in state.CurrentLevel.loadedResources)
                {
                    if (file is WADArchive && file.filename.Equals(filename, StringComparison.OrdinalIgnoreCase))
                    {
                        savearchive = (WADArchive)file;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("The archive for the current save desination is not loaded");
                }
            }
            else
            {
                //add the resource onto the stack before doing anything
                if (saveinto)
                {
                    savearchive = (WADArchive)WADArchive.loadResourceFile(filename);
                    ArchiveHeader lhead = new ArchiveHeader();
                    lhead.filename = filename;
                    state.CurrentMapInfo.files.Add(lhead);
                    destArchiveLoaded = true;
                }
            }

            //Serialize and get an ascii representation of the map
            //It's possible ports might eventually support UTF-8, but for now ASCII is safest bet
            string mapstring = state.CurrentLevel.Serialize();
            byte[] mapdata = Encoding.ASCII.GetBytes(mapstring);

            //Do some special things to save the map special lumps
            List<int> idlist = savearchive.FindMapLumps(state.CurrentMapInfo.lumpname);
            if (destArchiveLoaded && idlist.Count >= 2) //The map is already present, so overwrite the TEXTMAP lump
            {
                savearchive.lumps[idlist[1]].SetCachedData(mapdata);
            }
            else
            {
                Lump mapheader = new Lump(state.CurrentMapInfo.lumpname, 0);
                mapheader.size = 0; savearchive.AddLump(mapheader);
                Lump mapdatal = new Lump("TEXTMAP", mapdata.Length);
                mapdatal.SetCachedData(mapdata); savearchive.AddLump(mapdatal);
                Lump mapend = new Lump("ENDMAP", 0);
                mapend.size = 0; savearchive.AddLump(mapend);
            }

            savearchive.Save(filename);

            if (!destArchiveLoaded)
            {
                //hock the new map onto the resource list
                ArchiveHeader head = new ArchiveHeader();
                head.filename = filename;
                head.format = ResourceFormat.Wad;

                state.CurrentMapInfo.files.Add(head);
            }

            HasSavedBefore = true;
            return true;
        }
    }
}
