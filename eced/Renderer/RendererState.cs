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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace eced.Renderer
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

    public class RendererState
    {
        //float panx = -0.5f, pany = -0.5f;
        public float zoom = 1.0f;
        public Vector2 pan = new OpenTK.Vector2(0.0f, 0.0f);
        public Matrix4 project = Matrix4.Identity;
        public Vector2 screenSize;

        BasicRenderUniforms uniformsBasicRender = new BasicRenderUniforms();
        BasicThingUniforms uniformsThingRender = new BasicThingUniforms();

        public TextureManager Textures { get; }
        public EditorState CurrentState { get; private set; }

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

        public Shader TileMapShader { get; private set; }
        public Shader ThingShader { get; private set; }
        public Shader LineShader { get; private set; }
        public Shader TileMapPickShader { get; private set; }

        public RendererDrawer Drawer { get; private set; }

        public PickBuffer PickFB { get; private set; }

        public RendererState(EditorState editorState)
        {
            CurrentState = editorState;
            Textures = new TextureManager(this);
        }

        public void Init()
        {
            CreateShaderPrograms();
            Drawer = new RendererDrawer(this);
            Drawer.Init();
            PickFB = new PickBuffer();
        }

        private void CreateShaderPrograms()
        {
            TileMapShader = new Shader("tileMapShader");
            TileMapShader.Init();
            TileMapShader.AddShader("./resources/VertexPanTexture.txt", ShaderType.VertexShader);
            TileMapShader.AddShader("./resources/FragPanTextureAtlas.txt", ShaderType.FragmentShader);
            TileMapShader.LinkShader();
            TileMapShader.AddUniform("pan");
            TileMapShader.AddUniform("zoom");
            TileMapShader.AddUniform("tilesize");
            TileMapShader.AddUniform("project");
            TileMapShader.AddUniform("mapsize");
            TileMapShader.AddUniform("atlas");
            TileMapShader.AddUniform("numbers");
            TileMapShader.AddUniform("texInfo");
            TileMapShader.AddUniform("mapPlane");

            TileMapPickShader = new Shader("tileMapPickShader");
            TileMapPickShader.Init();
            TileMapPickShader.AddShader("./resources/VertexPanTexture.txt", ShaderType.VertexShader);
            TileMapPickShader.AddShader("./resources/FragPanPick.txt", ShaderType.FragmentShader);
            TileMapPickShader.LinkShader();
            TileMapPickShader.AddUniform("pan");
            TileMapPickShader.AddUniform("zoom");
            TileMapPickShader.AddUniform("tilesize");
            TileMapPickShader.AddUniform("project");
            TileMapPickShader.AddUniform("mapsize");

            ThingShader = new Shader("thingShader");
            ThingShader.Init();
            ThingShader.AddShader("./resources/VertexPanThing.txt", ShaderType.VertexShader);
            ThingShader.AddShader("./resources/FragPanThing.txt", ShaderType.FragmentShader);
            ThingShader.LinkShader();
            ThingShader.AddUniform("pan");
            ThingShader.AddUniform("zoom");
            ThingShader.AddUniform("thingpos");
            ThingShader.AddUniform("thingrad");
            ThingShader.AddUniform("thingColor");
            ThingShader.AddUniform("project");
            ThingShader.AddUniform("rotate");
            ThingShader.AddUniform("mapsize");
            ThingShader.AddUniform("tilesize");

            LineShader = new Shader("lineShader");
            LineShader.Init();
            LineShader.AddShader("./resources/VertexPanBasic.txt", ShaderType.VertexShader);
            LineShader.AddShader("./resources/FragPanThing.txt", ShaderType.FragmentShader);
            LineShader.LinkShader();
            LineShader.AddUniform("pan");
            LineShader.AddUniform("zoom");
            LineShader.AddUniform("thingColor");
            LineShader.AddUniform("project");
            LineShader.AddUniform("mapsize");
            LineShader.AddUniform("tilesize");
        }

        public void SetViewSize(int w, int h)
        {
            int halfWidth = w / 2;
            int halfHeight = h / 2;
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(-halfWidth, halfWidth, -halfHeight, halfHeight, -16, 16);
            GL.Viewport(0, 0, w, h);
            //TODO: These should be set as some sort of "pending" structure that's applied when a shader is bound, instead of binding and setting immediately
            TileMapShader.UseShader();
            GL.UniformMatrix4(TileMapShader.UniformLocations["project"], false, ref projectionMatrix);
            ThingShader.UseShader();
            GL.UniformMatrix4(ThingShader.UniformLocations["project"], false, ref projectionMatrix);
            LineShader.UseShader();
            GL.UniformMatrix4(LineShader.UniformLocations["project"], false, ref projectionMatrix);
            TileMapPickShader.UseShader();
            GL.UniformMatrix4(TileMapPickShader.UniformLocations["project"], false, ref projectionMatrix);
            ErrorCheck("RendererState::SetViewSize: Setting viewport");

            screenSize.X = w; screenSize.Y = h;
            PickFB.Create(w, h);
        }

        /// <summary>
        /// Call when loading a new level to inform the shaders of the level's attributes.
        /// </summary>
        /// <param name="level">The level to get the attribute sfrom.</param>
        public void SetLevelStaticUniforms(Level level)
        {
            TileMapShader.UseShader();
            GL.Uniform1(TileMapShader.UniformLocations["tilesize"], (float)64); //TODO
            GL.Uniform2(TileMapShader.UniformLocations["mapsize"], level.Width, level.Height);
            TileMapPickShader.UseShader();
            GL.Uniform1(TileMapPickShader.UniformLocations["tilesize"], (float)64); //TODO
            GL.Uniform2(TileMapPickShader.UniformLocations["mapsize"], level.Width, level.Height);
            /*ThingShader.UseShader();
            GL.Uniform1(ThingShader.UniformLocations["tilesize"], (float)64); //TODO
            GL.Uniform2(ThingShader.UniformLocations["mapsize"], level.Width, level.Height);*/
            LineShader.UseShader();
            GL.Uniform1(LineShader.UniformLocations["tilesize"], (float)64); //TODO
            GL.Uniform2(LineShader.UniformLocations["mapsize"], level.Width, level.Height);
            ErrorCheck("RendererState::SetLevelStaticUniforms: Setting level uniforms");

            SetTilemapStaticUniforms();
        }

        public void SetPan(OpenTK.Vector2 newPan)
        {
            this.pan = newPan;
            //TODO: These should be set as some sort of "pending" structure that's applied when a shader is bound, instead of binding and setting immediately
            TileMapShader.UseShader();
            GL.Uniform2(TileMapShader.UniformLocations["pan"], ref newPan);
            TileMapPickShader.UseShader();
            GL.Uniform2(TileMapPickShader.UniformLocations["pan"], ref newPan);
            ThingShader.UseShader();
            GL.Uniform2(ThingShader.UniformLocations["pan"], ref newPan);
            LineShader.UseShader();
            GL.Uniform2(LineShader.UniformLocations["pan"], ref newPan);
            ErrorCheck("RendererState::SetPan: Setting pan");
        }

        public void SetZoom(float zoom)
        {
            //TODO: These should be set as some sort of "pending" structure that's applied when a shader is bound, instead of binding and setting immediately
            TileMapShader.UseShader();
            GL.Uniform1(TileMapShader.UniformLocations["zoom"], zoom);
            TileMapPickShader.UseShader();
            GL.Uniform1(TileMapPickShader.UniformLocations["zoom"], zoom);
            ThingShader.UseShader();
            GL.Uniform1(ThingShader.UniformLocations["zoom"], zoom);
            LineShader.UseShader();
            GL.Uniform1(LineShader.UniformLocations["zoom"], zoom);
            ErrorCheck("RendererState::SetZoom: Setting zoom");
        }

        public void SetTilemapStaticUniforms()
        {
            TileMapShader.UseShader();
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, Textures.atlasTextureID);
            GL.Uniform1(TileMapShader.UniformLocations["atlas"], 1);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, Textures.resourceInfoID);
            GL.Uniform1(TileMapShader.UniformLocations["texInfo"], 2);
            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, Textures.numberTextureID);
            GL.Uniform1(TileMapShader.UniformLocations["numbers"], 3);
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
            GL.Uniform2(uniformsThingRender.mapsizeUL, level.Width, level.Height);

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
            GL.Uniform2(mapsizeUL, level.Width, level.Height);

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

        public static void ErrorCheck(string context)
        {
            ErrorCode errorNum = GL.GetError();
            if (errorNum != ErrorCode.NoError)
            {
                Console.WriteLine("Error in context {0}: {1}", context, errorNum);
            }
        }
    }
}
