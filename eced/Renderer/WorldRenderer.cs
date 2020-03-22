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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace eced.Renderer
{
    public class WorldRenderer
    {
        private RendererState state;
        private EditorState editorState;
        private int currentTilemapTexture = 0;
        private int currentViewMode = 1;

        public WorldRenderer(RendererState state)
        {
            this.state = state;
            this.editorState = state.CurrentState;
        }

        public void SetViewMode(int mode)
        {
            currentViewMode = mode;
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

        private int TranslateCharacter(char c)
        {
            //are there any practical .net implementations that don't use unicode BMP codepoints?
            //todo: validation
            if (c >= '0' && c <= '9')
                return c - '0';
            else if (c >= 'A' && c <= 'Z')
                return c - 'A' + 10;
            else if (c >= 'a' && c <= 'z')
                return c - 'a' + 10;

            return 0;
        }

        private void GetColorFromTexture(string name, short[] data, int offset)
        {
            if (name.Length < 7) return;
            data[offset + 1] = (short)((TranslateCharacter(name[1]) << 4) + TranslateCharacter(name[2]));
            data[offset + 2] = (short)((TranslateCharacter(name[3]) << 4) + TranslateCharacter(name[4]));
            data[offset + 3] = (short)((TranslateCharacter(name[5]) << 4) + TranslateCharacter(name[6]));
        }

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public short[] BuildPlaneData(int layer, int xPos, int yPos, int w, int h)
        {
            short[] planeData = new short[w * h * 4];

            int coord;
            string name;
            int mode, type;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    coord = (y * w + x) * 4;
                    if (editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].tile != -1)
                    {
                        type = 1;
                        name = editorState.CurrentLevel.InternalTileset[editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].tile].NorthTex;
                        if (name[0] == '#') //solid color
                        {
                            GetColorFromTexture(name, planeData, coord);
                            mode = 2;
                        }
                        else
                        {
                            planeData[coord+1] = (short)state.Textures.GetTextureID(name);
                            mode = 1;
                        }
                    }
                    else
                    {
                        type = 2;
                        if (currentViewMode == 1)
                        {
                            mode = 0;
                            planeData[coord + 1] = (short)editorState.CurrentLevel.ZoneDefs.IndexOf(editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].zone);
                        }
                        else
                        {
                            if (currentViewMode == 3) name = editorState.CurrentLevel.GetSector(x + xPos, y + yPos, layer).CeilingTexture;
                            else name = editorState.CurrentLevel.GetSector(x + xPos, y + yPos, layer).FloorTexture;

                            if (name[0] == '#') //solid color
                            {
                                GetColorFromTexture(name, planeData, coord);
                                mode = 2;
                            }
                            else
                            {
                                planeData[coord + 1] = (short)state.Textures.GetTextureID(name);
                                mode = 1;
                            }
                        }
                    }
                    planeData[coord] = (short)((mode << 8) + type);
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

        public void DrawLevelGrid()
        {
            Vector4 color = new Vector4(0.0f, 0.5f, 0.5f, 1.0f);
            int w = state.CurrentState.CurrentLevel.Width;
            int h = state.CurrentState.CurrentLevel.Height;

            float coord;
            for (int x = 0; x <= w; x++)
            {
                coord = ((float)x / w) * 2f - 1f;
                state.Drawer.DrawLine(new Vector3(coord, -1.0f, 0), new Vector3(coord, 1.0f, 0), color, color);
            }

            for (int y = 0; y <= h; y++)
            {
                coord = ((float)y / w) * 2f - 1f;
                state.Drawer.DrawLine(new Vector3(-1.0f, coord, 0), new Vector3(1.0f, coord, 0), color, color);
            }
        }

        public void DrawLevel()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, currentTilemapTexture);
            state.TileMapShader.UseShader();
            state.Drawer.DrawTilemap();
            DrawLevelGrid();
            state.Drawer.FlushLines();

            ThingDefinition def;
            foreach (Thing thing in state.CurrentState.CurrentLevel.Things)
            {
                def = state.CurrentState.CurrentLevel.GetThingDef(thing);

                state.Drawer.DrawThingBase(thing, def, 1.0f);
            }
            state.Drawer.FlushThings();
        }
    }
}
