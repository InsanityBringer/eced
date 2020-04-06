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
using System.Text;
using System.IO;

using eced.Brushes;
using eced.GameConfig;

namespace eced
{
    public enum CurrentToolNum
    {
        Unknown,
        RoomTool,
        TileTool,
        TextureTool,
        ThingTool,
        TriggerTool,
        SectorTool,
        ZoneTool,
        TagTool
    }
    public class EditorState
    {
        public ColorChart Colors { get; } = new ColorChart();
        private EditorBrush currentBrush;
        public CurrentToolNum CurrentTool { get; private set; }
        public EditorBrush[] BrushList { get; } = new EditorBrush[9];

        public PickResult LastOrthoHit { get; private set; }

        //fields for current primitive types
        public Tile currentTile;
        public ThingDefinition currentThingDef;

        //properties for the current editor state
        public MapInformation CurrentMapInfo { get; private set; }
        public Level CurrentLevel { get; private set; }
        public TileManager TileList { get; private set; }
        public ThingManager ThingList { get; private set; }
        public TriggerManager TriggerList { get; private set; }
        public VSwapNames VSwapNameList { get; private set; }
        public Thing HighlightedThing { get; private set; }
        public List<Thing> SelectedThings { get; } = new List<Thing>();
        public int HighlightedZone { get; private set; } = -1;

        public EditorIO EditorIOState { get; }
        private EditorInputHandler inputHandler;

        //This file is getting way too big aaaaa
        private bool facingMode = false;
        public bool IsThingMode
        {
            get
            {
                //todo: make better
                return currentBrush == BrushList[4];
            }
        }

        public EditorState()
        {
            EditorIOState = new EditorIO(this);
            inputHandler = new EditorInputHandler(this);
            Colors.Init();
        }

        private void CreateBrushes()
        {
            BrushList[0] = new EditorBrush(this);
            BrushList[1] = new RoomBrush(this);
            BrushList[2] = new TileBrush(this);
            BrushList[3] = new EditorBrush(this);
            BrushList[4] = new ThingBrush(this); ((ThingBrush)BrushList[4]).thinglist = ThingList; ((ThingBrush)BrushList[4]).thing = ThingList.thingList[0];
            BrushList[5] = new TriggerBrush(this);
            BrushList[6] = new SectorBrush(this);
            BrushList[7] = new FloodBrush(this);
            BrushList[8] = new TagTool(this);
        }

        private void LoadGameConfiguration()
        {
            //TODO: Actual game configuration elements
            TileList = new TileManager("./Resources/wolftiles.xml");
            TileList.LoadPalette();
            ThingList = new ThingManager();
            ThingList.LoadThingDefintions("./Resources/wolfactors.xml");
            TriggerList = new TriggerManager();
            TriggerList.LoadTriggerDefinitions("./Resources/wolftriggers.xml");
            VSwapNameList = new VSwapNames();
            VSwapNameList.LoadVSwapNames("./Resources/wolfvswap.xml");
            byte[] defaultPalette = new byte[768];
            Stream str = File.Open("./Resources/wolfpalette.pal", FileMode.Open);
            str.Read(defaultPalette, 0, 768);
            str.Close(); str.Dispose();
            CurrentMapInfo.SetPalette(defaultPalette);
        }

