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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace eced
{
    class TextureManager
    {
        public static Dictionary<String, int> textureList = new Dictionary<string, int>();

        public static int LookUpTextureID(String filename)
        {
            if (textureList.ContainsKey(filename))
            {
                return textureList[filename];
            }
            return -1;
        }

        public static int getTexture(String filename)
        {
            return getTexture(filename, false);
        }

        public static int getTexture(String filename, bool linear)
        {
            if (textureList.ContainsKey(filename))
            {
                int returnVal = textureList[filename];
                GL.BindTexture(TextureTarget.Texture2D, returnVal);
                return returnVal;
            }

            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Console.WriteLine("Loading texture {0} from file {1}", id, filename);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            bmp.Dispose();

            int FilterSParam = (int)TextureMinFilter.Linear;
            int FilterMParam = (int)TextureMagFilter.Linear;

            if (!linear)
            {
                FilterSParam = (int)TextureMinFilter.Nearest;
                FilterMParam = (int)TextureMagFilter.Nearest;
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, FilterSParam);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, FilterMParam);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            textureList.Add(filename, id);

            return id;
        }
    }
}
