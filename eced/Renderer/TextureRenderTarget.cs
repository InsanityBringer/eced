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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace eced.Renderer
{
    public class TextureRenderTarget
    {
        private int targetTexture, fboID;
        private Shader renderShader;
        private RendererState state;
        public TextureRenderTarget(RendererState state)
        {
            this.state = state;

            fboID = GL.GenFramebuffer();
            targetTexture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, targetTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 128, 128, 0, PixelFormat.Bgra, PixelType.UnsignedByte, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            RendererState.ErrorCheck("TextureRenderTarget: Creating Rendertarget texture");

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboID);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, targetTexture, 0);
            RendererState.ErrorCheck("TextureRenderTarget: Creating Rendertarget framebuffer");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            renderShader = new Shader("TextureRenderTarget");
            renderShader.Init();
            renderShader.AddShader("./Resources/VertexBasicTexture.txt", ShaderType.VertexShader);
            renderShader.AddShader("./Resources/FragBasicTexture.txt", ShaderType.FragmentShader);
            renderShader.LinkShader();
            renderShader.AddUniform("textureNum");
            renderShader.AddUniform("texInfo");
            renderShader.AddUniform("atlas");
            renderShader.UseShader();

            GL.Uniform1(renderShader.UniformLocations["atlas"], 1);
            GL.Uniform1(renderShader.UniformLocations["texInfo"], 2);
            RendererState.ErrorCheck("TextureRenderTarget: Setting Rendertarget shader uniforms");
        }

        public byte[] RenderTexture(int id)
        {
            byte[] output = new byte[128 * 128 * 4];
            //TODO: This is a lot of state changes in a short time, done repeatedly. Needs optimization.
            //For the moment it shouldn't be too bad since its cached, but it is a problem when first going through. 
            renderShader.UseShader();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboID);
            GL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Uniform1(renderShader.UniformLocations["textureNum"], id);
            GL.Viewport(0, 0, 128, 128);
            RendererState.ErrorCheck("TextureRenderTarget::RenderTarget: Setting up state");

            state.Drawer.DrawTilemap();
            //The worst synchronization primitive
            //(actually given how bad amd's drivers are does this do anything on them)
            GL.Finish();
            GL.ReadPixels(0, 0, 128, 128, PixelFormat.Bgra, PixelType.UnsignedByte, output);
            RendererState.ErrorCheck("TextureRenderTarget::RenderTarget: Reading results");

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            return output;
        }
    }
}
