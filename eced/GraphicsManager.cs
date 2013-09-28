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
using OpenTK.Graphics.OpenGL;

namespace eced
{
    class GraphicsManager
    {
        int panx = 0, pany = 0;
        int tilesx = 16, tilesy = 16;

        int screenwidth = 0, screenheight = 0;

        Random r = new Random();

        /// <summary>
        /// Current tile size, used for zoom
        /// </summary>
        public int tilesize = 32;
        private void renderTile(int x, int y, Tile tile)
        {
            //GL.Color3(r.NextDouble(), r.NextDouble(), r.NextDouble());

            int sx = x, sy = y;
            int ex = x + tilesize, ey = y + tilesize;

            int indexx = tile.id % 16;
            int indexy = tile.id / 16;

            double tsx = (double)indexx / 16d;
            double tsy = (double)indexy / 16d;

            double tex = (double)(indexx+1) / 16d;
            double tey = (double)(indexy+1) / 16d;

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(tsx, tsy); GL.Vertex3(sx, sy, 3);
            GL.TexCoord2(tsx, tey); GL.Vertex3(sx, ey, 3);
            GL.TexCoord2(tex, tey); GL.Vertex3(ex, ey, 3);
            GL.TexCoord2(tex, tsy); GL.Vertex3(ex, sy, 3);

            GL.End();
        }

        private void renderTrigger(Trigger trigger, bool highlight)
        {
            if (highlight)
                GL.Color3(1f, 1f, 0f);
            else GL.Color3(0f, 1f, 0f);

            int sx = trigger.x * tilesize; int ex = sx + tilesize;
            int sy = trigger.y * tilesize; int ey = sy + tilesize;

            //draw an 'x'
            GL.Begin(BeginMode.Lines);

            GL.Vertex3(sx, sy, 2.05);
            GL.Vertex3(ex, ey, 2.05);

            GL.Vertex3(ex, sy, 2.05);
            GL.Vertex3(sx, ey, 2.05);

            GL.End();
        }

        private void renderHexCode(int x, int y, int digit)
        {
            int sx = x * tilesize; int sy = y * tilesize;
            int ey = sy + tilesize; int ex = sx + (tilesize / 2);
            int digit1 = digit / 16;
            int digit2 = digit % 16;

            double tsy = 0; double tey = 1;

            double tsx = (double)digit1 / 16;
            double tex = (double)(digit1 + 1) / 16d;

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(tsx, tsy); GL.Vertex3(sx, sy, 2.75);
            GL.TexCoord2(tsx, tey); GL.Vertex3(sx, ey, 2.75);
            GL.TexCoord2(tex, tey); GL.Vertex3(ex, ey, 2.75);
            GL.TexCoord2(tex, tsy); GL.Vertex3(ex, sy, 2.75);

            GL.End();

            sx += (tilesize / 2); ex += (tilesize / 2);

            tsx = (double)digit2 / 16d;
            tex = (double)(digit2 + 1) / 16d;

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(tsx, tsy); GL.Vertex3(sx, sy, 2.75);
            GL.TexCoord2(tsx, tey); GL.Vertex3(sx, ey, 2.75);
            GL.TexCoord2(tex, tey); GL.Vertex3(ex, ey, 2.75);
            GL.TexCoord2(tex, tsy); GL.Vertex3(ex, sy, 2.75);

            GL.End();
        }

        public void renderThing(double lx, double ly, Thing thing, Level level)
        {
            double x = lx * tilesize + (tilesize / 2);
            double y = ly * tilesize + (tilesize / 2);
            GL.PushMatrix();

            /*GL.LoadIdentity();
            GL.Scale(1, -1, -1);

            GL.Translate(-((double)panx + (double)(screenwidth / 2d)), -((double)pany + (double)(screenheight / 2d)), 0d);

            //offset to the render location
            GL.Translate(x, y, 0d);*/

            ThingDefinition def = level.getThingDef(thing);

            double radius = (double)def.radius;
            radius *= ((double)tilesize / 64d);

            double sx = x - radius; double sy = y - radius;
            double ex = x + radius; double ey = y + radius;

            double r = (double)def.r / 255d;
            double g = (double)def.g / 255d;
            double b = (double)def.b / 255d;

            if (thing.highlighted)
            {
                r += .5d;
                g += .5d;
            }

            GL.Color4(r, g, b, 1d);

            GL.Begin(BeginMode.Quads);

            GL.Vertex3(sx, sy, 2d);
            GL.Vertex3(sx, ey, 2d);
            GL.Vertex3(ex, ey, 2d);
            GL.Vertex3(ex, sy, 2d);

            GL.End();

            GL.Translate(x, y, 0);

            //set rot to normal for the arrow
            GL.Rotate(270, 0d, 0d, 1d);
            //rotate by the thing's angle
            GL.Rotate(360 - thing.angle, 0, 0, 1d);
            //arrow is a bit darker
            GL.Color4(r/4d, g/4d, b/4d, 1d);

            //render the arrow as lines
            //draw around origin for rotation
            GL.Begin(BeginMode.Lines);

            //stalk
            GL.Vertex2(0, -radius / 2d);
            GL.Vertex2(0, radius / 2d);

            //head
            GL.Vertex2(-(radius / 2d), 0);
            GL.Vertex2(0, radius / 2d);
            GL.Vertex2((radius / 2d), 0);
            GL.Vertex2(0, radius / 2d); 
            
            GL.End();

            GL.PopMatrix();
        }

