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
            GL.DrawArrays(PrimitiveType.Lines, 0, lastLinePoint / 2);
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
            tilemapBuffer[5] = 0.0f;

            tilemapBuffer[6 + 0] = -1.0f;
            tilemapBuffer[6 + 1] = -1.0f;
            tilemapBuffer[6 + 2] = 0f;
            tilemapBuffer[6 + 3] = 1.0f;
            tilemapBuffer[6 + 4] = 0.0f;
            tilemapBuffer[6 + 5] = 1.0f;

            tilemapBuffer[12 + 0] = 1.0f;
            tilemapBuffer[12 + 1] = -1.0f;
            tilemapBuffer[12 + 2] = 0f;
            tilemapBuffer[12 + 3] = 1.0f;
            tilemapBuffer[12 + 4] = 1.0f;
            tilemapBuffer[12 + 5] = 1.0f;

            tilemapBuffer[18 + 0] = 1.0f;
            tilemapBuffer[18 + 1] = 1.0f;
            tilemapBuffer[18 + 2] = 0f;
            tilemapBuffer[18 + 3] = 1.0f;
            tilemapBuffer[18 + 4] = 1.0f;
            tilemapBuffer[18 + 5] = 0.0f;

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
    }
}
