/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
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
    public enum VAOInidices
    {
        Tilemap = 0,
        Thing,
        LineBuffer,
        BasicTexture,
        NumVAOs
    }

    public enum DrawMode
    {
        Lines,
        Other
    }
    public class RendererDrawer
    {
        private const int NUM_LINE_POINTS = 250;
        private const int NUM_THING_POINTS = 150;
        private RendererState state;
        //Drawing VAO Names
        private int[] vaoNames = new int[(int)VAOInidices.NumVAOs];
        //Line data
        private int lineBufferName;
        private float[] lineBuffer = new float[NUM_LINE_POINTS * 8];
        private int lastLinePoint;
        //Tilemap data
        private int tilemapBufferName;
        private float[] tilemapBuffer = new float[6 * 6];
        //Thing data
        private int thingBufferName;
        private float[] thingBuffer = new float[12 * NUM_THING_POINTS];
        private int lastThingNum;
        private int thingArrowTexture;
        //Basic texture data
        private int basicBufferName;

        public RendererDrawer(RendererState state)
        {
            this.state = state;
        }

        public void Init()
        {
            GL.GenVertexArrays((int)VAOInidices.NumVAOs, vaoNames);
            RendererState.ErrorCheck("RendererDrawer::Init: Creating vertex array objects");

            InitLineBuffer();
            InitTilemapBuffer();
            InitThingBuffer();
            InitThingTextures();
            InitBasicBuffer();
        }

        public void InitLineBuffer()
        {
            //Lines are usually drawn in good quantities, so try to buffer lots at once
            lineBufferName = GL.GenBuffer();
            GL.BindVertexArray(vaoNames[(int)VAOInidices.LineBuffer]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, lineBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, NUM_LINE_POINTS * sizeof(float) * 8, (IntPtr)0, BufferUsageHint.DynamicDraw);
            RendererState.ErrorCheck("RendererDrawer::InitLineBuffer: Creating line vertex buffer");

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 4);
            RendererState.ErrorCheck("RendererDrawer::InitLineBuffer: Creating line vertex attributes");

        }

        public void FlushLines()
        {
            if (lastLinePoint == 0) return;
            state.LineShader.UseShader();

            GL.BindVertexArray(vaoNames[(int)VAOInidices.LineBuffer]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, lineBufferName); //need to bind the buffer to upload the data

            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, sizeof(float) * 8 * lastLinePoint, lineBuffer);
            RendererState.ErrorCheck("RendererDrawer::FlushLines: Creating line VBO");
            GL.DrawArrays(PrimitiveType.Lines, 0, lastLinePoint);
            RendererState.ErrorCheck("RendererDrawer::FlushLines: Drawing lines");

            lastLinePoint = 0;
        }

        public void DrawLine(Vector3 src, Vector3 dest, Vector4 srcColor, Vector4 dstColor)
        {
            if (lastLinePoint == NUM_LINE_POINTS)
                FlushLines();

            lineBuffer[lastLinePoint * 8 + 0] = src.X;
            lineBuffer[lastLinePoint * 8 + 1] = src.Y;
            lineBuffer[lastLinePoint * 8 + 2] = src.Z;
            lineBuffer[lastLinePoint * 8 + 3] = 1.0f;
            lineBuffer[lastLinePoint * 8 + 4] = srcColor.X;
            lineBuffer[lastLinePoint * 8 + 5] = srcColor.Y;
            lineBuffer[lastLinePoint * 8 + 6] = srcColor.Z;
            lineBuffer[lastLinePoint * 8 + 7] = srcColor.W;
            lastLinePoint++;
            lineBuffer[lastLinePoint * 8 + 0] = dest.X;
            lineBuffer[lastLinePoint * 8 + 1] = dest.Y;
            lineBuffer[lastLinePoint * 8 + 2] = dest.Z;
            lineBuffer[lastLinePoint * 8 + 3] = 1.0f;
            lineBuffer[lastLinePoint * 8 + 4] = dstColor.X;
            lineBuffer[lastLinePoint * 8 + 5] = dstColor.Y;
            lineBuffer[lastLinePoint * 8 + 6] = dstColor.Z;
            lineBuffer[lastLinePoint * 8 + 7] = dstColor.W;
            lastLinePoint++;
        }

        private void InitTilemapBuffer()
        {
            tilemapBuffer[0] = -1.0f;
            tilemapBuffer[1] = 1.0f;
            tilemapBuffer[2] = 0f;
            tilemapBuffer[3] = 1.0f;
            tilemapBuffer[4] = 0.0f;
            tilemapBuffer[5] = 1.0f;

            tilemapBuffer[6 + 0] = -1.0f;
            tilemapBuffer[6 + 1] = -1.0f;
            tilemapBuffer[6 + 2] = 0f;
            tilemapBuffer[6 + 3] = 1.0f;
            tilemapBuffer[6 + 4] = 0.0f;
            tilemapBuffer[6 + 5] = 0.0f;

            tilemapBuffer[12 + 0] = 1.0f;
            tilemapBuffer[12 + 1] = -1.0f;
            tilemapBuffer[12 + 2] = 0f;
            tilemapBuffer[12 + 3] = 1.0f;
            tilemapBuffer[12 + 4] = 1.0f;
            tilemapBuffer[12 + 5] = 0.0f;

            tilemapBuffer[18 + 0] = 1.0f;
            tilemapBuffer[18 + 1] = 1.0f;
            tilemapBuffer[18 + 2] = 0f;
            tilemapBuffer[18 + 3] = 1.0f;
            tilemapBuffer[18 + 4] = 1.0f;
            tilemapBuffer[18 + 5] = 1.0f;

            tilemapBufferName = GL.GenBuffer();
            GL.BindVertexArray(vaoNames[(int)VAOInidices.Tilemap]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, tilemapBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * sizeof(float) * 6, tilemapBuffer, BufferUsageHint.StaticDraw);
            RendererState.ErrorCheck("RendererDrawer::InitLineBuffer: Creating tilemap vertex buffer");

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, sizeof(float) * 6, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 6, sizeof(float) * 4);
            RendererState.ErrorCheck("RendererDrawer::InitLineBuffer: Creating tilemap vertex attributes");
        }

        public void DrawTilemap()
        {
            GL.BindVertexArray(vaoNames[(int)VAOInidices.Tilemap]);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
        }

        public void InitThingBuffer()
        {
            thingBufferName = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, thingBufferName);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, 12 * NUM_THING_POINTS * sizeof(float), (IntPtr)0, BufferUsageHint.DynamicRead);

            GL.BindBufferRange(BufferRangeTarget.ShaderStorageBuffer, 0, thingBufferName, (IntPtr)0, sizeof(float) * NUM_THING_POINTS * 12);
            RendererState.ErrorCheck("RendererDrawer::InitThingBuffer: Creating thing shader storage block");

            thingArrowTexture = TextureManager.GetTexture("./Resources/thingarrow.png", false, true);
        }

        public void InitThingTextures()
        {
            int[] buffer = new int[17 * 17 * 8];
            ResourceFiles.Images.BasicImage hack;
            thingArrowTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, thingArrowTexture);
            //hideous hack
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thingarrow.png"));
            Array.Copy(hack.data, 0, buffer, 0, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thingammo.png"));
            Array.Copy(hack.data, 0, buffer, 17*17, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thingweapon.png"));
            Array.Copy(hack.data, 0, buffer, 17 * 17 * 2, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thinghealth.png"));
            Array.Copy(hack.data, 0, buffer, 17 * 17 * 3, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thingkey.png"));
            Array.Copy(hack.data, 0, buffer, 17 * 17 * 4, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thingtreasure.png"));
            Array.Copy(hack.data, 0, buffer, 17 * 17 * 5, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thingpowerup.png"));
            Array.Copy(hack.data, 0, buffer, 17 * 17 * 6, 17 * 17);
            hack = ResourceFiles.Images.PNGCodec.BasicImageFromBitmap(new System.Drawing.Bitmap("./Resources/thinglight.png"));
            Array.Copy(hack.data, 0, buffer, 17 * 17 * 7, 17 * 17);

            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, 17, 17, 8, 0, PixelFormat.Bgra, PixelType.UnsignedByte, buffer);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
        }

        public void FlushThings()
        {
            if (lastThingNum == 0) return;

            state.ThingShader.UseShader();

            GL.BufferSubData<float>(BufferTarget.ShaderStorageBuffer, (IntPtr)0, sizeof(float) * 12 * lastThingNum, thingBuffer);
            RendererState.ErrorCheck("RendererDrawer::FlushThings: Uploading thing buffer data");

            GL.BindVertexArray(vaoNames[(int)VAOInidices.Tilemap]);
            GL.BindTexture(TextureTarget.Texture2DArray, thingArrowTexture);
            GL.DrawArraysInstanced(PrimitiveType.TriangleFan, 0, 4, lastThingNum);
            RendererState.ErrorCheck("RendererDrawer::FlushThings: Drawing things");

            lastThingNum = 0;
        }

        public void DrawThingBase(Thing thing, ThingDefinition def, float alpha)
        {
            if (lastThingNum == NUM_THING_POINTS)
                FlushThings();

            thingBuffer[lastThingNum * 12 + 0] = thing.x;
            thingBuffer[lastThingNum * 12 + 1] = thing.y;
            thingBuffer[lastThingNum * 12 + 2] = thing.z;
            thingBuffer[lastThingNum * 12 + 3] = 0f;

            if (thing.highlighted)
            {
                thingBuffer[lastThingNum * 12 + 4] = (def.R + 64) / 255f;
                thingBuffer[lastThingNum * 12 + 5] = (def.G + 64) / 255f;
                thingBuffer[lastThingNum * 12 + 6] = def.B / 255f;
            }
            else if (thing.selected)
            {
                thingBuffer[lastThingNum * 12 + 4] = def.R / 512f;
                thingBuffer[lastThingNum * 12 + 5] = (def.G / 2f + 128) / 255f;
                thingBuffer[lastThingNum * 12 + 6] = (def.B / 2f + 128) / 255f;
            }
            else
            {
                thingBuffer[lastThingNum * 12 + 4] = def.R / 255f;
                thingBuffer[lastThingNum * 12 + 5] = def.G / 255f;
                thingBuffer[lastThingNum * 12 + 6] = def.B / 255f;
            }
            thingBuffer[lastThingNum * 12 + 7] = alpha;

            thingBuffer[lastThingNum * 12 + 8] = def.Radius;
            thingBuffer[lastThingNum * 12 + 9] = thing.angle;
            thingBuffer[lastThingNum * 12 + 10] = (float)def.Icon;

            lastThingNum++;
        }

        private void InitBasicBuffer()
        {
            tilemapBuffer[0] = 0.0f;
            tilemapBuffer[1] = 1.0f;
            tilemapBuffer[2] = 0f;
            tilemapBuffer[3] = 1.0f;
            tilemapBuffer[4] = 0.0f;
            tilemapBuffer[5] = 1.0f;

            tilemapBuffer[6 + 0] = 0.0f;
            tilemapBuffer[6 + 1] = 0.0f;
            tilemapBuffer[6 + 2] = 0f;
            tilemapBuffer[6 + 3] = 1.0f;
            tilemapBuffer[6 + 4] = 0.0f;
            tilemapBuffer[6 + 5] = 0.0f;

            tilemapBuffer[12 + 0] = 1.0f;
            tilemapBuffer[12 + 1] = 0.0f;
            tilemapBuffer[12 + 2] = 0f;
            tilemapBuffer[12 + 3] = 1.0f;
            tilemapBuffer[12 + 4] = 1.0f;
            tilemapBuffer[12 + 5] = 0.0f;

            tilemapBuffer[18 + 0] = 1.0f;
            tilemapBuffer[18 + 1] = 1.0f;
            tilemapBuffer[18 + 2] = 0f;
            tilemapBuffer[18 + 3] = 1.0f;
            tilemapBuffer[18 + 4] = 1.0f;
            tilemapBuffer[18 + 5] = 1.0f;

            basicBufferName = GL.GenBuffer();
            GL.BindVertexArray(vaoNames[(int)VAOInidices.BasicTexture]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, basicBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * sizeof(float) * 6, tilemapBuffer, BufferUsageHint.StaticDraw);
            RendererState.ErrorCheck("RendererDrawer::InitLineBuffer: Creating tilemap vertex buffer");

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, sizeof(float) * 6, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 6, sizeof(float) * 4);
            RendererState.ErrorCheck("RendererDrawer::InitLineBuffer: Creating tilemap vertex attributes");
        }
    }
}