        private void LoadResources(MapInformation mapinfo, Level level)
        {
            for (int i = 0; i < mapinfo.files.Count; i++)
            {
                ResourceFiles.Archive file;
                //TODO: This needs to be done elsewhere
                if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.Wad)
                {
                    string fixfilename = mapinfo.files[i].filename;
                    //replace all slashes with backslashes to prevent issues
                    fixfilename = fixfilename.Replace('/', '\\');
                    file = ResourceFiles.WADArchive.loadResourceFile(mapinfo.files[i].filename);
                    file.OpenFile();
                    level.loadedResources.Add(file);
                    file.CloseFile();
                }
                else if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.Zip)
                {
                    file = ResourceFiles.ZIPArchive.loadResourceFile(mapinfo.files[i].filename);
                    file.OpenFile();
                    level.loadedResources.Add(file);
                    file.CloseFile();
                }
                else if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.VSwap)
                {
                    file = ResourceFiles.VSwapArchive.OpenArchive(mapinfo.files[i].filename, VSwapNameList);
                    level.loadedResources.Add(file);
                    file.CloseFile();
                }
            }
        }

        public void CreateNewLevel(MapInformation mapinfo)
        {
            CurrentMapInfo = mapinfo;
            LoadGameConfiguration();
            EditorIOState.InitNewLevel();
            Level level = new Level(mapinfo.sizex, mapinfo.sizey, mapinfo.layers, TileList.tileset[0]);
            level.localThingList = this.ThingList;

            LoadResources(mapinfo, level);
            CreateBrushes();

            CurrentLevel = level;
        }

        public bool CreateLevelFromData(MapInformation mapinfo, byte[] data)
        {
            Level level;
            if (EditorIOState.InitFromLump(mapinfo.files[0].filename, data, out level))
            {
                //we're good, so lets kick off this mess
                CurrentMapInfo = mapinfo;
                LoadGameConfiguration();
                CurrentLevel = level;
                level.localThingList = this.ThingList;

                LoadResources(mapinfo, level);
                CreateBrushes();
            }
            else
            {
                if (EditorIOState.LastErrorLine != -1)
                {
                    //TODO: Real error handling
                    Console.WriteLine("Error on line {0}: {1}", EditorIOState.LastErrorLine, EditorIOState.LastError);
                }
                return false;
            }
            return true;
        }
        
        public void CloseLevel()
        {
            //arc.closeResource();
            CurrentLevel.DisposeLevel();
            CurrentLevel = null;
            SelectedThings.Clear();
        }

        /// <summary>
        /// Saves a map a specified WAD file
        /// <param name="filename">Filename to save to</param>
        /// <param name="saveinto">True if the map should be written into the wad if it already exists, or false if it should create a new wad</param>
        /// </summary>
        public bool SaveMapToFile(string filename, bool saveinto)
        {
            bool res = EditorIOState.SaveMapToFile(filename, saveinto);
            if (res)
            {
                //trash all the old resources to be sure we're up to date
                CurrentLevel.DisposeLevel();
                LoadResources(CurrentMapInfo, CurrentLevel);
            }
            return res;
        }

        public void UpdateHighlight(PickResult res)
        {
            if (HighlightedThing != null)
            {
                HighlightedThing.highlighted = false;
                HighlightedThing = null;
            }
            if (!IsThingMode) return;
            Thing thing = CurrentLevel.HighlightThing(res);
            if (thing != null)
            {
                thing.highlighted = true;
                HighlightedThing = thing;
            }
        }

        public void ToggleSelectedThing(Thing thing)
        {
            if (facingMode) return;
            if (thing.selected)
            {
                thing.selected = false;
                SelectedThings.Remove(thing); //TODO: optimize?
            }
            else
            {
                thing.selected = true;
                SelectedThings.Add(thing);
            }
        }

        public void ClearSelectedThings()
        {
            if (facingMode) return;
            foreach (Thing thing in SelectedThings)
                thing.selected = false;
            SelectedThings.Clear();
        }

        public void DeleteSelectedThings()
        {
            if (facingMode) return;
            foreach (Thing thing in SelectedThings)
                CurrentLevel.DeleteThing(thing);
            ClearSelectedThings();
        }

        public void StartFacing()
        {
            facingMode = true;
        }

        public void EndFacing()
        {
            facingMode = false;
        }

        public void SetBrush(int brushNum)
        {
            EndFacing(); //prevent issues with facing remaining
            CurrentTool = (CurrentToolNum)brushNum;
            currentBrush = BrushList[brushNum];
        }

        public bool BrushDown(PickResult pos, int button)
        {
            currentBrush.StartBrush(pos, CurrentLevel, button);
            return currentBrush.Repeatable;
        }

        public void BrushFromTo(PickResult src, PickResult dst, int button)
        {
            if (currentBrush.Interpolated)
                LineDrawer.DrawLineWithBrush(src, dst, CurrentLevel, button, currentBrush);
            else
                currentBrush.ApplyToTile(dst, CurrentLevel, button);
        }

        public void BrushEnd()
        {
            currentBrush.EndBrush(CurrentLevel);
        }

        public void HandlePick(PickResult res)
        {
            UpdateHighlight(res);
            LastOrthoHit = res;
            if (CurrentTool == CurrentToolNum.ZoneTool)
            {
                HighlightedZone = CurrentLevel.GetZoneID(res.x, res.y, res.z);
            }
            else if (CurrentTool == CurrentToolNum.ThingTool)
            {
                if (facingMode)
                    DoFacing();
            }
        }

        private void DoFacing()
        {
            float sx, sy, dx, dy;
            float newang;
            sx = LastOrthoHit.x + LastOrthoHit.xf;
            sy = LastOrthoHit.y + LastOrthoHit.yf;
            foreach (Thing thing in SelectedThings)
            {
                dx = sx - thing.x;
                dy = sy - thing.y;
                //ah, fun math. Get a "normalized" angle
                newang = (float)(Math.Atan2(-dy, dx) / (2 * Math.PI));
                newang *= 8; //8 cardinal dirs
                newang = (float)Math.Round(newang, MidpointRounding.AwayFromZero); //Round to nearest 45th
                newang *= 45f;
                thing.angle = (int)newang;
                if (thing.angle < 0) thing.angle = 360 + thing.angle;
            }
        }

        public bool HandleInputEvent(InputEvent ev)
        {
            return inputHandler.HandleInputEvent(ev);
        }
    }
}
