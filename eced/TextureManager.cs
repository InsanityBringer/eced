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
    /// <summary>
    /// Represents a single cell on the tilemap texture atlas
    /// </summary>
    struct TextureCell
    {
        public int x, y;
        public int w, h;
    }

    enum TextureFormat
    {
        FORMAT_PNG,
        FORMAT_WOLFWALL,
        FORMAT_UNKNOWN
    }

    /// <summary>
    /// Manages textures, responsible for building tilemap texture atlas and filling in details about each texture
    /// </summary>
    // (rewritten for the first time since 2012)
    class TextureManager
    {
        public static Dictionary<String, int> textureList = new Dictionary<string, int>();

        private byte[] textureAtlasTexture;
        private int[] resourceInfoTexture;

        const int baseAtlasSize = 2048;

        public int atlasTextureID;
        public int resourceInfoID;
        public int lastID = 0;

        public List<TextureCell> cells;
        public Dictionary<String, int> textureIDList = new Dictionary<string, int>();

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
        //atlasing algorthim lifted from Quake II GPL source release
        public int[] allocated;
        
        /// <summary>
        /// Finds the best location to allocate a texture on the atlas
        /// </summary>
        /// <param name="width">Width of the block</param>
        /// <param name="height">Height of the block</param>
        /// <param name="res">A structure indicating where the resultant block is placed</param>
        /// <returns>True if a spot is found, false if no spot is found</returns>
        public bool allocateAtlasSpace(int width, int height, out TextureCell res)
        {
            int i, j;
            int best, best2;

            TextureCell result = new TextureCell();

            best = baseAtlasSize;

            for (i = 0; i < baseAtlasSize - width; i++)
            {
                best2 = 0;
                for (j = 0; j < width; j++)
                {
                    if (allocated[i + j] >= best)
                        break;
                    if (allocated[i + j] > best2)
                        best2 = allocated[i + j];
                }
                if (j == width)
                {
                    Console.WriteLine(".. oh dear! {0} {1}", i, best2);
                    result.x = i;
                    result.y = best = best2;
                }
            }
            result.w = width;
            result.h = height;
            res = result;

            if (best + height > baseAtlasSize)
            {
                Console.WriteLine("could not find location for atlasing");
                return false;
            }

            for (i = 0; i < width; i++)
            {
                //Console.WriteLine("updating allocation");
                allocated[res.x + i] = best + res.h;
            }

            return true;
        }

        /// <summary>
        /// Allocates the texture for the tilemap atlas
        /// </summary>
        public void allocateAtlasTexture()
        {
            allocated = new int[baseAtlasSize];
            atlasTextureID = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);

            byte[] bogusArray = new byte[baseAtlasSize * baseAtlasSize * 4];

            //GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rgba8, baseAtlasSize, baseAtlasSize);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, baseAtlasSize, baseAtlasSize, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, bogusArray);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("ALLOCATE ATLAS GL ERROR: {0}", err.ToString());
            }

            cells = new List<TextureCell>();
        }

        /// <summary>
        /// Uploads a texture to the atlas at the specified cell block
        /// </summary>
        /// <param name="image">The image to upload</param>
        /// <param name="cell">The location the upload will occur</param>
        public void uploadImageToAtlas(Bitmap image, TextureCell cell)
        {
            //GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);

            BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, cell.x, cell.y, cell.w, cell.h, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, imageData.Scan0);

            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("UPLOAD ATLAS GL ERROR: {0}", err.ToString());
            }

            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Adds an image to the texture atlas
        /// </summary>
        /// <param name="image">The image to upload</param>
        public void addImageToCollection(Bitmap image)
        {
            TextureCell newCell;
            bool res = allocateAtlasSpace(image.Width, image.Height, out newCell);

            if (res)
            {
                uploadImageToAtlas(image, newCell);
                cells.Add(newCell);
            }
            else
            {
                throw new AtlasFullException("There are too many textures");
            }
        }

        public void createInfoTexture()
        {
            this.resourceInfoID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, resourceInfoID);
            int numResources = cells.Count;
            short[] data = new short[numResources * 4];

            for (int i = 0; i < numResources; i++)
            {
                data[i * 4 + 0] = (short)cells[i].w;
                data[i * 4 + 1] = (short)cells[i].h;
                data[i * 4 + 2] = (short)cells[i].x;
                data[i * 4 + 3] = (short)cells[i].y;

                Console.WriteLine("{0} {1} {2} {3}", cells[i].w, cells[i].h, cells[i].x, cells[i].y);
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, 1, numResources, 0, OpenTK.Graphics.OpenGL.PixelFormat.RgbaInteger, PixelType.Short, data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public int getTextureID(string name)
        {
            int id = 0;
            textureIDList.TryGetValue(name, out id);

            return id;
        }

        public void getTextureList(ResourceFiles.ResourceArchive archive)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);
            List<ResourceFiles.ResourceFile> lumps = archive.getResourceList(ResourceFiles.ResourceNamespace.NS_TEXTURE);

            Bitmap defTexture = new Bitmap("./resources/missingtex.png");
            addImageToCollection(defTexture);
            defTexture.Dispose();

            textureIDList.Add("missingno", 0); lastID++;

            for (int i = 0; i < lumps.Count; i++)
            {
                try
                {
                    byte[] data = archive.loadResource(lumps[i].fullname);
                    if (data != null)
                    {
                        //Bitmap image = new Bitmap(
                        TextureFormat format = checkFormat(ref data);

                        //if its PNG, we can process it as a Windows bitmap
                        if (format == TextureFormat.FORMAT_PNG)
                        {
                            System.IO.MemoryStream pngstr = new System.IO.MemoryStream(data);

                            Bitmap img = new Bitmap(pngstr);

                            addImageToCollection(img);

                            img.Dispose();
                            pngstr.Close();
                            pngstr.Dispose();

                            textureIDList.Add(lumps[i].name, lastID); lastID++;
                        }
                    }
                    //if it isn't known, don't add it, possibly add a warning texture if it gets used
                    else
                    {
                    }
                }
                catch (Exception exc)
                {
                }
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Cleans up the current atlas
        /// </summary>
        public void cleanup()
        {
            allocated = null;
            GL.DeleteTexture(atlasTextureID);
            GL.DeleteTexture(resourceInfoID);


        }

        public TextureFormat checkFormat(ref byte[] data)
        {
            //137  80  78  71
            if (data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71)
            {
                return TextureFormat.FORMAT_PNG;
            }
            return TextureFormat.FORMAT_UNKNOWN;
        }
    }

    [Serializable]
    public class AtlasFullException : Exception
    {
        public AtlasFullException() { }
        public AtlasFullException( string message ) : base( message ) { }
        public AtlasFullException( string message, Exception inner ) : base( message, inner ) { }
        protected AtlasFullException( 
            System.Runtime.Serialization.SerializationInfo info, 
            System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
