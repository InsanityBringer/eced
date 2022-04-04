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
    public class RendererState
    {
        //float panx = -0.5f, pany = -0.5f;
        public float zoom = 1.0f;
        public Vector2 pan = new OpenTK.Vector2(0.0f, 0.0f);
        public Matrix4 project = Matrix4.Identity;
        public Vector2 screenSize;

        public TextureManager Textures { get; }
        public EditorState CurrentState { get; private set; }

        public Shader TileMapShader { get; private set; }
        public Shader ThingShader { get; private set; }
        public Shader LineShader { get; private set; }
        public Shader BasicTextureShader { get; private set; }

        public RendererDrawer Drawer { get; private set; }

        private int basicUniformBufferNum;
        private float[] basicUniformBufferData = new float[24];
        private bool basicUniformBlockDirty = false;
        const int BASIC_PAN_OFFSET = 0;
        const int BASIC_ZOOM_OFFSET = 2;
        const int BASIC_TILESIZE_OFFSET = 3;
        const int BASIC_PROJECTION_OFFSET = 4;
        const int BASIC_MAPSIZE_OFFSET = 20;

        public RendererState(EditorState editorState)
        {
            CurrentState = editorState;
            Textures = new TextureManager(this);
        }

        public void Init()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            CreateShaderPrograms();
            Drawer = new RendererDrawer(this);
            Drawer.Init();
        }

        private void CreateShaderPrograms()
        {
            TileMapShader = new Shader("tileMapShader");
            TileMapShader.Init();
            TileMapShader.AddShader("./Resources/VertexPanTexture.txt", ShaderType.VertexShader);
            TileMapShader.AddShader("./Resources/FragTextureAtlas.txt", ShaderType.FragmentShader);
            TileMapShader.LinkShader();
            TileMapShader.AddUniformBlock("BasicBlock"); //The binding doesn't need to be updated here, since it's bound to 0 by default.
            TileMapShader.AddUniform("atlas");
            TileMapShader.AddUniform("numbers");
            TileMapShader.AddUniform("texInfo");
            TileMapShader.AddUniform("mapPlane");

            ThingShader = new Shader("thingShader");
            ThingShader.Init();
            ThingShader.AddShader("./Resources/VertexPanThing.txt", ShaderType.VertexShader);
            ThingShader.AddShader("./Resources/FragThing.txt", ShaderType.FragmentShader);
            ThingShader.LinkShader();
            TileMapShader.AddUniformBlock("BasicBlock");

            LineShader = new Shader("lineShader");
            LineShader.Init();
            LineShader.AddShader("./Resources/VertexPanBasic.txt", ShaderType.VertexShader);
            LineShader.AddShader("./Resources/FragColor.txt", ShaderType.FragmentShader);
            LineShader.LinkShader();
            TileMapShader.AddUniformBlock("BasicBlock");

            basicUniformBufferNum = GL.GenBuffer();
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, basicUniformBufferNum);
            GL.BufferData(BufferTarget.UniformBuffer, basicUniformBufferData.Length * sizeof(float), basicUniformBufferData, BufferUsageHint.DynamicRead);
            ErrorCheck("Creating basic UBO");
        }

        public void SetViewSize(int w, int h)
        {
            int halfWidth = w / 2;
            int halfHeight = h / 2;
            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(-halfWidth, halfWidth, halfHeight, -halfHeight, -16, 16);
            int i = 0;
            for (int v = 0; v < 4; v++)
            {
                for (int u = 0; u < 4; u++)
                {
                    basicUniformBufferData[BASIC_PROJECTION_OFFSET + i++] = projectionMatrix[u, v];
                }
            }

            basicUniformBlockDirty = true;

            screenSize.X = w; screenSize.Y = h;
        }

        public void SetGLViewport()
        {
            GL.Viewport(0, 0, (int)screenSize.X, (int)screenSize.Y);
        }

        /// <summary>
        /// Call when loading a new level to inform the shaders of the level's attributes.
        /// </summary>
        /// <param name="level">The level to get the attribute sfrom.</param>
        public void SetLevelStaticUniforms(Level level)
        {
            basicUniformBufferData[BASIC_TILESIZE_OFFSET] = level.TileSize;
            basicUniformBufferData[BASIC_MAPSIZE_OFFSET] = level.Width;
            basicUniformBufferData[BASIC_MAPSIZE_OFFSET + 1] = level.Height;
            basicUniformBlockDirty = true;
            ErrorCheck("RendererState::SetLevelStaticUniforms: Setting level uniforms");

            SetTilemapStaticUniforms();
        }

        public void SetPan(OpenTK.Vector2 newPan)
        {
            this.pan = newPan;
            basicUniformBufferData[BASIC_PAN_OFFSET] = newPan.X;
            basicUniformBufferData[BASIC_PAN_OFFSET + 1] = newPan.Y;
            basicUniformBlockDirty = true;
            ErrorCheck("RendererState::SetPan: Setting pan");
        }

        public void AddPan(int x, int y)
        {
            SetPan(pan + new Vector2(x, y));
        }

        public void SetZoom(float zoom, int mx, int my)
        {
            mx -= (int)(screenSize.X / 2); my -= (int)(screenSize.Y / 2);
            mx -= (int)pan.X; my -= (int)pan.Y;
            int panOffX = (int)(mx * (zoom / this.zoom)) - mx;
            int panOffY = (int)(my * (zoom / this.zoom)) - my;
            AddPan(-panOffX, -panOffY);
            this.zoom = zoom;
            basicUniformBufferData[BASIC_ZOOM_OFFSET] = zoom;
            basicUniformBlockDirty = true;
            ErrorCheck("RendererState::SetZoom: Setting zoom");
        }

        public void SetTilemapStaticUniforms()
        {
            TileMapShader.UseShader();
            GL.Uniform1(TileMapShader.UniformLocations["mapPlane"], 4);
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

        public void CheckUniformsDirty()
        {
            if (basicUniformBlockDirty)
            {
                GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, basicUniformBufferData.Length * sizeof(float), basicUniformBufferData);
                basicUniformBlockDirty = false;
                ErrorCheck("Updating dirty basic uniform block");
            }
        }

        public PickResult Pick(int mousex, int mousey)
        {
            PickResult res = new PickResult();

            float halfScreenWidth = screenSize.X / 2;
            float halfScreenHeight = screenSize.Y / 2;
            float mouseCoordX = mousex - halfScreenWidth - (pan.X);
            float mouseCoordY = mousey - halfScreenHeight - (pan.Y);

            float halfTileWidth = CurrentState.CurrentLevel.Width;
            float halfTileHeight = CurrentState.CurrentLevel.Height;
            float rightBound = 1.0f * halfTileWidth * CurrentState.CurrentLevel.TileSize * zoom;
            float upperBound = 1.0f * halfTileHeight * CurrentState.CurrentLevel.TileSize * zoom;

            float xTile = mouseCoordX / rightBound;
            float yTile = mouseCoordY / upperBound;

            xTile *= CurrentState.CurrentLevel.Width;
            yTile *= CurrentState.CurrentLevel.Height;

            res.x = (int)xTile;
            res.y = (int)yTile;
            //needs to be a cleaner way of doing this...
            res.xf = xTile - (int)xTile;
            res.yf = yTile - (int)yTile;

            res.z = CurrentState.ActiveLayer;

            return res;
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
