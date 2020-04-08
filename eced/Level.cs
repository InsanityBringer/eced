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
        public int TileSize { get; set; } = 64;
        public List<Plane> Planes { get; } = new List<Plane>();
        public int Brightness { get; set; } = 255;
        public float Visibility { get; set; } = 1.0f;
        public string Name { get; set; } = "New Level";
        public bool Experimental { get; set; } = false;

        //Data management
        public List<Tile> Tileset { get; } = new List<Tile>();
        public Dictionary<Tile, int> TilesetMap { get; } = new Dictionary<Tile, int>();
        public List<Sector> Sectorset { get; } = new List<Sector>();
        public Dictionary<Sector, int> SectorsetMap { get; } = new Dictionary<Sector, int>();
        public List<TriggerList> Triggers { get; } = new List<TriggerList>();
        public Dictionary<TilePosition, int> TriggersMap { get; } = new Dictionary<TilePosition, int>();
        //public List<Thing> Things { get; } = new List<Thing>();
        public List<Zone> ZoneDefs { get; } = new List<Zone>();

        //Dirty rectangle properties
        public bool Dirty { get; private set; } = false;
        public DirtyRectangle dirtyRectangle; //could be property but the structure nature makes that useless... TODO make better

        public ThingManager localThingList;

        public int lastFloorCode = 0;

        public int[] highlightedPos = new int[2];

        public List<ResourceFiles.Archive> loadedResources = new List<ResourceFiles.Archive>();

        public Level(int w, int h, int d, Tile defaultTile)
        {
            Random r = new Random();
            this.Width = w;
            this.Height = h;
            this.Depth = d;

            for (int i = 0; i < Depth; i++)
                Planes.Add(new Plane(w, h));

            Sectorset.Add(new Sector());

            for (int z = 0; z < Depth; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Planes[z].cells[x, y].tile = 0;
                        Planes[z].cells[x, y].sector = 0;
                        Planes[z].cells[x, y].zone = -1;
                    }
                }
            }
            if (defaultTile != null)
            {
                AddTile(defaultTile);
            }
            ClearDirty();
        }

        /// <summary>
        /// Creates a completely blank level, for deserializing purposes. 
        /// </summary>
        private Level()
        {
            this.Width = 0;
            this.Height = 0;
            this.Depth = 0;

            ClearDirty();
        }

        public void AddPlane(Plane plane, int where = -1)
        {
            if (plane.Width != Width || plane.Height != Height)
            {
                throw new Exception("Level::AddPlane: Plane added with mismatched width and height.");
            }
            if (where < 0 || where >= Planes.Count) //append to top of stack
            {
                Planes.Add(plane);
            }
            else
            {
                Planes.Insert(where, plane);
            }
            Depth = Planes.Count;
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
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                return Planes[z].cells[x, y].zone;
            }
            return -1;
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

        public int CalculateHeight()
        {
            int height = 0;
            foreach (Plane plane in Planes)
            {
                height += plane.Depth;
            }
            return height;
        }

        public int CalculatePlaneForHeight(float z)
        {
            int zInMU = (int)(z * TileSize);
            int current = 0;
            for (int i = 0; i < Planes.Count; i++)
            {
                current += Planes[i].Depth;
                if (zInMU < current) return i;
            }
            return -1;
        }

        public void LinkThing(Thing thing)
        {
            int zInMU = (int)(thing.z * TileSize);
            //Clamp the thing to the height of the level
            if (thing.z < 0) thing.z = 0;
            if (zInMU >= CalculateHeight()) thing.z = ((float)(CalculateHeight() - 1) / TileSize);
            //Link the thing into the proper plane.
            int plane = CalculatePlaneForHeight(thing.z);
            Planes[plane].Things.Add(thing);
        }

        public void AddThing(Thing thing)
        {
            LinkThing(thing);
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
            List<Thing> localThings = Planes[res.z].Things;
            for (int lx = 0; lx < localThings.Count; lx++)
            {
                Thing thing = localThings[lx];
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
            int plane = CalculatePlaneForHeight(thing.z);
            if (Planes[plane].Things.Contains(thing))
            {
                Planes[plane].Things.Remove(thing);
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

        public void DeleteTriggersAt(int x, int y, int z)
        {
            TilePosition pos; pos.x = x; pos.y = y; pos.z = z;
            if (TriggersMap.ContainsKey(pos))
            {
                Triggers.RemoveAt(TriggersMap[pos]);
                ResetTriggerMap();
            }
        }

        private void ResetTriggerMap()
        {
            TriggersMap.Clear();
            TriggerList triggers;
            for (int i = 0; i < Triggers.Count; i++)
            {
                triggers = Triggers[i];
                TriggersMap.Add(triggers.pos, i);
            }
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
			
            if (Experimental)
                sb.Append("namespace = \"ECWolf-v12\";\n");
            else
                sb.Append("namespace = \"Wolf3D\";\n");
            sb.AppendFormat("tilesize = {0};\n", TileSize);
            //cheap escaping
            sb.AppendFormat("name = \"{0}\";\n", Name.Replace("\"", "\\\""));
            sb.AppendFormat("width = {0};\n", Width);
            sb.AppendFormat("height = {0};\n", Height);
            if (Experimental)
            {
                sb.AppendFormat("defaultlightlevel = {0};\n", Brightness);
                sb.AppendFormat("defaultvisibility = {0:N2};\n", Visibility);
            }

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
            for (int i = 0; i < Depth; i++)
            {
                for (int x = 0; x < Planes[i].Things.Count; x++)
                {
                    sb.Append(Planes[i].Things[x].Serialize());
                    sb.Append("\n");
                }
                sb.Append("\n");
            }

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
                sb.Append("\n");
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

        public static bool DeserializeLevel(CodeImp.DoomBuilder.IO.UniversalCollection collection, out Level level)
        {
            //Things and triggers are linked to planes, so hold a buffer of them and add them later in case the planes haven't been loaded yet
            List<Thing> localThings = new List<Thing>();
            List<Trigger> localTriggers = new List<Trigger>();
            level = null;
            string levelNamespace;
            if (!collection[0].Key.Equals("namespace", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Level::DeserializeLevel: Namespace isn't first value in UWMF map.");
            }
            levelNamespace = (string)collection[0].Value;
            if (levelNamespace != "ECWolf-v12" && levelNamespace != "Wolf3D")
            {
                throw new Exception(string.Format("Level::DeserializeLevel: Unexpected namespace {0}.", levelNamespace));
            }
            string key;
            level = new Level();
            int planenum = 0;
            if (levelNamespace == "ECWolf-v12")
                level.Experimental = true;
            foreach (CodeImp.DoomBuilder.IO.UniversalEntry entry in collection)
            {
                key = entry.Key.ToLowerInvariant();
                switch (key)
                {
                    case "width":
                        entry.ValidateType(typeof(int));
                        level.Width = (int)entry.Value;
                        break;
                    case "height":
                        entry.ValidateType(typeof(int));
                        level.Height = (int)entry.Value;
                        break;
                    case "tilesize":
                        entry.ValidateType(typeof(int));
                        level.TileSize = (int)entry.Value;
                        break;
                    case "name":
                        entry.ValidateType(typeof(string));
                        level.Name = (string)entry.Value;
                        break;
                    case "defaultlightlevel":
                        if (!level.Experimental)
                            throw new Exception("Level::DeserializeLevel: Attempting to use defaultlightlevel in non-experimental map.");
                        entry.ValidateType(typeof(int));
                        level.Brightness = (int)entry.Value;
                        break;
                    case "defaultvisibility":
                        if (!level.Experimental)
                            throw new Exception("Level::DeserializeLevel: Attempting to use defaultvisibility in non-experimental map.");
                        try
                        {
                            entry.ValidateType(typeof(float));
                            level.Visibility = (float)entry.Value;
                        }
                        catch (Exception) //no decimal places, try integer instead
                        {
                            entry.ValidateType(typeof(int));
                            level.Visibility = (float)((int)entry.Value);
                        }
                        break;
                    case "tile":
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        Tile tile = Tile.Deserialize((CodeImp.DoomBuilder.IO.UniversalCollection)entry.Value);
                        level.AddTile(tile);
                        break;
                    case "sector":
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        Sector sector = Sector.Deserialize((CodeImp.DoomBuilder.IO.UniversalCollection)entry.Value);
                        level.AddSector(sector);
                        break;
                    case "plane":
                        if (level.Width == 0 || level.Height == 0)
                            throw new Exception("Level::DeserializeLevel: Defining plane before level size is defined.");
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        Plane plane = Plane.Deserialize(level, (CodeImp.DoomBuilder.IO.UniversalCollection)entry.Value);
                        level.AddPlane(plane);
                        break;
                    case "zone":
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        Zone zone = new Zone();
                        level.AddZone(zone);
                        break;
                    case "thing":
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        Thing thing = Thing.Reconstruct((CodeImp.DoomBuilder.IO.UniversalCollection)entry.Value);
                        //level.AddThing(thing);
                        localThings.Add(thing);
                        break;
                    case "trigger":
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        Trigger trigger = Trigger.Reconstruct((CodeImp.DoomBuilder.IO.UniversalCollection)entry.Value);
                        //level.AddTrigger(trigger.x, trigger.y, trigger.z, trigger);
                        localTriggers.Add(trigger);
                        break;
                    case "planemap":
                        entry.ValidateType(typeof(CodeImp.DoomBuilder.IO.UniversalCollection));
                        level.ProcessPlanemap((CodeImp.DoomBuilder.IO.UniversalCollection)entry.Value, ref planenum);
                        break;
                }
            }
            //All planes should be loaded, so link the things and triggers now.
            foreach (Thing thing in localThings)
            {
                level.AddThing(thing);
            }
            foreach (Trigger trigger in localTriggers)
            {
                level.AddTrigger(trigger.x, trigger.y, trigger.z, trigger);
            }
            return true;
        }

        public void ProcessPlanemap(CodeImp.DoomBuilder.IO.UniversalCollection collection, ref int planenum)
        {
            if (planenum >= Planes.Count)
            {
                throw new Exception(string.Format("Level::ProcessPlanemap: Planemap specified for plane {0} but plane {0} has not been created.", planenum));
            }
            List<Cell> newCells;
            foreach (CodeImp.DoomBuilder.IO.UniversalEntry entry in collection)
            {
                string name = entry.Key.ToLowerInvariant();
                switch (name)
                {
                    case "planedata":
                        entry.ValidateType(typeof(List<Cell>));
                        newCells = (List<Cell>)entry.Value;
                        if (newCells.Count < Width * Height)
                        {
                            throw new Exception(string.Format("Level::ProcessPlanemap: Insufficient planemap data for plane {0}.", planenum));
                        }
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                Planes[planenum].cells[x, y] = newCells[y * Width + x];
                            }
                        }
                        break;
                }
            }
            planenum++;
        }
    }
}
