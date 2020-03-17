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

using OpenTK.Graphics.OpenGL;

namespace eced.Renderer
{
    public class PickBuffer
    {
        int fboID;
        int textureID = -1;
        int w, h;

        public PickBuffer()
        {
            GL.CreateFramebuffers(1, out fboID);
        }
        public void Create(int width, int height)
        {
            w = width; h = height;
            if (textureID != -1)
                GL.DeleteTexture(textureID);
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rg16i, width, height);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            RendererState.ErrorCheck("PickBuffer::Create: creating texture");

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboID);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureID, 0);
            RendererState.ErrorCheck("PickBuffer::Create: attaching framebuffer texture");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Use()
        {
            //int[] pair = { 32767, 32767, 0, 0 };
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboID);
            //GL.ClearBuffer(ClearBuffer.Color, (int)DrawBuffersEnum.ColorAttachment0, pair);
            //GL.ClearColor(0f, 0f, 0f, 0f);
            //GL.Clear(ClearBufferMask.ColorBufferBit);
            RendererState.ErrorCheck("PickBuffer::Use: clearing pick buffer");
        }

        public short[] GetPickBuffer()
        {
            short[] buffer = new short[w * h * 2];
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.RgInteger, PixelType.Short, buffer);
            return buffer;
        }
    }
}
