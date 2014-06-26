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
        //float panx = -0.5f, pany = -0.5f;
        OpenTK.Vector2 pan = new OpenTK.Vector2(0.0f, 0.0f);
        int tilesx = 16, tilesy = 16;

        int screenwidth = 0, screenheight = 0;

        int vboindex = 0;

        int worldTextureID;
        int resourceTextureID;
        int atlasTextureID;

        Random r = new Random();

        public float zoom = 2.0f;

        float[] vbo = {   0.0f, 0.0f, 0.0f, 1.0f,
                          1.0f, 0.0f, 0.0f, 1.0f, 
                          1.0f, 1.0f, 0.0f, 1.0f, 
                          
                          1.0f, 1.0f, 0.0f, 1.0f, 
                          0.0f, 1.0f, 0.0f, 1.0f, 
                          0.0f, 0.0f, 0.0f, 1.0f };

        public void tempSetupShaderRenderer(Level level, uint program, OpenTK.Vector2 winsize)
        {
            GL.UseProgram(program);
            int numTextures = 0;
            short[] mapTexture = level.buildPlaneData(0);
            short[] resTexture = level.buildResourceData(ref numTextures);

            byte[] testArray = new byte[64 * 64 * 4];

            //for (int i = 0; i < 4096; i++)
            //{
            r.NextBytes(testArray);
            //}

            //int mapTextureID = GL.GenTexture();
            //int resTextureID = GL.GenTexture();

            int[] tids = new int[4]; GL.GenTextures(4, tids);

            GL.ActiveTexture(TextureUnit.Texture0);
            atlasTextureID = tids[0] = TextureManager.getTexture(".\\resources\\sneswolftiles.PNG");
            worldTextureID = tids[1];
            resourceTextureID = tids[2];

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, tids[1]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, level.width, level.height, 0, PixelFormat.RgbaInteger, PixelType.Short, mapTexture);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, tids[2]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, 1, numTextures, 0, PixelFormat.RgbaInteger, PixelType.Short, resTexture);

            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, tids[3]);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 64, 64, 0, PixelFormat.Rgba, PixelType.UnsignedByte, testArray);

            GL.ActiveTexture(TextureUnit.Texture0);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP TEXTURE GL Error: {0}", error.ToString());
            }

            int panUL = GL.GetUniformLocation(program, "pan");
            int zoomUL = GL.GetUniformLocation(program, "zoom");
            int windowUL = GL.GetUniformLocation(program, "window");
            int tilesizeUL = GL.GetUniformLocation(program, "tilesize");
            int atlasUL = GL.GetUniformLocation(program, "atlas");
            int mapPlaneUL = GL.GetUniformLocation(program, "mapPlane");
            int texInfoUL = GL.GetUniformLocation(program, "texInfo");
            int fovUL = GL.GetUniformLocation(program, "fov");
            int projectUL = GL.GetUniformLocation(program, "project");

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP UNIFORMS FIND GL Error: {0}", error.ToString());
            }

            GL.Uniform2(panUL, pan);
            //forced zoom level
            GL.Uniform1(zoomUL, 1.0f);
            //TODO: tilesize fixed
            GL.Uniform1(tilesizeUL, 8f);
            GL.Uniform2(windowUL, (int)winsize.X, (int)winsize.Y);
            //Console.WriteLine(winsize.X / winsize.Y);
            GL.Uniform1(atlasUL, 0);
            GL.Uniform1(mapPlaneUL, 1);
            GL.Uniform1(texInfoUL, 2);
            GL.Uniform1(fovUL, winsize.X / winsize.Y);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP UNIFORMS GL Error: {0}", error.ToString());
            }

            vboindex = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboindex);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * vbo.Length) ,vbo, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.UseProgram(0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP BUFFERS GL Error: {0}", error.ToString());
            }
        }

        public void drawLevel(Level level, uint program, OpenTK.Vector2 winsize)
        {
            GL.UseProgram(program);

            int panUL = GL.GetUniformLocation(program, "pan");
            int zoomUL = GL.GetUniformLocation(program, "zoom");
            int windowUL = GL.GetUniformLocation(program, "window");
            int tilesizeUL = GL.GetUniformLocation(program, "tilesize");
            int atlasUL = GL.GetUniformLocation(program, "atlas");
            int mapPlaneUL = GL.GetUniformLocation(program, "mapPlane");
            int texInfoUL = GL.GetUniformLocation(program, "texInfo");
            int projectUL = GL.GetUniformLocation(program, "project");

            GL.Uniform2(panUL, pan);
            GL.Uniform1(zoomUL, zoom);
            //TODO: tilesize fixed
            GL.Uniform1(tilesizeUL, 64.0f);
            //GL.Uniform2(windowUL, winsize);
            GL.Uniform2(windowUL, (int)winsize.X, (int)winsize.Y);
            OpenTK.Matrix4 project = OpenTK.Matrix4.CreateOrthographic(winsize.X / 64f / 8f, winsize.Y / 64f / 8f, -8f, 8f);
            GL.UniformMatrix4(projectUL, false, ref project);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, worldTextureID);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, resourceTextureID);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboindex);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.UseProgram(0);
        }

        public void updateWorldTexture(Level level)
        {
            for (int i = 0; i < level.updateCells.Count; i++)
            {
                OpenTK.Vector2 loc = level.updateCells[i];
                Tile tile = level.getTile((int)loc.X, (int)loc.Y, 0);
                short[] id = new short[4];
                if (tile != null)
                    id[0] = id[1] = id[2] = id[3] = (short)tile.id;
                else
                    id[0] = id[1] = id[2] = id[3] = (short)-1;

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, worldTextureID);

                GL.TexSubImage2D(TextureTarget.Texture2D, 0, (int)loc.X, (int)loc.Y, 1, 1, PixelFormat.RgbaInteger, PixelType.Short, id);

                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.ActiveTexture(TextureUnit.Texture0);
            }
            level.updateCells.Clear();
        }

        public void renderThing(double lx, double ly, Thing thing, Level level)
        {
            //preserved for reference
            /*double x = lx * tilesize + (tilesize / 2);
            double y = ly * tilesize + (tilesize / 2);
            GL.PushMatrix();

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

            GL.PopMatrix();*/
        }

        public void addTriggersToList(ref List<Trigger> to, ref List<Trigger> from)
        {
            for (int x = 0; x < from.Count; x++)
            {
                to.Add(from[x]);
            }
        }

        public void setpan(OpenTK.Vector2 newPan)
        {
            this.pan = newPan;
        }
    }
}
