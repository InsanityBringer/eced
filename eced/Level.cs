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
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }
        public int TileSize { get; private set; } = 64;
        public List<Plane> Planes { get; } = new List<Plane>();

        //Data management
        public List<Tile> Tileset { get; } = new List<Tile>();
        public Dictionary<Tile, int> TilesetMap { get; } = new Dictionary<Tile, int>();
        public List<Sector> Sectorset { get; } = new List<Sector>();
        public Dictionary<Sector, int> SectorsetMap { get; } = new Dictionary<Sector, int>();
        public List<TriggerList> Triggers { get; } = new List<TriggerList>();
        public Dictionary<TilePosition, int> TriggersMap { get; } = new Dictionary<TilePosition, int>();
        public List<Thing> Things { get; } = new List<Thing>();
        public List<Zone> ZoneDefs { get; } = new List<Zone>();

        //Dirty rectangle properties
        public bool Dirty { get; private set; } = false;
        public DirtyRectangle dirtyRectangle; //could be property but the structure nature makes that useless... TODO make better

        private List<NumberCell> tempPlanemap;

        public ThingManager localThingList;

        public int lastFloorCode = 0;

        public Cell highlightedTrigger = null;
        public int[] highlightedPos = new int[2];

        public List<ResourceFiles.Archive> loadedResources = new List<ResourceFiles.Archive>();

        public Level(int w, int h, int d, Tile defaultTile)
        {
            Random r = new Random();
            this.Width = w;
            this.Height = h;
            this.Depth = d;

            Planes.Add(new Plane(w, h));
            Sectorset.Add(new Sector());

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Planes[0].cells[x, y] = new Cell();
                    Planes[0].cells[x, y].tile = 0;
                    Planes[0].cells[x, y].sector = 0;
                    Planes[0].cells[x, y].zone = -1;
                }
            }
            if (defaultTile != null)
            {
                AddTile(defaultTile);
            }
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

        public void Invalidate()
        {
            dirtyRectangle.x1 = 0; dirtyRectangle.y1 = 0;
            dirtyRectangle.x2 = Width-1; dirtyRectangle.y2 = Height-1;
            Dirty = true;
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
            return Planes[z].cells[x, y].zone;
        }

        public void AddTile(Tile tile)
        {
            Console.WriteLine("adding tile to internal tileset");
            TilesetMap.Add(tile, Tileset.Count);
            Tileset.Add(tile);
            Console.WriteLine("tileset size: {0}", Tileset.Count);
        }

        public Tile GetTile(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                if (Planes[z].cells[x, y].tile == -1) return null;

                return Tileset[Planes[z].cells[x, y].tile];
            }

            return null; //need to find a better return value
        }

        public void SetTile(int x, int y, int z, Tile tile)
        {
            int tileid;
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                if (tile == null)
                {
                    Planes[z].cells[x, y].tile = -1;
                }
                else
                {
                    if (!TilesetMap.ContainsKey(tile))
                    {
                        AddTile(tile);
                    }
                    tileid = TilesetMap[tile];
                    if (tileid != Planes[z].cells[x, y].tile)
                    {
                        Planes[z].cells[x, y].tile = tileid;
                        Planes[z].cells[x, y].zone = -1;
                    }
                }
                SetDirty(x, y);
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
            SectorsetMap.Add(sector, Sectorset.Count);
            Sectorset.Add(sector);
        }

        public Sector GetSector(int x, int y, int z)
        {
            return Sectorset[Planes[z].cells[x, y].sector];
        }

        public void SetSector(int x, int y, int z, Sector sector)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                if (!SectorsetMap.ContainsKey(sector))
                    AddSector(sector);

                Planes[z].cells[x, y].sector = SectorsetMap[sector];
                SetDirty(x, y);
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
            return localThingList.GetThingDef(thing.type);
        }

        public Thing HighlightThing(PickResult res)
        {
            float pickX = (res.x + res.xf) * TileSize;
            float pickY = (res.y + res.yf) * TileSize;
            //TODO: spatial partitioning
            for (int lx = 0; lx < this.Things.Count; lx++)
            {
                Thing thing = Things[lx];
                ThingDefinition def = GetThingDef(thing);
                float sx = thing.x * TileSize - def.Radius; float ex = thing.x * TileSize + def.Radius;
                float sy = thing.y * TileSize - def.Radius; float ey = thing.y * TileSize + def.Radius;
                if (pickX >= sx && pickX < ex && pickY >= sy && pickY < ey)
                {
                    return thing;
                }
            }
            return null;
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
            TilePosition pos; pos.x = x; pos.y = y; pos.z = z;
            //If there's already a trigger at this position, simply add to the stack
            if (TriggersMap.ContainsKey(pos))
            {
                Triggers[TriggersMap[pos]].Triggers.Add(trigger);
            }
            //Otherwise, create a new trigger stack at that position
            else
            {
                TriggerList list = new TriggerList();
                list.pos.x = x; list.pos.y = y; list.pos.z = z;
                list.Triggers.Add(trigger);
                Triggers.Add(list);
                TriggersMap.Add(pos, Triggers.Count - 1);
            }
        }

        public TriggerList GetTriggers(int x, int y, int z)
        {
            TilePosition pos; pos.x = x; pos.y = y; pos.z = z;
            if (!TriggersMap.ContainsKey(pos)) return null;
            return Triggers[TriggersMap[pos]];
        }

        public void HighlightTrigger(int x, int y, int z)
        {
            //TODO
            /*List<Trigger> trigger = GetTriggers(x, y, z);
            if (trigger.Count > 0)
            {
                Console.WriteLine("highlighting!");
                Planes[z].cells[x, y].highlighted = true;
                this.highlightedTrigger = Planes[z].cells[x, y];
                this.highlightedPos[0] = x;
                this.highlightedPos[1] = y;
            }*/
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

        public void SetZone(int x, int y, int z, int code)
        {
            while (code >= ZoneDefs.Count) //heeh
            {
                ZoneDefs.Add(new Zone());
            }

            Planes[z].cells[x, y].zone = code;
            SetDirty(x, y);
        }

        public int GetUniqueZone()
        {
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

            for (int x = 0; x < Tileset.Count; x++)
            {
                sb.Append(Tileset[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");
            for (int x = 0; x < ZoneDefs.Count; x++)
            {
                sb.Append(ZoneDefs[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");
            for (int x = 0; x < Things.Count; x++)
            {
                sb.Append(Things[x].Serialize());
                sb.Append("\n");
            }
            sb.Append("\n");

            for (int x = 0; x < Sectorset.Count; x++)
            {
                sb.Append(Sectorset[x].Serialize());
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

            for (int i = 0; i < Triggers.Count; i++)
            {
                foreach (Trigger trigger in Triggers[i].Triggers)
                    sb.Append(trigger.Serialize());
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
                stringbuilder.Append("\t{"); stringbuilder.Append(Planes[plane].cells[x, y].tile);
                stringbuilder.Append(", "); stringbuilder.Append(Planes[plane].cells[x, y].sector);
                stringbuilder.Append(", "); stringbuilder.Append(Planes[plane].cells[x, y].zone);
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
            if (Tileset.Contains(tile))
                return Tileset.IndexOf(tile);

            return -1;
        }

        public int GetSectorID(int x, int y, int z)
        {
            return Planes[z].cells[x, y].sector;
        }

        public bool CompareZoneID(int x, int y, int z, int code)
        {
            return GetZoneID(x, y, z) == code;
        }

        public Zone GetZoneAt(int x, int y, int z)
        {
            return ZoneDefs[Planes[z].cells[x, y].zone];
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
                            cell.tile = tempPlanemap[index].tile;
                            tilesadded++;
                        }
                        //TODO: This is REALLY BAD
                        //TODO: LIKE SERIOUSLY WHY DID I THINK ANYTHING HERE WAS A GOOD IDEA
                        //cell.sector = new Sector(); //no sector management
                        if (tempPlanemap[index].zone >= 0)
                            cell.zone = tempPlanemap[index].zone;

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
