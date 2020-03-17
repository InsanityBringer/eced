﻿/*  ---------------------------------------------------------------------
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
using System.Text;

using eced.Brushes;

namespace eced
{
    public class EditorState
    {
        private EditorBrush currentBrush;
        public EditorBrush[] BrushList { get; } = new EditorBrush[9];

        //fields for current primitive types
        public Tile currentTile;
        public ThingDefinition currentThingDef;

        //properties for the current editor state
        public MapInformation CurrentMapInfo { get; private set; }
        public Level CurrentLevel { get; private set; }
        public TileManager TileList { get; private set; }
        public ThingManager ThingList { get; private set; }

        private void CreateBrushes()
        {
            BrushList[0] = new EditorBrush();
            BrushList[1] = new RoomBrush(); BrushList[1].normalTile = TileList.tileset[0];
            BrushList[2] = new TileBrush(); BrushList[2].normalTile = TileList.tileset[0];
            BrushList[3] = new EditorBrush();
            BrushList[4] = new ThingBrush(); ((ThingBrush)BrushList[4]).thinglist = ThingList; ((ThingBrush)BrushList[4]).thing = ThingList.thingList[0];
            BrushList[5] = new TriggerBrush();
            BrushList[6] = new SectorBrush();
            BrushList[7] = new FloodBrush();
            BrushList[8] = new TagTool();
        }

        private void LoadGameConfiguration()
        {
            //TODO: Actual game configuration elements
            TileList = new TileManager("./resources/wolftiles.xml");
            TileList.LoadPalette();
            ThingList = new ThingManager();
            ThingList.LoadThingDefintions("./resources/wolfactors.xml");
        }

        private void LoadResources(MapInformation mapinfo, Level level)
        {
            for (int i = 0; i < mapinfo.files.Count; i++)
            {
                ResourceFiles.ResourceArchive file;
                //TODO: This needs to be done elsewhere
                if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.FORMAT_WAD)
                {
                    string fixfilename = mapinfo.files[i].filename;
                    //replace all slashes with backslashes to prevent issues
                    fixfilename = fixfilename.Replace('/', '\\');
                    file = ResourceFiles.WADResourceFile.loadResourceFile(mapinfo.files[i].filename);
                    file.openFile();
                    level.loadedResources.Add(file);
                    file.closeFile();
                }
                else if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.FORMAT_ZIP)
                {
                    file = ResourceFiles.ZIPResourceFile.loadResourceFile(mapinfo.files[i].filename);
                    file.openFile();
                    level.loadedResources.Add(file);
                    file.closeFile();
                }
            }
        }

        public void CreateNewLevel(MapInformation mapinfo)
        {
            LoadGameConfiguration();
            Level level = new Level(mapinfo.sizex, mapinfo.sizey, mapinfo.layers, TileList.tileset[0]);
            level.localThingList = this.ThingList;
            //this.selectedTile = tilelist.tileset[0];

            LoadResources(mapinfo, level);
            CreateBrushes();

            CurrentLevel = level;

            //this.UpdateCurrentZoneList();
            //gbTileSelection.SetPalette(tilelist.tileset);
        }

        public void CloseLevel()
        {
            //arc.closeResource();
            CurrentLevel.DisposeLevel();
            CurrentLevel = null;
        }

        /// <summary>
        /// Saves a map a specified WAD file
        /// <param name="filename">Filename to save to</param>
        /// <param name="saveinto">Makes the save code put the WAD onto the resource stack before saving</param>
        /// </summary>
        public void SaveMapToFile(string filename, bool saveinto)
        {
            //TODO: Temp
            bool destArchiveLoaded = this.CurrentMapInfo.ContainsResource(filename);

            //init it off the bat because Visual Studio is being lovely about detecting whether or not it was loaded
            ResourceFiles.WADResourceFile savearchive = new ResourceFiles.WADResourceFile();

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
                foreach (ResourceFiles.ResourceArchive file in CurrentLevel.loadedResources)
                {
                    if (file.archiveName.Equals(filename, StringComparison.OrdinalIgnoreCase))
                    {
                        savearchive = (ResourceFiles.WADResourceFile)file;
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
                    savearchive = (ResourceFiles.WADResourceFile)ResourceFiles.WADResourceFile.loadResourceFile(filename);
                    CurrentLevel.loadedResources.Add(savearchive);
                    ResourceFiles.ResourceArchiveHeader lhead = new ResourceFiles.ResourceArchiveHeader();
                    lhead.filename = filename;
                    CurrentMapInfo.files.Add(lhead);
                    destArchiveLoaded = true;
                }
            }

            //Do some special things to save the map special lumps
            if (destArchiveLoaded)
            {
                List<int> idlist = savearchive.findSpecialMapLumps(this.CurrentMapInfo.lumpname);

                //TODO: actually preserve special lumps

                //Delete the current version of the map out of the archive
                savearchive.deleteMap(CurrentMapInfo.lumpname);
            }

            string mapstring = CurrentLevel.Serialize();

            //Console.WriteLine(mapstring);

            //Get an ascii representation of the map
            byte[] mapdata = Encoding.ASCII.GetBytes(mapstring);

            //Console.WriteLine(mapdata.Length);

            List<ResourceFiles.ResourceFile> lumps = new List<ResourceFiles.ResourceFile>();

            ResourceFiles.ResourceFile mapheader = new ResourceFiles.ResourceFile(this.CurrentMapInfo.lumpname, ResourceFiles.ResourceType.RES_GENERIC, 0);
            mapheader.pointer = 0; lumps.Add(mapheader);
            ResourceFiles.ResourceFile mapdatal = new ResourceFiles.ResourceFile("TEXTMAP", ResourceFiles.ResourceType.RES_GENERIC, mapdata.Length);
            mapdatal.pointer = 0; lumps.Add(mapdatal);
            ResourceFiles.ResourceFile mapend = new ResourceFiles.ResourceFile("ENDMAP", ResourceFiles.ResourceType.RES_GENERIC, 0);
            mapend.pointer = 0; lumps.Add(mapend);


            savearchive.updateToNewWad(filename, ref lumps, ref mapdata, destArchiveLoaded);

            if (!destArchiveLoaded)
            {
                //hock the new map onto the resource list
                ResourceFiles.ResourceArchiveHeader head = new ResourceFiles.ResourceArchiveHeader();
                head.filename = filename;
                head.format = ResourceFiles.ResourceFormat.FORMAT_WAD;

                this.CurrentMapInfo.files.Add(head);
            }

            //trash all the old resources to be sure we're up to date
            CurrentLevel.DisposeLevel();
            //tm.cleanup();
            LoadResources(CurrentMapInfo, CurrentLevel);
        }

        public void SetBrush(int brushNum)
        {
            currentBrush = BrushList[brushNum];
        }

        public bool BrushDown(OpenTK.Vector2 pos, int button)
        {
            currentBrush.ApplyToTile(pos, 0, CurrentLevel, button);
            return currentBrush.repeatable;
        }

        public void BrushFromTo(OpenTK.Vector2 src, OpenTK.Vector2 dst, int button)
        {
            LineDrawer.DrawLineWithBrush(src, dst, CurrentLevel, button, currentBrush);
        }

        public void BrushEnd()
        {
            currentBrush.EndBrush(CurrentLevel);
        }
    }
}