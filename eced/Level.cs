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

using eced.GameConfig;

namespace eced
{
    public struct DirtyRectangle
    {
        public int x1, y1;
        public int x2, y2;
    }
    public class Level
    {
        //public int width = 64, height = 64, depth = 1;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }

        //public Cell[, ,] cells;

        public List<Plane> Planes { get; } = new List<Plane>();

        public List<Tile> InternalTileset { get; } = new List<Tile>();
        public List<Thing> Things { get; } = new List<Thing>();
        public List<Zone> ZoneDefs { get; } = new List<Zone>();
        public List<Sector> Sectors { get; } = new List<Sector>();
        //private Dictionary<int, Trigger> triggerList = new Dictionary<int, Trigger>();
        //private List<int> triggerKeys = new List<int>();

        //Dirty rectangle properties
        public bool Dirty { get; private set; } = false;
        public DirtyRectangle dirtyRectangle; //could be property but the structure nature makes that useless... TODO make better

        private List<NumberCell> tempPlanemap;

        public ThingManager localThingList;

        public int lastFloorCode = 0;

        public Thing highlighted = null;
        public Cell highlightedTrigger = null;
        public int[] highlightedPos = new int[2];

        public List<OpenTK.Vector2> updateCells = new List<OpenTK.Vector2>();

        public List<ResourceFiles.Archive> loadedResources = new List<ResourceFiles.Archive>();

