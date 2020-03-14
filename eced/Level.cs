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

namespace eced
{
    public class Level
    {
        public int width = 64, height = 64, depth = 1;

        //public Cell[, ,] cells;

        private List<Plane> planes = new List<Plane>();

        private List<Tile> internalTileset = new List<Tile>();
        private List<Thing> things = new List<Thing>();
        private List<Zone> zonedefs = new List<Zone>();
        private List<Sector> sectors = new List<Sector>();
        //private Dictionary<int, Trigger> triggerList = new Dictionary<int, Trigger>();
        //private List<int> triggerKeys = new List<int>();

        private List<NumberCell> tempPlanemap;

        public ThingManager localThingList;

        public int lastFloorCode = 0;

        public Thing highlighted = null;
        public Cell highlightedTrigger = null;
        public int[] highlightedPos = new int[2];

        public List<OpenTK.Vector2> updateCells = new List<OpenTK.Vector2>();

        public List<ResourceFiles.ResourceArchive> loadedResources = new List<ResourceFiles.ResourceArchive>();

        public TextureManager tm;

        public Level(int w, int h, int d, Tile defaultTile)
        {
            Random r = new Random();
            this.width = w;
            this.height = h;
            this.depth = d;

            //cells = new Cell[width, height, depth];

            planes.Add(new Plane(w, h));
            sectors.Add(new Sector());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    planes[0].cells[x, y] = new Cell();
                    planes[0].cells[x, y].tile = defaultTile;
                    planes[0].cells[x, y].sector = sectors[0];
                }
            }
            if (defaultTile != null)
                internalTileset.Add(defaultTile);
            Console.WriteLine("tileset size: {0}", internalTileset.Count);
        }

        /// <summary>
        /// Closes and empties the loaded resource list
        /// </summary>
        public void DisposeLevel()
        {
            for (int i = 0; i < loadedResources.Count; i++)
            {
                loadedResources[i].closeResource();
            }
            loadedResources.Clear();
        }

        public Cell GetCell(int x, int y, int z)
        {
            return planes[z].cells[x, y];
        }

        public int GetZoneID(int x, int y, int z)
        {
            return zonedefs.IndexOf(planes[z].cells[x, y].zone);
        }

        public void SetCell(int x, int y, int z, Cell cell)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                planes[z].cells[x, y] = cell;
                OpenTK.Vector2 triggerPos = new OpenTK.Vector2(x, y);
                if (cell.triggerList.Count > 0)
                {
                    if (!planes[z].cellsWithTriggers.Contains(triggerPos))
                    {
                        planes[z].cellsWithTriggers.Add(triggerPos);
                    }
                }
                else if (cell.triggerList.Count == 0 && planes[z].cellsWithTriggers.Contains(triggerPos))
                {
                    planes[z].cellsWithTriggers.Remove(triggerPos);
                }
            }
        }

        public void AddTile(Tile tile)
        {
            Console.WriteLine("adding tile to internal tileset");
            internalTileset.Add(tile);
            Console.WriteLine("tileset size: {0}", internalTileset.Count);
        }

        public Tile GetTile(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return planes[z].cells[x, y].tile;

            return TileManager.tile1;
        }

        public void SetTile(int x, int y, int z, Tile tile)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                if (tile != planes[z].cells[x, y].tile)
                {
                    //cells[x, y, z].tile = tile;
                    //cells[x, y, z].zone = null;
                    planes[z].cells[x, y].tile = tile;
                    planes[z].cells[x, y].zone = null;
                    if (!internalTileset.Contains(tile) && tile != null)
                    {
                        Console.WriteLine("adding tile to internal tileset");
                        internalTileset.Add(tile);
                        Console.WriteLine("tileset size: {0}", internalTileset.Count);
                    }
                    updateCells.Add(new OpenTK.Vector2(x, y));
                }
            }
        }

        public void SetTag(int x, int y, int z, int tag)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                planes[z].cells[x, y].tag = tag;
            }
        }

        public void AddSector(Sector sector)
        {
            sectors.Add(sector);
        }

        public void SetSector(int x, int y, int z, Sector sector)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                planes[z].cells[x, y].sector = sector;

                if (!sectors.Contains(sector))
                    sectors.Add(sector);
            }
        }

        public void AddThing(Thing thing)
        {
            this.things.Add(thing);
        }

        public List<Thing> GetThings()
        {
            return things;
        }

        public List<Thing> GetThingsInRange(int sx, int sy, int w, int h)
        {
            List<Thing> thinglist = new List<Thing>();
            int ex = sx + w;
            int ey = sy + h;
            for (int x = 0; x < this.things.Count; x++)
            {
                Thing thing = things[x];
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

            for (int lx = 0; lx < this.things.Count; lx++)
            {
                Thing thing = things[lx];
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
            if (this.things.Contains(thing))
            {
                int lx = (int)thing.x;
                int ly = (int)thing.y;

                this.things.Remove(thing);
            }
        }

        public void ReplaceThing(Thing thing, Thing newThing)
        {
            if (this.things.Contains(thing))
            {
                int lx = (int)newThing.x;
                int ly = (int)newThing.y;

                this.things[this.things.IndexOf(thing)] = newThing;
            }
        }

        public void AddTrigger(int x, int y, int z, Trigger trigger)
        {
            planes[z].cells[x, y].triggerList.Add(trigger);
            OpenTK.Vector2 triggerPos = new OpenTK.Vector2(x, y);
            if (!planes[z].cellsWithTriggers.Contains(triggerPos))
            {
                planes[z].cellsWithTriggers.Add(triggerPos);
            }
        }

        public List<Trigger> GetTriggers(int x, int y, int z)
        {
            return planes[z].cells[x, y].triggerList;
        }

        public List<OpenTK.Vector2> GetTriggerLocations()
        {
            return planes[0].cellsWithTriggers;
        }

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public short[] BuildPlaneData(int layer)
        {
            short[] planeData = new short[this.width * this.height * 4];

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    //planeData[(x * width + y) * 4] = (short)planes[layer].cells[x, y].tile.id;
                    if (planes[layer].cells[x, y].tile != null)
                        planeData[(y * width + x) * 4] = (short)tm.getTextureID(planes[layer].cells[x, y].tile.NorthTex);
                    else planeData[(y * width + x) * 4] = -1;

                    if (planes[layer].cells[x, y].tile == null)
                        planeData[(y * width + x) * 4 + 1] = (short)zonedefs.IndexOf(planes[layer].cells[x, y].zone);
                }
            }

            return planeData;
        }

        public void HighlightTrigger(int x, int y, int z)
        {
            List<Trigger> trigger = GetTriggers(x, y, z);
            if (trigger.Count > 0)
            {
                Console.WriteLine("highlighting!");
                planes[z].cells[x, y].highlighted = true;
                this.highlightedTrigger = planes[z].cells[x, y];
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
            if (code == zonedefs.Count)
            {
                zonedefs.Add(new Zone());
                Console.WriteLine("adding a zone");
            }

            planes[z].cells[x, y].zone = zonedefs[code];
            updateCells.Add(new OpenTK.Vector2(x, y));
        }

        public int GetUniqueCode()
        {
            Console.WriteLine("getting code {0}", zonedefs.Count);
            return zonedefs.Count;
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
            sb.Append("width = " + width.ToString() + ";\n");
            sb.Append("height = " + height.ToString() + ";\n");

            //Plane plane = new Plane();

            for (int x = 0; x < internalTileset.Count; x++)
            {
                sb.Append(internalTileset[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");
            for (int x = 0; x < zonedefs.Count; x++)
            {
                sb.Append(zonedefs[x].getUWMFString());
                sb.Append("\n");
            }
            sb.Append("\n");
            for (int x = 0; x < things.Count; x++)
            {
                sb.Append(things[x].getUWMFString());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < sectors.Count; x++)
            {
                sb.Append(sectors[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < planes.Count; x++)
            {
                sb.Append(planes[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < planes.Count; x++)
            {
                sb.Append(SerializePlaneMap(x));
            }
            sb.Append("\n");

            for (int i = 0; i < width * height; i++)
            {
                for (int p = 0; p < this.depth; p++)
                {
                    int x = i % width;
                    int y = i / width;
                    for (int li = 0; li < planes[p].cells[x, y].triggerList.Count; li++)
                    {
                        sb.Append(planes[p].cells[x, y].triggerList[li].Serialize());
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
            for (int i = 0; i < width * height; i++)
            {
                int x = i % width;
                int y = i / width;
                stringbuilder.Append("\t{"); stringbuilder.Append(GetTileID(planes[plane].cells[x, y].tile));
                stringbuilder.Append(", "); stringbuilder.Append(GetSectorID(x, y, plane));
                stringbuilder.Append(", "); stringbuilder.Append(GetZoneID(planes[plane].cells[x, y]));
                stringbuilder.Append(", "); stringbuilder.Append(planes[plane].cells[x, y].tag);
                stringbuilder.Append("}");
                if (i < ((width * height) - 1))
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
            if (internalTileset.Contains(tile))
                return internalTileset.IndexOf(tile);

            return -1;
        }

        public int GetSectorID(int x, int y, int z)
        {
            if (sectors.Contains(planes[z].cells[x, y].sector))
                return sectors.IndexOf(planes[z].cells[x, y].sector);

            return -1;
        }

        public int GetZoneID(Cell cell)
        {
            if (cell.zone == null)
            {
                return -1;
            }
            return zonedefs.IndexOf(cell.zone);
        }

        public bool CompareZoneID(int x, int y, int z, int code)
        {
            return GetZoneID(planes[z].cells[x, y]) == code;
        }

        public Zone GetZoneAt(int x, int y, int z)
        {
            return planes[z].cells[x, y].zone;
        }

        public int GetZoneIDAt(int x, int y, int z)
        {
            if (zonedefs.Contains(planes[z].cells[x, y].zone))
            {
                return zonedefs.IndexOf(planes[z].cells[x, y].zone);
            }
            return -1;
        }

        public void SetTempPlaneMap(List<NumberCell> planemap)
        {
            this.tempPlanemap = planemap;
        }

        public void AddZone(Zone zone)
        {
            this.zonedefs.Add(zone);
        }

        public List<Zone> GetZone()
        {
            return this.zonedefs;
        }

        public bool IsCellHighlighted(int x, int y, int z)
        {
            return planes[z].cells[x, y].highlighted;
        }

        public void ProcessPlanemap()
        {
            if (this.tempPlanemap == null)
                return;

            int index = 0;
            int tilesadded = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Cell cell = new Cell();
                        if (tempPlanemap[index].tile >= 0)
                        {
                            cell.tile = this.internalTileset[tempPlanemap[index].tile];
                            tilesadded++;
                        }
                        cell.sector = new Sector(); //no sector management
                        if (tempPlanemap[index].zone >= 0)
                            cell.zone = this.zonedefs[tempPlanemap[index].zone];

                        planes[z].cells[x, y] = cell;
                    }

                    index++;
                }
            }
            this.tempPlanemap = null;
            Console.WriteLine("added {0} tiles", tilesadded);
        }
    }
}
