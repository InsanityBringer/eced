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
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace eced
{
    public struct BasicRenderUniforms
    {
        //VS
        public int panUL;
        public int zoomUL;
        public int tilesizeUL;
        public int projectUL;
        public int mapsizeUL;

        //FS
        public int atlasUL;
        public int mapPlaneUL;
        public int texInfoUL;
        public int numbersUL;
    }

    public struct BasicThingUniforms
    {
        public int panUL;// = GL.GetUniformLocation(program, "pan");
        public int zoomUL;// = GL.GetUniformLocation(program, "zoom");
        public int thingposUL;// = GL.GetUniformLocation(program, "thingpos");
        public int thingradUL;// = GL.GetUniformLocation(program, "thingrad");
        public int thingcolorUL;// = GL.GetUniformLocation(program, "thingColor");
        public int projectUL;// = GL.GetUniformLocation(program, "project");
        public int rotateUL;// = GL.GetUniformLocation(program, "rotate");
        public int mapsizeUL;// = GL.GetUniformLocation(program, "mapsize");
    }

    public class GraphicsManager
    {
        //float panx = -0.5f, pany = -0.5f;
        OpenTK.Vector2 pan = new OpenTK.Vector2(0.0f, 0.0f);
        int tilesx = 16, tilesy = 16;

        int screenwidth = 0, screenheight = 0;

        int vboindex = 0;

        int worldTextureID;
        int resourceTextureID;
        int atlasTextureID;
        int numberTextureID;

        Random r = new Random();

        BasicRenderUniforms uniformsBasicRender = new BasicRenderUniforms();
        BasicThingUniforms uniformsThingRender = new BasicThingUniforms();

        public TextureManager Textures { get; } = new TextureManager();
        public EditorState CurrentState { get; private set; }

        public float zoom = 2.0f;

        float[] levelVBO = {   0.0f, 0.0f, 0.0f, 1.0f,
                          1.0f, 0.0f, 0.0f, 1.0f,
                          1.0f, 1.0f, 0.0f, 1.0f,

                          1.0f, 1.0f, 0.0f, 1.0f,
                          0.0f, 1.0f, 0.0f, 1.0f,
                          0.0f, 0.0f, 0.0f, 1.0f };

        float[] bodyVBO = {   -0.5f, -0.5f, 0.0f, 1.0f,
                          0.5f, -0.5f, 0.0f, 1.0f,
                          0.5f, 0.5f, 0.0f, 1.0f,

                          0.5f, 0.5f, 0.0f, 1.0f,
                         -0.5f, 0.5f, 0.0f, 1.0f,
                         -0.5f,-0.5f, 0.0f, 1.0f };

        float[] arrowVBO = {  0.25f, 0.5f, 0.0f, 1.0f,
                              0.75f, 0.5f, 0.0f, 1.0f,
                              0.75f, 0.5f, 0.0f, 1.0f,
                              0.5f, 0.25f, 0.0f, 1.0f,
                              0.75f, 0.5f, 0.0f, 1.0f,
                              0.5f, 0.75f, 0.0f, 1.0f };

        float[] triggersVBO = {  0.0f,  0.0f, 0.0f, 1.0f,
                                 1.0f,  1.0f, 0.0f, 1.0f,
                                 0.0f,  1.0f, 0.0f, 1.0f,
                                 1.0f,  0.0f, 0.0f, 1.0f };

        float[] lineVBOTemplate = { 0.0f, 0.0f, 0.0f, 1.0f,
                                    0.0f, 0.0f, 0.0f, 1.0f};

        int frames = 0;

        private int arrowVBOID, bodyVBOID, triggersVBOID, lineVBOID;

        public GraphicsManager(EditorState editorState)
        {
            CurrentState = editorState;
        }

        public void setupLevelRendering(Level level, uint program, OpenTK.Vector2 winsize)
        {
            GL.UseProgram(program);

            byte[] testArray = new byte[64 * 64 * 4];

            uniformsBasicRender.panUL = GL.GetUniformLocation(program, "pan");
            uniformsBasicRender.zoomUL = GL.GetUniformLocation(program, "zoom");
            uniformsBasicRender.tilesizeUL = GL.GetUniformLocation(program, "tilesize");
            uniformsBasicRender.atlasUL = GL.GetUniformLocation(program, "atlas");
            uniformsBasicRender.mapPlaneUL = GL.GetUniformLocation(program, "mapPlane");
            uniformsBasicRender.texInfoUL = GL.GetUniformLocation(program, "texInfo");
            uniformsBasicRender.projectUL = GL.GetUniformLocation(program, "project");
            uniformsBasicRender.numbersUL = GL.GetUniformLocation(program, "numbers");
            uniformsBasicRender.mapsizeUL = GL.GetUniformLocation(program, "mapsize");

            ErrorCode error = GL.GetError();

            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP UNIFORMS FIND GL Error: {0}", error.ToString());
            }

            GL.Uniform2(uniformsBasicRender.panUL, pan);
            //forced zoom level
            GL.Uniform1(uniformsBasicRender.zoomUL, 1.0f);
            //TODO: tilesize fixed
            GL.Uniform1(uniformsBasicRender.tilesizeUL, 8f);
            GL.Uniform1(uniformsBasicRender.atlasUL, 0);
            GL.Uniform1(uniformsBasicRender.mapPlaneUL, 1);
            GL.Uniform1(uniformsBasicRender.texInfoUL, 2);
            GL.Uniform1(uniformsBasicRender.numbersUL, 3);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP UNIFORMS GL Error: {0}", error.ToString());
            }

            vboindex = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboindex);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * levelVBO.Length) ,levelVBO, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.UseProgram(0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP BUFFERS GL Error: {0}", error.ToString());
            }
        }

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public short[] BuildPlaneData(int layer)
        {
            short[] planeData = new short[CurrentState.CurrentLevel.width * CurrentState.CurrentLevel.height * 4];

            for (int x = 0; x < CurrentState.CurrentLevel.width; x++)
            {
                for (int y = 0; y < CurrentState.CurrentLevel.height; y++)
                {
                    //planeData[(x * width + y) * 4] = (short)planes[layer].cells[x, y].tile.id;
                    if (CurrentState.CurrentLevel.Planes[layer].cells[x, y].tile != null)
                        planeData[(y * CurrentState.CurrentLevel.width + x) * 4] = (short)Textures.getTextureID(CurrentState.CurrentLevel.Planes[layer].cells[x, y].tile.NorthTex);
                    else planeData[(y * CurrentState.CurrentLevel.width + x) * 4] = -1;

                    if (CurrentState.CurrentLevel.Planes[layer].cells[x, y].tile == null)
                        planeData[(y * CurrentState.CurrentLevel.width + x) * 4 + 1] = (short)CurrentState.CurrentLevel.ZoneDefs.IndexOf(CurrentState.CurrentLevel.Planes[layer].cells[x, y].zone);
                }
            }

            return planeData;
        }

        public void setupTextures(Level level)
        {
            short[] mapTexture = BuildPlaneData(0);

            //int[] tids = new int[4]; GL.GenTextures(4, tids);
            GL.ActiveTexture(TextureUnit.Texture0);
            atlasTextureID = Textures.atlasTextureID;
            worldTextureID = GL.GenTexture();
            resourceTextureID = Textures.resourceInfoID;
            numberTextureID = Textures.numberTextureID;

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, worldTextureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, level.width, level.height, 0, PixelFormat.RgbaInteger, PixelType.Short, mapTexture);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            //GL.ActiveTexture(TextureUnit.Texture0);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP TEXTURE GL Error: {0}", error.ToString());
            }
        }

        public void drawLevel(Level level, uint program, OpenTK.Vector2 winsize)
        {
            //Console.WriteLine("{0}", program);
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("DRAW ENTRY GL Error: {0}", error.ToString());
            }

            GL.UseProgram(program);
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("DRAW PROGRAM GL Error: {0}", error.ToString());
            }

            GL.Uniform2(uniformsBasicRender.panUL, pan);
            GL.Uniform1(uniformsBasicRender.zoomUL, zoom);
            //TODO: tilesize fixed
            GL.Uniform1(uniformsBasicRender.tilesizeUL, 64.0f);
            OpenTK.Matrix4 project = OpenTK.Matrix4.CreateOrthographic(winsize.X / 64f / 8f, winsize.Y / 64f / 8f, -8f, 8f);
            GL.UniformMatrix4(uniformsBasicRender.projectUL, false, ref project);
            GL.Uniform2(uniformsBasicRender.mapsizeUL, level.width, level.height);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("DRAW UNIFORMS GL Error: {0}", error.ToString());
            }

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, worldTextureID);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, resourceTextureID);

            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, numberTextureID);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("DRAW BIND TEX GL Error: {0}", error.ToString());
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboindex);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("DRAW GL Error: {0}", error.ToString());
            }


            if (frames == 0)
            {
                Console.WriteLine("heh");
            }

            frames++;

            GL.UseProgram(0);
        }

        public void updateWorldTexture(Level level)
        {
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, worldTextureID);
            for (int i = 0; i < level.updateCells.Count; i++)
            {
                OpenTK.Vector2 loc = level.updateCells[i];
                Tile tile = level.GetTile((int)loc.X, (int)loc.Y, 0);
                short[] id = new short[4];
                
                if (tile != null)
                {
                    id[0] = id[1] = id[2] = id[3] = (short)Textures.getTextureID(tile.NorthTex);
                }
                else
                {
                    id[0] = id[2] = id[3] = (short)-1;
                    id[1] = (short)level.GetZoneID((int)loc.X, (int)loc.Y, 0);
                }

                GL.TexSubImage2D(TextureTarget.Texture2D, 0, (int)loc.X, (int)loc.Y, 1, 1, PixelFormat.RgbaInteger, PixelType.Short, id);

                //Console.WriteLine("pushing {0} cell {1} {2}", id[0], loc.X, loc.Y);
            }
            level.updateCells.Clear();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
        }

        /// <summary>
        /// Readies buffers for thing rendering, only needs to be done on startup
        /// </summary>
        public void setupThingRendering()
        {
            arrowVBOID = GL.GenBuffer();
            bodyVBOID = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, arrowVBOID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * arrowVBO.Length), arrowVBO, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bodyVBOID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * levelVBO.Length), levelVBO, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void setupTriggerRendering()
        {
            triggersVBOID = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.triggersVBOID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * triggersVBO.Length), triggersVBO, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void setupLineRendering()
        {
            lineVBOID = GL.GenBuffer();
        }

        public void setupThingUniforms(int program)
        {
            GL.UseProgram(program);

            uniformsThingRender.panUL = GL.GetUniformLocation(program, "pan");
            uniformsThingRender.zoomUL = GL.GetUniformLocation(program, "zoom");
            uniformsThingRender.thingposUL = GL.GetUniformLocation(program, "thingpos");
            uniformsThingRender.thingradUL = GL.GetUniformLocation(program, "thingrad");
            uniformsThingRender.thingcolorUL = GL.GetUniformLocation(program, "thingColor");
            uniformsThingRender.projectUL = GL.GetUniformLocation(program, "project");
            uniformsThingRender.rotateUL = GL.GetUniformLocation(program, "rotate");
            uniformsThingRender.mapsizeUL = GL.GetUniformLocation(program, "mapsize");

            GL.UseProgram(0);
        }

        public void drawThing(Thing thing, Level level, int program, OpenTK.Vector2 winsize)
        {
            //GL.UseProgram(program);
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("THING ENTRY DRAW GL Error: {0}", error.ToString());
            }

            ThingDefinition thingdef = level.GetThingDef(thing);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("THING UNIFORM FIND DRAW GL Error {0}", error.ToString());
            }

            GL.Uniform2(uniformsThingRender.panUL, pan);
            GL.Uniform1(uniformsThingRender.zoomUL, zoom);
            GL.Uniform2(uniformsThingRender.thingposUL, new OpenTK.Vector2(thing.x, thing.y));
            GL.Uniform1(uniformsThingRender.thingradUL, (float)thingdef.radius);
            if (thing.highlighted)
                GL.Uniform4(uniformsThingRender.thingcolorUL, new OpenTK.Vector4((thingdef.r + 128) / 255f, (thingdef.g + 128) / 255f, thingdef.b / 255f, 1.0f));
            else
                GL.Uniform4(uniformsThingRender.thingcolorUL, new OpenTK.Vector4(thingdef.r / 255f, thingdef.g / 255f, thingdef.b / 255f, 1.0f));
            OpenTK.Matrix4 project = OpenTK.Matrix4.CreateOrthographic(winsize.X / 64f / 8f, winsize.Y / 64f / 8f, -8f, 8f);
            GL.UniformMatrix4(uniformsThingRender.projectUL, false, ref project);
            OpenTK.Matrix4 rotate = OpenTK.Matrix4.Identity;
            GL.UniformMatrix4(uniformsThingRender.rotateUL, false, ref rotate);
            GL.Uniform2(uniformsThingRender.mapsizeUL, level.width, level.height);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("THING UNIFORM DRAW GL Error: {0}", error.ToString());
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.bodyVBOID);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("THING BODY DRAW GL Error: {0} {1}", error.ToString(), program);
            }

            rotate = OpenTK.Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(thing.angle));
            GL.UniformMatrix4(uniformsThingRender.rotateUL, false, ref rotate);

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.arrowVBOID);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.Uniform4(uniformsThingRender.thingcolorUL, new OpenTK.Vector4(thingdef.r / 255f / 4f, thingdef.g / 255f / 4f, thingdef.b / 255f / 4f, 1.0f));
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
            GL.DisableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("THING ARROW DRAW GL Error: {0}", error.ToString());
            }

            //GL.UseProgram(0);
        }

        public void drawGrid(Level level, int program, OpenTK.Vector2 winsize)
        {
            GL.UseProgram(program);
            OpenTK.Vector4 color = new OpenTK.Vector4(0.0f, 0.5f, 0.5f, 1.0f);
            for (int y = 0; y < level.height; y++)
            {
                float ly = (float)y / (float)level.height;
                drawLine(new OpenTK.Vector2(0.0f, ly), new OpenTK.Vector2(1.0f, ly), color, level, program, winsize);
            }

            for (int x = 0; x < level.width; x++)
            {
                float lx = (float)x / (float)level.width;
                drawLine(new OpenTK.Vector2(lx, 0.0f), new OpenTK.Vector2(lx, 1.0f), color, level, program, winsize);
            }
            GL.UseProgram(0);
        }

        public void drawLine(OpenTK.Vector2 src, OpenTK.Vector2 dst, OpenTK.Vector4 color, Level level, int program, OpenTK.Vector2 winsize)
        {
            //GL.UseProgram(program);
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("LINE ENTRY DRAW GL Error: {0}", error.ToString());
            }

            lineVBOTemplate[0] = src.X;
            lineVBOTemplate[1] = src.Y;
            lineVBOTemplate[4] = dst.X;
            lineVBOTemplate[5] = dst.Y;

            GL.BindBuffer(BufferTarget.ArrayBuffer, lineVBOID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * lineVBOTemplate.Length), lineVBOTemplate, BufferUsageHint.DynamicDraw);

            int panUL = GL.GetUniformLocation(program, "pan");
            int zoomUL = GL.GetUniformLocation(program, "zoom");
            int thingcolorUL = GL.GetUniformLocation(program, "thingColor");
            int projectUL = GL.GetUniformLocation(program, "project");
            int mapsizeUL = GL.GetUniformLocation(program, "mapsize");

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("LINE UNIFORM FIND DRAW GL Error: {0}", error.ToString());
            }

            GL.Uniform2(panUL, pan);
            GL.Uniform1(zoomUL, zoom);
            GL.Uniform4(thingcolorUL, color);
            OpenTK.Matrix4 project = OpenTK.Matrix4.CreateOrthographic(winsize.X / 64f / 8f, winsize.Y / 64f / 8f, -8f, 8f);
            GL.UniformMatrix4(projectUL, false, ref project);
            GL.Uniform2(mapsizeUL, level.width, level.height);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("LINE UNIFORM DRAW GL Error: {0} {1}", error.ToString(), program);
            }

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
            GL.DisableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("LINE DRAW GL Error: {0}", error.ToString());
            }

            //GL.UseProgram(0);
        }

        public void drawTrigger(OpenTK.Vector2 pos, Level level, int program, OpenTK.Vector2 winsize)
        {
            //GL.UseProgram(program);
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("TRIGGER ENTRY DRAW GL Error: {0}", error.ToString());
            }

            //ThingDefinition thingdef = level.getThingDef(thing);
            int panUL = GL.GetUniformLocation(program, "pan");
            int zoomUL = GL.GetUniformLocation(program, "zoom");
            int thingposUL = GL.GetUniformLocation(program, "thingpos");
            int thingradUL = GL.GetUniformLocation(program, "thingrad");
            int thingcolorUL = GL.GetUniformLocation(program, "thingColor");
            int projectUL = GL.GetUniformLocation(program, "project");
            int rotateUL = GL.GetUniformLocation(program, "rotate");
            int mapsizeUL = GL.GetUniformLocation(program, "mapsize");

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("TRIGGER UNIFORM FIND DRAW GL Error: {0} {1} {2} {3} {4} {5}", error.ToString(), panUL, zoomUL, thingposUL, thingradUL, thingcolorUL);
            }

            GL.Uniform2(panUL, pan);
            GL.Uniform1(zoomUL, zoom);
            GL.Uniform2(thingposUL, new OpenTK.Vector2(pos.X + .5f, pos.Y + .5f));
            GL.Uniform1(thingradUL, (float)32.0f);
            /*if (thing.highlighted)
                GL.Uniform4(thingcolorUL, new OpenTK.Vector4((thingdef.r + 128) / 255f, (thingdef.g + 128) / 255f, thingdef.b / 255f, 1.0f));
            else*/
                GL.Uniform4(thingcolorUL, new OpenTK.Vector4(0f, 1f, 0f, 1.0f));
            OpenTK.Matrix4 project = OpenTK.Matrix4.CreateOrthographic(winsize.X / 64f / 8f, winsize.Y / 64f / 8f, -8f, 8f);
            GL.UniformMatrix4(projectUL, false, ref project);
            OpenTK.Matrix4 rotate = OpenTK.Matrix4.Identity;
            GL.UniformMatrix4(rotateUL, false, ref rotate);
            GL.Uniform2(mapsizeUL, level.width, level.height);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("TRIGGER UNIFORMS DRAW GL Error: {0} {1} {2} {3} {4} {5}", error.ToString(), panUL, zoomUL, thingposUL, thingradUL, thingcolorUL);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, this.triggersVBOID);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Lines, 0, 4);
            GL.DisableVertexAttribArray(0);

            error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("TRIGGER X DRAW GL Error: {0} {1}", error.ToString(), program);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //GL.UseProgram(0);
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