        public Level(int w, int h, int d, Tile defaultTile)
        {
            Random r = new Random();
            this.Width = w;
            this.Height = h;
            this.Depth = d;

            //cells = new Cell[width, height, depth];

            Planes.Add(new Plane(w, h));
            Sectors.Add(new Sector());

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Planes[0].cells[x, y] = new Cell();
                    Planes[0].cells[x, y].tile = defaultTile;
                    Planes[0].cells[x, y].sector = Sectors[0];
                }
            }
            if (defaultTile != null)
                InternalTileset.Add(defaultTile);
            Console.WriteLine("tileset size: {0}", InternalTileset.Count);
            ClearDirty();
        }

        /// <summary>
        /// Closes and empties the loaded resource list
        /// </summary>
        public void DisposeLevel()
        {
            for (int i = 0; i < loadedResources.Count; i++)
            {
                loadedResources[i].CloseResource();
            }
            loadedResources.Clear();
        }

        public void ClearDirty()
        {
            dirtyRectangle.x1 = int.MaxValue;
            dirtyRectangle.y1 = int.MaxValue;
            dirtyRectangle.x2 = 0;
            dirtyRectangle.y2 = 0;
            Dirty = false;
        }

        private void SetDirty(int x, int y)
        {
            if (x < dirtyRectangle.x1) dirtyRectangle.x1 = x;
            if (y < dirtyRectangle.y1) dirtyRectangle.y1 = y;

            if (x > dirtyRectangle.x2) dirtyRectangle.x2 = x;
            if (y > dirtyRectangle.y2) dirtyRectangle.y2 = y;
            Dirty = true;
        }

        public Cell GetCell(int x, int y, int z)
        {
            return Planes[z].cells[x, y];
        }

        public int GetZoneID(int x, int y, int z)
        {
            return ZoneDefs.IndexOf(Planes[z].cells[x, y].zone);
        }

        public void SetCell(int x, int y, int z, Cell cell)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                Planes[z].cells[x, y] = cell;
                OpenTK.Vector2 triggerPos = new OpenTK.Vector2(x, y);
                if (cell.triggerList.Count > 0)
                {
                    if (!Planes[z].cellsWithTriggers.Contains(triggerPos))
                    {
                        Planes[z].cellsWithTriggers.Add(triggerPos);
                    }
                }
                else if (cell.triggerList.Count == 0 && Planes[z].cellsWithTriggers.Contains(triggerPos))
                {
                    Planes[z].cellsWithTriggers.Remove(triggerPos);
                }
            }
        }

        public void AddTile(Tile tile)
        {
            Console.WriteLine("adding tile to internal tileset");
            InternalTileset.Add(tile);
            Console.WriteLine("tileset size: {0}", InternalTileset.Count);
        }

        public Tile GetTile(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
                return Planes[z].cells[x, y].tile;

            return null; //need to find a better return value
        }

        public void SetTile(int x, int y, int z, Tile tile)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                if (tile != Planes[z].cells[x, y].tile)
                {
                    //cells[x, y, z].tile = tile;
                    //cells[x, y, z].zone = null;
                    Planes[z].cells[x, y].tile = tile;
                    Planes[z].cells[x, y].zone = null;
                    if (!InternalTileset.Contains(tile) && tile != null)
                    {
                        Console.WriteLine("adding tile to internal tileset");
                        InternalTileset.Add(tile);
                        Console.WriteLine("tileset size: {0}", InternalTileset.Count);
                    }
                    //updateCells.Add(new OpenTK.Vector2(x, y));
                    SetDirty(x, y);
                }
            }
        }

        public void SetTag(int x, int y, int z, int tag)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                Planes[z].cells[x, y].tag = tag;
            }
        }

        public void AddSector(Sector sector)
        {
            Sectors.Add(sector);
        }

        public void SetSector(int x, int y, int z, Sector sector)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                Planes[z].cells[x, y].sector = sector;

                if (!Sectors.Contains(sector))
                    Sectors.Add(sector);
            }
        }

        public void AddThing(Thing thing)
        {
            this.Things.Add(thing);
        }

        public List<Thing> GetThings()
        {
            return Things;
        }

        public List<Thing> GetThingsInRange(int sx, int sy, int w, int h)
        {
            List<Thing> thinglist = new List<Thing>();
            int ex = sx + w;
            int ey = sy + h;
            for (int x = 0; x < this.Things.Count; x++)
            {
                Thing thing = Things[x];
                if (thing.x >= sx && thing.x < ex && thing.y >= sy && thing.y < ey)
                {
                    thinglist.Add(thing);
                }
            }
            //Console.WriteLine("{0} things added", thinglist.Count);
            return thinglist;
        }

        public ThingDefinition GetThingDef(Thing thing)
        {
            if (!localThingList.idToThingListMapping.ContainsKey(thing.typeid))
            {
                return localThingList.GetUnknownThing();
            }
            return localThingList.thingList[localThingList.idToThingListMapping[thing.typeid]];
        }

        public void HighlightThing(int x, int y)
        {
            if (this.highlighted != null)
                return;

            for (int lx = 0; lx < this.Things.Count; lx++)
            {
                Thing thing = Things[lx];
                ThingDefinition def = GetThingDef(thing);
                int sx = (int)thing.getXCoord() - def.radius; int ex = (int)thing.getXCoord() + def.radius;
                int sy = (int)thing.getYCoord() - def.radius; int ey = (int)thing.getYCoord() + def.radius;
                if (x >= sx && x < ex && y >= sy && y < ey)
                {
                    this.highlighted = thing;
                    thing.highlighted = true;
                    Console.WriteLine("Highlighted");
                    return;
                }
            }
        }

        public void UpdateHighlight(int x, int y)
        {
            if (this.highlighted == null)
            {
                HighlightThing(x, y);
                return;
            }

            ThingDefinition def = GetThingDef(highlighted);
            int sx = (int)highlighted.getXCoord() - def.radius; int ex = (int)highlighted.getXCoord() + def.radius;
            int sy = (int)highlighted.getYCoord() - def.radius; int ey = (int)highlighted.getYCoord() + def.radius;
            if ((x < sx || x >= ex || y < sy || y >= ey) && 
                !this.highlighted.moving)
            {
                Console.WriteLine("unhighlighting");
                this.highlighted.highlighted = false;
                this.highlighted = null;
            }
        }

        public void DeleteThing(Thing thing)
        {
            if (this.Things.Contains(thing))
            {
                int lx = (int)thing.x;
                int ly = (int)thing.y;

                this.Things.Remove(thing);
            }
        }

        public void ReplaceThing(Thing thing, Thing newThing)
        {
            if (this.Things.Contains(thing))
            {
                int lx = (int)newThing.x;
                int ly = (int)newThing.y;

                this.Things[this.Things.IndexOf(thing)] = newThing;
            }
        }

        public void AddTrigger(int x, int y, int z, Trigger trigger)
        {
            Planes[z].cells[x, y].triggerList.Add(trigger);
            OpenTK.Vector2 triggerPos = new OpenTK.Vector2(x, y);
            if (!Planes[z].cellsWithTriggers.Contains(triggerPos))
            {
                Planes[z].cellsWithTriggers.Add(triggerPos);
            }
        }

        public List<Trigger> GetTriggers(int x, int y, int z)
        {
            return Planes[z].cells[x, y].triggerList;
        }

        public List<OpenTK.Vector2> GetTriggerLocations()
        {
            return Planes[0].cellsWithTriggers;
        }

        public void HighlightTrigger(int x, int y, int z)
        {
            List<Trigger> trigger = GetTriggers(x, y, z);
            if (trigger.Count > 0)
            {
                Console.WriteLine("highlighting!");
                Planes[z].cells[x, y].highlighted = true;
                this.highlightedTrigger = Planes[z].cells[x, y];
                this.highlightedPos[0] = x;
                this.highlightedPos[1] = y;
            }
        }

        public void UpdateTriggerHighlight(int x, int y, int z)
        {
            if (x < 0 || y < 0)
                return;

            if (this.highlightedTrigger == null)
            {
                HighlightTrigger(x, y, z);
                return;
            }
            else
            {
                if (x != this.highlightedPos[0] || y != this.highlightedPos[1])
                {
                    this.highlightedTrigger.highlighted = false;
                    this.highlightedTrigger = null;
                    return;
                }
            }
        }

        public void AssignFloorCode(int x, int y, int z, int code)
        {
            Console.WriteLine("setting code {0}", code);
            if (code == ZoneDefs.Count)
            {
                ZoneDefs.Add(new Zone());
                Console.WriteLine("adding a zone");
            }

            Planes[z].cells[x, y].zone = ZoneDefs[code];
            updateCells.Add(new OpenTK.Vector2(x, y));
        }

        public int GetUniqueCode()
        {
            Console.WriteLine("getting code {0}", ZoneDefs.Count);
            return ZoneDefs.Count;
        }

        /// <summary>
        /// Gets a string representing the map in UWMF format
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            StringBuilder sb = new StringBuilder(); 
			
            sb.Append("namespace = \"Wolf3D\";\n");
            sb.Append("tilesize = 64;\n");
            sb.Append("name = \"ecedtest\";\n");
            sb.Append("width = " + Width.ToString() + ";\n");
            sb.Append("height = " + Height.ToString() + ";\n");

            //Plane plane = new Plane();

            for (int x = 0; x < InternalTileset.Count; x++)
            {
                sb.Append(InternalTileset[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");
            for (int x = 0; x < ZoneDefs.Count; x++)
            {
                sb.Append(ZoneDefs[x].getUWMFString());
                sb.Append("\n");
            }
            sb.Append("\n");
            for (int x = 0; x < Things.Count; x++)
            {
                sb.Append(Things[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < Sectors.Count; x++)
            {
                sb.Append(Sectors[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < Planes.Count; x++)
            {
                sb.Append(Planes[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < Planes.Count; x++)
            {
                sb.Append(SerializePlaneMap(x));
            }
            sb.Append("\n");

            for (int i = 0; i < Width * Height; i++)
            {
                for (int p = 0; p < this.Depth; p++)
                {
                    int x = i % Width;
                    int y = i / Width;
                    for (int li = 0; li < Planes[p].cells[x, y].triggerList.Count; li++)
                    {
                        sb.Append(Planes[p].cells[x, y].triggerList[li].Serialize());
                        sb.Append("\n");
                    }
                }
            }
            sb.Append("\n");
			return sb.ToString();
        }

        public string SerializePlaneMap(int plane)
        {
            StringBuilder stringbuilder = new StringBuilder();
            stringbuilder.Append("planemap\n");
            stringbuilder.Append("{\n");
            for (int i = 0; i < Width * Height; i++)
            {
                int x = i % Width;
                int y = i / Width;
                stringbuilder.Append("\t{"); stringbuilder.Append(GetTileID(Planes[plane].cells[x, y].tile));
                stringbuilder.Append(", "); stringbuilder.Append(GetSectorID(x, y, plane));
                stringbuilder.Append(", "); stringbuilder.Append(GetZoneID(Planes[plane].cells[x, y]));
                stringbuilder.Append(", "); stringbuilder.Append(Planes[plane].cells[x, y].tag);
                stringbuilder.Append("}");
                if (i < ((Width * Height) - 1))
                {
                    stringbuilder.Append(",");
                }
                stringbuilder.Append("\n");
            }
            stringbuilder.Append("}");

            return stringbuilder.ToString();
        }

        public int GetTileID(Tile tile)
        {
            if (InternalTileset.Contains(tile))
                return InternalTileset.IndexOf(tile);

            return -1;
        }

        public int GetSectorID(int x, int y, int z)
        {
            if (Sectors.Contains(Planes[z].cells[x, y].sector))
                return Sectors.IndexOf(Planes[z].cells[x, y].sector);

            return -1;
        }

        public int GetZoneID(Cell cell)
        {
            if (cell.zone == null)
            {
                return -1;
            }
            return ZoneDefs.IndexOf(cell.zone);
        }

        public bool CompareZoneID(int x, int y, int z, int code)
        {
            return GetZoneID(Planes[z].cells[x, y]) == code;
        }

        public Zone GetZoneAt(int x, int y, int z)
        {
            return Planes[z].cells[x, y].zone;
        }

        public int GetZoneIDAt(int x, int y, int z)
        {
            if (ZoneDefs.Contains(Planes[z].cells[x, y].zone))
            {
                return ZoneDefs.IndexOf(Planes[z].cells[x, y].zone);
            }
            return -1;
        }

        public void SetTempPlaneMap(List<NumberCell> planemap)
        {
            this.tempPlanemap = planemap;
        }

        public void AddZone(Zone zone)
        {
            this.ZoneDefs.Add(zone);
        }

        public List<Zone> GetZones()
        {
            return this.ZoneDefs;
        }

        public bool IsCellHighlighted(int x, int y, int z)
        {
            return Planes[z].cells[x, y].highlighted;
        }

        public void ProcessPlanemap()
        {
            if (this.tempPlanemap == null)
                return;

            int index = 0;
            int tilesadded = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Depth; z++)
                    {
                        Cell cell = new Cell();
                        if (tempPlanemap[index].tile >= 0)
                        {
                            cell.tile = this.InternalTileset[tempPlanemap[index].tile];
                            tilesadded++;
                        }
                        cell.sector = new Sector(); //no sector management
                        if (tempPlanemap[index].zone >= 0)
                            cell.zone = this.ZoneDefs[tempPlanemap[index].zone];

                        Planes[z].cells[x, y] = cell;
                    }

                    index++;
                }
            }
            this.tempPlanemap = null;
            Console.WriteLine("added {0} tiles", tilesadded);
        }
    }
}
