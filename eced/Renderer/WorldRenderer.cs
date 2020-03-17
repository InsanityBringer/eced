using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            currentTilemapTexture = CreateTilemapTexture(editorState.CurrentLevel.Width, editorState.CurrentLevel.Height, BuildPlaneData(layer));
        }

        /// <summary>
        /// Builds a 2-dimensional array representing the data of a single plane
        /// </summary>
        /// <param name="layer">The layer to make the plane from</param>
        public short[] BuildPlaneData(int layer)
        {
            short[] planeData = new short[editorState.CurrentLevel.Width * editorState.CurrentLevel.Height * 4];
            Random random = new Random();

            for (int x = 0; x < editorState.CurrentLevel.Width; x++)
            {
                for (int y = 0; y < editorState.CurrentLevel.Height; y++)
                {
                    //planeData[(x * width + y) * 4] = (short)planes[layer].cells[x, y].tile.id;
                    if (editorState.CurrentLevel.Planes[layer].cells[x, y].tile != null)
                        planeData[(y * editorState.CurrentLevel.Width + x) * 4] = (short)state.Textures.getTextureID(editorState.CurrentLevel.Planes[layer].cells[x, y].tile.NorthTex);
                    else planeData[(y * editorState.CurrentLevel.Width + x) * 4] = -1;

                    planeData[(y * editorState.CurrentLevel.Width + x) * 4] = (short)random.Next(0, 40);

                    if (editorState.CurrentLevel.Planes[layer].cells[x, y].tile == null)
                        planeData[(y * editorState.CurrentLevel.Width + x) * 4 + 1] = (short)editorState.CurrentLevel.ZoneDefs.IndexOf(editorState.CurrentLevel.Planes[layer].cells[x, y].zone);
                }
            }

            return planeData;
        }

        private int CreateTilemapTexture(int w, int h, short[] data)
        {
            //short[] mapTexture = BuildPlaneData(0);

            //int[] tids = new int[4]; GL.GenTextures(4, tids);
            GL.ActiveTexture(TextureUnit.Texture0);
            int worldTextureID = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, worldTextureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, w, h, 0, PixelFormat.RgbaInteger, PixelType.Short, data);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            //GL.ActiveTexture(TextureUnit.Texture0);

            RendererState.ErrorCheck("WorldRenderer::CreateTilemapTexture: Creating tilemap texture");

            return worldTextureID;
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
            state.Drawer.DrawTilemap();
        }
    }
}
