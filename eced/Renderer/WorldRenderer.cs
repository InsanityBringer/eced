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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace eced.Renderer
{
    public class WorldRenderer
    {
        private RendererState state;
        private EditorState editorState;
        private int currentTilemapTexture = 0;
        public WorldRenderer(RendererState state)
        {
            this.state = state;
            this.editorState = state.CurrentState;
        }
        public void LevelChanged()
        {
            state.SetLevelStaticUniforms(editorState.CurrentLevel);
            SetCurrentLayer(0);
        }

        public void SetCurrentLayer(int layer)
        {
            if (editorState.CurrentLevel == null) return;
            if (layer < 0 || layer >= editorState.CurrentLevel.Depth) return;

            if (currentTilemapTexture != 0) GL.DeleteTexture(currentTilemapTexture);

            currentTilemapTexture = CreateTilemapTexture(editorState.CurrentLevel.Width, editorState.CurrentLevel.Height);

            UpdateTilemapRegion(currentTilemapTexture, 0, 0, editorState.CurrentLevel.Width, editorState.CurrentLevel.Height, BuildPlaneData(layer, 0, 0, editorState.CurrentLevel.Width, editorState.CurrentLevel.Height));
        }

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public short[] BuildPlaneData(int layer, int xPos, int yPos, int w, int h)
        {
            short[] planeData = new short[w * h * 4];
            Random random = new Random();

            int coord;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    coord = (y * w + x) * 4;
                    //planeData[(x * width + y) * 4] = (short)planes[layer].cells[x, y].tile.id;
                    if (editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].tile != null)
                        planeData[coord] = (short)state.Textures.GetTextureID(editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].tile.NorthTex);
                    else
                    {
                        planeData[coord] = -1;
                        planeData[coord + 1] = (short)editorState.CurrentLevel.ZoneDefs.IndexOf(editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].zone);
                    }
                    //planeData[coord] = (short)random.Next(0, 40);
                }
            }

            return planeData;
        }

        private int CreateTilemapTexture(int w, int h)
        {
            //short[] mapTexture = BuildPlaneData(0);

            //int[] tids = new int[4]; GL.GenTextures(4, tids);
            int worldTextureID = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, worldTextureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, w, h, 0, PixelFormat.RgbaInteger, PixelType.Short, (IntPtr)0);
            //GL.BindTexture(TextureTarget.Texture2D, 0);
            //GL.ActiveTexture(TextureUnit.Texture0);

            RendererState.ErrorCheck("WorldRenderer::CreateTilemapTexture: Creating tilemap texture");

            return worldTextureID;
        }

        private void UpdateTilemapRegion(int textureID, int x, int y, int w, int h, short[] data)
        {
            Console.WriteLine("pushing region {0} {1}, {2} {3}", x, y, w, h);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, x, y, w, h, PixelFormat.RgbaInteger, PixelType.Short, data);
        }

        public void UpdateLevel()
        {
            if (editorState.CurrentLevel.Dirty)
            {
                DirtyRectangle rect = editorState.CurrentLevel.dirtyRectangle;
                int w = rect.x2 - rect.x1 + 1; int h = rect.y2 - rect.y1 + 1;
                UpdateTilemapRegion(currentTilemapTexture, rect.x1, rect.y1, w, h, BuildPlaneData(0, rect.x1, rect.y1, w, h));
                editorState.CurrentLevel.ClearDirty();
            }
        }

        public void DrawLevel()
        {
            Random random = new Random(1);
            for (int i = 0; i < 100; i++)
            {
                state.Drawer.DrawLine(new Vector3((float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0), 0),
                    new Vector3((float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0), 0),
                    new Vector4((float)(random.NextDouble()), (float)(random.NextDouble()), (float)(random.NextDouble()), 1.0f),
                    new Vector4((float)(random.NextDouble()), (float)(random.NextDouble()), (float)(random.NextDouble()), 1.0f));
            }
            state.Drawer.FlushLines();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, currentTilemapTexture);
            state.TileMapShader.UseShader();
            state.Drawer.DrawTilemap();
        }

        public Vector2 PickOrtho(int mousex, int mousey)
        {
            if (mousex < 0 || mousey < 0 || mousex >= (int)state.screenSize.X || mousey >= (int)state.screenSize.Y) return new Vector2(32767, 32767);
            state.PickFB.Use();
            short[] buffer = new short[2];
            state.TileMapPickShader.UseShader();
            state.Drawer.DrawTilemap();
            GL.Finish(); //godawful hack
            RendererState.ErrorCheck("WorldRenderer::PickOrtho: drawing to buffer");
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
            //GL.PixelStore(PixelStoreParameter.PackAlignment, 1);
            GL.ReadPixels(mousex, mousey, 1, 1, PixelFormat.RgInteger, PixelType.Short, buffer);
            RendererState.ErrorCheck("WorldRenderer::PickOrtho: reading pixels");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            RendererState.ErrorCheck("WorldRenderer::PickOrtho: unbinding");

            return new Vector2(buffer[0], buffer[1]);
        }
    }
}