        public void addTriggersToList(ref List<Trigger> to, ref List<Trigger> from)
        {
            for (int x = 0; x < from.Count; x++)
            {
                to.Add(from[x]);
            }
        }

        public void renderLevel(Level level)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Scale(1, -1, -1);
            //GL.Translate(-320, -240, 0);

            int sfx = panx / tilesize;
            int sfy = pany / tilesize;

            GL.Translate(-((double)panx + (double)(screenwidth / 2d)), -((double)pany + (double)(screenheight / 2d)), 0d);

            if (sfx < 0)
                sfx = 0;

            if (sfy < 0)
                sfy = 0;

            /*for (int x = sfx; x < 64; x++)
            {
                for (int y = sfy; y < 64; y++)
                {
                    if (level.getTile(x, y, 0) != null)
                        renderTile(x * tilesize, y * tilesize, level.getTile(x, y, 0));
                }
            }*/
            GL.Color3(1f, 1f, 1f);

            for (int x = 0; x < level.width / 16; x++)
            {
                for (int y = 0; y < level.height / 16; y++)
                {
                    RenderChunk chunk = level.chunkids[y * (level.width / 16) + x];
                    if (chunk.dirty)
                    {
                        List<Trigger> triggerlist = new List<Trigger>();
                        //Console.WriteLine("Chunk {0} getting rebuilt", chunk.listid);
                        GL.DeleteLists(chunk.listid, 1);
                        GL.NewList(chunk.listid, ListMode.Compile);

                        GL.Color3(1f, 1f, 1f);
                        int sx = x * 16; int ex = sx + 16;
                        int sy = y * 16; int ey = sy + 16;

                        TextureManager.getTexture("./resources/sneswolftiles.PNG");
                        
                        for (int lx = sx; lx < ex; lx++)
                        {
                            for (int ly = sy; ly < ey; ly++)
                            {
                                if (level.getTile(lx, ly, 0) != null)
                                    renderTile(lx * tilesize, ly * tilesize, level.getTile(lx, ly, 0));

                                List<Trigger> celltriggers = level.getTriggers(lx, ly, 0);
                                addTriggersToList(ref triggerlist, ref celltriggers);
                            }
                        }

                        TextureManager.getTexture("./resources/floorfont.PNG");
                        for (int lx = sx; lx < ex; lx++)
                        {
                            for (int ly = sy; ly < ey; ly++)
                            {
                                if (level.getZoneIDAt(lx, ly, 0) != -1)
                                {
                                    renderHexCode(lx, ly, level.getZoneIDAt(lx, ly, 0));
                                }
                            }
                        }

                        GL.Disable(EnableCap.Texture2D);

                        List<Thing> thinglist = level.getThingsInRange(x * 16, y * 16, 16, 16);

                        for (int i = 0; i < thinglist.Count; i++)
                        {
                            Thing thing = thinglist[i];
                            renderThing(thing.x, thing.y, thing, level);
                        }
                        GL.Color3(1d, 1d, 1d);

                        //List<Trigger> triggerlist = level.getTriggersInChunk(x, y, 0);

                        for (int i = 0; i < triggerlist.Count; i++)
                        {
                            Trigger trigger = triggerlist[i];
                            renderTrigger(trigger, level.isCellHighlighted(trigger.x, trigger.y, trigger.z));
                        }

                        GL.Enable(EnableCap.Texture2D);

                        GL.EndList();

                        chunk.dirty = false;
                    }
                    GL.CallList(chunk.listid);
                }
            }

            GL.Disable(EnableCap.Texture2D);

            renderGridLines();

            GL.Enable(EnableCap.Texture2D);
        }

        public void renderGridLines()
        {
            //GL.Disable(EnableCap.Texture2D);
            GL.Color3(0, .5, 1.0);
            int sfx = Math.Max(0, panx / tilesize);
            int sfy = Math.Max(0, pany / tilesize);
            
            GL.Begin(BeginMode.Lines);
            for (int x = sfx; x < 65; x++)
            {
                GL.Vertex3(x * tilesize, 0, 2.5d);
                GL.Vertex3(x * tilesize, 64 * tilesize, 2.5d);
            }
            GL.End();

            GL.Begin(BeginMode.Lines);
            for (int y = sfy; y < 65; y++)
            {
                GL.Vertex3(0, y * tilesize, 2.5d);
                GL.Vertex3(64 * tilesize, y * tilesize, 2.5d);
            }
            GL.End();
            //GL.Enable(EnableCap.Texture2D);
        }

        public void pan(int px, int py, int screenx, int screeny)
        {
            panx = px;
            pany = py;

            tilesx = screenx / tilesize + 1;
            tilesy = screeny / tilesize + 1;

            if (tilesx > 64)
                tilesx = 64;
            if (tilesy > 64)
                tilesy = 64;

            this.screenwidth = screenx;
            this.screenheight = screeny;

            Console.WriteLine("{0} {1} {2} {3}", panx, pany, tilesx, tilesy);
        }
    }
}
