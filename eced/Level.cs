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
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace eced
{
    class Level
    {
        public int width = 64, height = 64, depth = 1;

        //public Cell[, ,] cells;

        private List<Plane> planes = new List<Plane>();

        private List<Tile> internalTileset = new List<Tile>();
        private List<Thing> things = new List<Thing>();
        private List<Zone> zonedefs = new List<Zone>();
        //private Dictionary<int, Trigger> triggerList = new Dictionary<int, Trigger>();
        //private List<int> triggerKeys = new List<int>();

        private List<NumberCell> tempPlanemap;

        public ThingManager localThingList;

        public int lastFloorCode = 0;

        public Thing highlighted = null;
        public Cell highlightedTrigger = null;
        public int[] highlightedPos = new int[2];

        public Level(int w, int h, int d, Tile defaultTile)
        {
            Random r = new Random();
            this.width = w;
            this.height = h;
            this.depth = d;

            //cells = new Cell[width, height, depth];

            planes.Add(new Plane(w, h));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    planes[0].cells[x, y] = new Cell();
                    planes[0].cells[x, y].tile = defaultTile;
                }
            }
            if (defaultTile != null)
                internalTileset.Add(defaultTile);
            Console.WriteLine("tileset size: {0}", internalTileset.Count);
        }

        public void addTile(Tile tile)
        {
            Console.WriteLine("adding tile to internal tileset");
            internalTileset.Add(tile);
            Console.WriteLine("tileset size: {0}", internalTileset.Count);
        }

        public Tile getTile(int x, int y, int z)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return planes[z].cells[x, y].tile;

            return TileManager.tile1;
        }

        public void setTile(int x, int y, int z, Tile tile)
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
                    markChunkDirty(x / 16, y / 16);
                }
            }
        }

        public void markChunkDirty(int x, int y)
        {
        }

        public void markAllChunksDirty()
        {
        }

        public void addThing(Thing thing)
        {
            this.things.Add(thing);
            markChunkDirty((int)thing.x / 16, (int)thing.y / 16);
        }

        public List<Thing> getThings()
        {
            return things;
        }

        public List<Thing> getThingsInRange(int sx, int sy, int w, int h)
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

        public ThingDefinition getThingDef(Thing thing)
        {
            return localThingList.thinglist[thing.typeid];
        }

        public void highlightThing(int x, int y)
        {
            if (this.highlighted != null)
                return;

            for (int lx = 0; lx < this.things.Count; lx++)
            {
                Thing thing = things[lx];
                ThingDefinition def = getThingDef(thing);
                int sx = (int)thing.getXCoord() - def.radius; int ex = (int)thing.getXCoord() + def.radius;
                int sy = (int)thing.getYCoord() - def.radius; int ey = (int)thing.getYCoord() + def.radius;
                if (x >= sx && x < ex && y >= sy && y < ey)
                {
                    this.highlighted = thing;
                    thing.highlighted = true;
                    Console.WriteLine("Highlighted");
                    markChunkDirty((int)thing.x / 16, (int)thing.y / 16);
                    return;
                }
            }
        }

        public void updateHighlight(int x, int y)
        {
            if (this.highlighted == null)
            {
                highlightThing(x, y);
                return;
            }

            ThingDefinition def = getThingDef(highlighted);
            int sx = (int)highlighted.getXCoord() - def.radius; int ex = (int)highlighted.getXCoord() + def.radius;
            int sy = (int)highlighted.getYCoord() - def.radius; int ey = (int)highlighted.getYCoord() + def.radius;
            if ((x < sx || x >= ex || y < sy || y >= ey) && 
                !this.highlighted.moving)
            {
                Console.WriteLine("unhighlighting");
                this.highlighted.highlighted = false;
                markChunkDirty((int)highlighted.x / 16, (int)highlighted.y / 16);
                this.highlighted = null;
            }
        }

        public void deleteThing(Thing thing)
        {
            if (this.things.Contains(thing))
            {
                int lx = (int)thing.x;
                int ly = (int)thing.y;

                this.things.Remove(thing);

                markChunkDirty(lx / 16, ly / 16);
            }
        }

        public void replaceThing(Thing thing, Thing newThing)
        {
            if (this.things.Contains(thing))
            {
                int lx = (int)newThing.x;
                int ly = (int)newThing.y;

                this.things[this.things.IndexOf(thing)] = newThing;

                markChunkDirty(lx / 16, ly / 16);
            }
        }

        public void addTrigger(int x, int y, int z, Trigger trigger)
        {
            planes[z].cells[x, y].triggerList.Add(trigger);
            markChunkDirty(x / 16, y / 16);
        }

        public List<Trigger> getTriggers(int x, int y, int z)
        {
            return planes[z].cells[x, y].triggerList;
        }

        /*public List<Trigger> getTriggersInChunk(int x, int y, int z)
        {
            List<Trigger> ltriggerList = new List<Trigger>();
            int sx = x * 16; int ex = sx + 16;
            int sy = y * 16; int ey = sy + 16;
            for (int i = 0; i < triggerKeys.Count; i++)
            {
                Trigger trigger = this.triggerList[triggerKeys[i]];
                if (trigger.x >= sx && trigger.x < ex && trigger.y >= sy && trigger.y < ey)
                {
                    ltriggerList.Add(trigger);
                }
            }

            return ltriggerList;
        }*/

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public short[] buildPlaneData(int layer)
        {
            short[] planeData = new short[this.width * this.height * 4];

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    planeData[(x * width + y) * 4] = (short)planes[layer].cells[x, y].tile.id;
                }
            }

            return planeData;
        }

        /// <summary>
        /// Builds a 1-dimensional array representing the data of all known resources
        /// Required for the renderer. Intended to be uploaded as an RGBA32I texture
        /// </summary>
        /// <param name="numTextures">The amount of textures returned in this texture</param>
        /// <returns></returns>
        public short[] buildResourceData(ref int numTextures)
        {
            short[] textureData = new short[256 * 4];

            //HACK: fills out with 256 8x8 texutres
            //TODO: get real resource mangament

            for (int i = 0; i < 256; i++)
            {
                textureData[i * 4 + 0] = 8;
                textureData[i * 4 + 1] = 8;
                textureData[i * 4 + 2] = 0;//(short)(i % 16);
                textureData[i * 4 + 3] = 0;// (short)(i / 16);
            }

            numTextures = 256;

            return textureData;
        }

        public void highlightTrigger(int x, int y, int z)
        {
            List<Trigger> trigger = getTriggers(x, y, z);
            if (trigger.Count > 0)
            {
                Console.WriteLine("highlighting!");
                planes[z].cells[x, y].highlighted = true;
                this.highlightedTrigger = planes[z].cells[x, y];
                this.highlightedPos[0] = x;
                this.highlightedPos[1] = y;
                this.markChunkDirty(x / 16, y / 16);
            }
        }

        public void updateTriggerHighlight(int x, int y, int z)
        {
            if (x < 0 || y < 0)
                return;

            if (this.highlightedTrigger == null)
            {
                highlightTrigger(x, y, z);
                return;
            }
            else
            {
                if (x != this.highlightedPos[0] || y != this.highlightedPos[1])
                {
                    this.highlightedTrigger.highlighted = false;
                    this.markChunkDirty(this.highlightedPos[0] / 16, this.highlightedPos[1] / 16);
                    this.highlightedTrigger = null;
                    return;
                }
            }
        }

        public void assignFloorCode(int x, int y, int z, int code)
        {
            Console.WriteLine("setting code {0}", code);
            if (code == zonedefs.Count)
            {
                zonedefs.Add(new Zone());
                Console.WriteLine("adding a zone");
            }

            planes[z].cells[x, y].zone = zonedefs[code];
            this.markChunkDirty(x / 16, y / 16);
        }

        public int getUniqueCode()
        {
            Console.WriteLine("getting code {0}", zonedefs.Count);
            return zonedefs.Count;
        }

        public void saveToUWMFFile(string filename)
        {
            StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create));

            sw.Write("namespace = \"Wolf3D\";\n");
            sw.Write("tilesize = 64;\n");
            sw.Write("name = \"ecedtest\";\n");
            sw.Write("width = " + width.ToString() + ";\n");
            sw.Write("height = " + height.ToString() + ";\n");
            
            //Plane plane = new Plane();

            for (int x = 0; x < internalTileset.Count; x++)
            {
                sw.Write(internalTileset[x].getUWMFString());
                sw.Write("\n");
            }
            sw.Write("\n");
            for (int x = 0; x < zonedefs.Count; x++)
            {
                sw.Write(zonedefs[x].getUWMFString());
                sw.Write("\n");
            }
            sw.Write("\n");
            for (int x = 0; x < things.Count; x++)
            {
                sw.Write(things[x].getUWMFString());
                sw.Write("\n");
            }
            sw.Write("\n");

            sw.Write(new Sector().getUWMFString());
            sw.Write("\n");

            for (int x = 0; x < planes.Count; x++)
            {
                sw.Write(planes[x].makeUWMFString());
            }
            sw.Write("\n");

            for (int x = 0; x < planes.Count; x++)
            {
                sw.Write(writePlaneDef(x));
            }
            sw.Write("\n");

            for (int i = 0; i < width * height; i++)
            {
                for (int p = 0; p < this.depth; p++)
                {
                    int x = i % width;
                    int y = i / width;
                    for (int li = 0; li < planes[p].cells[x, y].triggerList.Count; x++)
                    {
                        sw.Write(planes[p].cells[x, y].triggerList[li].getUWMFString());
                    }
                    sw.Write("\n");
                }
            }
            sw.Write("\n");

            sw.Flush();
            sw.Close();
        }

        public string writePlaneDef(int plane)
        {
            StringBuilder stringbuilder = new StringBuilder();
            stringbuilder.Append("planemap\n");
            stringbuilder.Append("{\n");
            for (int i = 0; i < width * height; i++)
            {
                int x = i % width;
                int y = i / width;
                stringbuilder.Append("\t{"); stringbuilder.Append(getTileId(planes[plane].cells[x, y].tile));
                stringbuilder.Append(", "); stringbuilder.Append(getSectorID(x, y, plane));
                stringbuilder.Append(", "); stringbuilder.Append(getZoneId(planes[plane].cells[x, y]));
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

        public int getTileId(Tile tile)
        {
            if (internalTileset.Contains(tile))
                return internalTileset.IndexOf(tile);

            return -1;
        }

        public int getSectorID(int x, int y, int z)
        {
            return 0;
        }

        public int getZoneId(Cell cell)
        {
            if (cell.zone == null)
            {
                return -1;
            }
            return zonedefs.IndexOf(cell.zone);
        }

        public bool compareZoneID(int x, int y, int z, int code)
        {
            return getZoneId(planes[z].cells[x, y]) == code;
        }

        public Zone getZoneAt(int x, int y, int z)
        {
            return planes[z].cells[x, y].zone;
        }

        public int getZoneIDAt(int x, int y, int z)
        {
            if (zonedefs.Contains(planes[z].cells[x, y].zone))
            {
                return zonedefs.IndexOf(planes[z].cells[x, y].zone);
            }
            return -1;
        }

        public void setTempPlaneMap(List<NumberCell> planemap)
        {
            this.tempPlanemap = planemap;
        }

        public void addZone(Zone zone)
        {
            this.zonedefs.Add(zone);
        }

        public List<Zone> getZones()
        {
            return this.zonedefs;
        }

        public bool isCellHighlighted(int x, int y, int z)
        {
            return planes[z].cells[x, y].highlighted;
        }

        public void processPlanemap()
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
