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

        private void GetColorFromTexture(string name, ushort[] data, int offset)
        {
            if (name.Length < 7) return;
            data[offset + 1] = (ushort)((TranslateCharacter(name[1]) << 4) + TranslateCharacter(name[2]));
            data[offset + 1] += (ushort)((TranslateCharacter(name[3]) << 12) + (TranslateCharacter(name[4]) << 8));
            data[offset + 2] = (ushort)((TranslateCharacter(name[5]) << 4) + TranslateCharacter(name[6]));
        }

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public ushort[] BuildPlaneData(int layer, int xPos, int yPos, int w, int h)
        {
            ushort[] planeData = new ushort[w * h * 4];

            int coord;
            string name;
            int mode, type;
            Sector sector;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    coord = (y * w + x) * 4;
                    if (editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].tile != -1)
                    {
                        type = 1;
                        name = editorState.CurrentLevel.Tileset[editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].tile].NorthTex;
                        if (name[0] == '#') //solid color
                        {
                            GetColorFromTexture(name, planeData, coord);
                            mode = 2;
                        }
                        else
                        {
                            planeData[coord+1] = (ushort)state.Textures.GetTextureID(name);
                            mode = 1;
                        }
                    }
                    else
                    {
                        type = 2;
                        if (currentViewMode == 1)
                        {
                            mode = 0;
                            planeData[coord + 1] = (ushort)editorState.CurrentLevel.Planes[layer].cells[x + xPos, y + yPos].zone;
                        }
                        else
                        {
                            sector = editorState.CurrentLevel.GetSector(x + xPos, y + yPos, layer);
                            if (currentViewMode == 3) name = sector.CeilingTexture;
                            else name = sector.FloorTexture;

                            if (name[0] == '#') //solid color
                            {
                                GetColorFromTexture(name, planeData, coord);
                                mode = 2;
                            }
                            else
                            {
                                planeData[coord + 1] = (ushort)state.Textures.GetTextureID(name);
                                mode = 1;
                            }
                            planeData[coord + 3] = (ushort)sector.Light;
                        }
                    }
                    planeData[coord] = (ushort)((mode << 8) + type);
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

        private void UpdateTilemapRegion(int textureID, int x, int y, int w, int h, ushort[] data)
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
            Vector4 color = new Vector4(0.3f, 0.4f, 0.5f, 1.0f);
            int w = state.CurrentState.CurrentLevel.Width;
            int h = state.CurrentState.CurrentLevel.Height;

            float coord;
            for (int x = 0; x <= w; x++)
            {
                coord = (float)x;
                state.Drawer.DrawLine(new Vector3(coord, 0, 0), new Vector3(coord, h, 0), color, color);
            }

            for (int y = 0; y <= h; y++)
            {
                coord = (float)y;
                state.Drawer.DrawLine(new Vector3(0, coord, 0), new Vector3(w, coord, 0), color, color);
            }
        }

        public void DrawTrigger(TriggerList triggerList)
        {
            int triggerActivation = 0;
            int tilesize = state.CurrentState.CurrentLevel.TileSize;
            Vector4 color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            Vector3 src = new Vector3(), dest = new Vector3();
            //i've made some mistakes with coordinate specification...
            float baseX = (float)triggerList.pos.x;
            float baseY = (float)triggerList.pos.y;
            foreach (Trigger trigger in triggerList.Triggers)
            {
                if (trigger.actn) triggerActivation |= 1 | 4;
                if (trigger.acts) triggerActivation |= 1 | 16;
                if (trigger.acte) triggerActivation |= 2 | 8;
                if (trigger.actw) triggerActivation |= 2 | 32;
            }

            if ((triggerActivation & 3) == 3) //activatable in more than one axis
            {
                src.X = baseX + 0.2f;
                src.Y = baseY + 0.2f;
                dest.X = baseX + 0.8f;
                dest.Y = baseY + 0.2f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.Y = baseY + 0.8f;
                dest.Y = baseY + 0.8f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + 0.2f;
                src.Y = baseY + 0.2f;
                dest.X = baseX + 0.2f;
                dest.Y = baseY + 0.8f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + 0.8f;
                dest.X = baseX + 0.8f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + .5f;
                src.Y = baseY + (((triggerActivation & 4) != 0) ? 0.0f : .2f);
                dest.X = baseX + .5f;
                dest.Y = baseY + (((triggerActivation & 16) != 0) ? 1.0f : .8f);
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + (((triggerActivation & 32) != 0) ? 0.0f : .2f);
                src.Y = baseY + .5f;
                dest.X = baseX + (((triggerActivation & 8) != 0) ? 1.0f : .8f);
                dest.Y = baseY + .5f;
                state.Drawer.DrawLine(src, dest, color, color);
            }
            else if ((triggerActivation & 1) != 0)
            {
                src.X = baseX;
                src.Y = baseY;
                dest.X = baseX;
                dest.Y = baseY + 1f;
                state.Drawer.DrawLine(src, dest, color, color);
                src.X = baseX + 1f;
                dest.X = baseX + 1f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX;
                src.Y = baseY + 0.5f;
                dest.X = baseX + 1f;
                dest.Y = baseY + 0.5f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + .5f;
                src.Y = baseY + (((triggerActivation & 4) != 0) ? .25f : .5f);
                dest.X = baseX + .5f;
                dest.Y = baseY + (((triggerActivation & 16) != 0) ? .75f : .5f);
                state.Drawer.DrawLine(src, dest, color, color);
            }
            else
            {
                src.X = baseX;
                src.Y = baseY;
                dest.X = baseX + 1f;
                dest.Y = baseY;
                state.Drawer.DrawLine(src, dest, color, color);
                src.Y = baseY + 1f;
                dest.Y = baseY + 1f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + 0.5f;
                src.Y = baseY;
                dest.X = baseX + 0.5f;
                dest.Y = baseY + 1f;
                state.Drawer.DrawLine(src, dest, color, color);

                src.X = baseX + (((triggerActivation & 32) != 0) ? .25f : .5f);
                src.Y = baseY + .5f;
                dest.X = baseX + (((triggerActivation & 8) != 0) ? .75f : .5f);
                dest.Y = baseY + .5f;
                state.Drawer.DrawLine(src, dest, color, color);
            }
        }

        public void DrawLevel()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, currentTilemapTexture);
            UpdateLevel();
            state.TileMapShader.UseShader();
            state.Drawer.DrawTilemap();
            DrawLevelGrid();
            state.Drawer.FlushLines();
            GL.LineWidth(3);
            foreach (TriggerList triggerList in state.CurrentState.CurrentLevel.Triggers)
            {
                DrawTrigger(triggerList);
            }
            state.Drawer.FlushLines();
            GL.LineWidth(1);

            float alpha = 0.7f;
            if (state.CurrentState.IsThingMode)
                alpha = 1.0f;
            ThingDefinition def;
            foreach (Thing thing in state.CurrentState.CurrentLevel.Things)
            {
                def = state.CurrentState.CurrentLevel.GetThingDef(thing);

                state.Drawer.DrawThingBase(thing, def, alpha);
            }
            state.Drawer.FlushThings();
        }
    }
}
