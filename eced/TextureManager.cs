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
    public struct TextureCell
    {
        public int x, y;
        public int w, h;
    }

    public enum TextureFormat
    {
        FORMAT_PNG,
        FORMAT_PATCH,
        FORMAT_WOLFWALL,
        FORMAT_UNKNOWN
    }

    /// <summary>
    /// Manages textures, responsible for building tilemap texture atlas and filling in details about each texture
    /// </summary>
    // (rewritten for the first time since 2012)
    public class TextureManager
    {
        public static Dictionary<String, int> textureList = new Dictionary<string, int>();

        private byte[] textureAtlasTexture;
        private int[] resourceInfoTexture;

        const int baseAtlasSize = 4096;

        public int atlasTextureID;
        public int resourceInfoID;
        public int numberTextureID;
        public int lastID = 0;

        public List<TextureCell> cells;
        public Dictionary<String, int> textureIDList = new Dictionary<string, int>();

        /// <summary>
        /// The current palette for processing doom format patches
        /// </summary>
        public byte[] palette = new byte[768];

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

            //Console.WriteLine("Loading texture {0} from file {1}", id, filename);

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

        public TextureManager()
        {
            //Init the palette with grayscale by default
            for (int i = 0; i < 256; i++)
            {
                palette[i*3] = palette[i*3+1] = palette[i*3+2] = (byte)i;
            }
        }

        public void uploadNumberTexture()
        {
            this.numberTextureID = getTexture("./resources/floorfont.png");
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
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, cell.x, cell.y, cell.w, cell.h, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, imageData.Scan0);

            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.WriteLine("UPLOAD ATLAS GL ERROR: {0}", err.ToString());
            }

            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Uploads a doom patch to the texture atlas
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="cell"></param>
        public void uploadPatchToAtlas(ref byte[] patch, TextureCell cell)
        {
            //Current offset in the patch
            int offset = 8;

            short w = (short)cell.w;
            short h = (short)cell.h;
            //No reason to store the pointers really
            //short[] ptrtable = new short[w];

            //Load the image data for each patch
            for (int i = 0; i < w; i++)
            {  
                int pointer = BinaryHelper.getInt32(patch[offset], patch[offset + 1], patch[offset + 2], patch[offset + 3]);
                offset += 4;

                int newoffset = pointer;
                //Load the offset and length of the patch
                byte yoffs = patch[pointer]; pointer++;
                while (yoffs != 255)
                {
                    byte len = patch[pointer]; pointer++;
                    pointer++; //Garbage byte
                    byte[] spandata = new byte[len];
                    //Resultant 32bit span
                    int[] finalspan = new int[len];
                    //copy the span into the array
                    Array.Copy(patch, pointer, spandata, 0, len);
                    //Use the current palette to build a 32bit patch
                    for (int p = 0; p < len; p++)
                    {
                        byte patchbyte = spandata[p];
                        finalspan[p] = BinaryHelper.getInt32(palette[patchbyte * 3 + 2], palette[patchbyte * 3 + 1], palette[patchbyte * 3 + 0], 255);
                    }
                    pointer += len + 1;
                    GL.TexSubImage2D(TextureTarget.Texture2D, 0, cell.x + i, cell.y + yoffs, 1, len, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, finalspan);

                    yoffs = patch[pointer]; pointer++;
                }
            }
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

        /// <summary>
        /// Adds a Doom patch to the texture atlas
        /// </summary>
        /// <param name="patch">Data dump of the patch to upload</param>
        public void addPatchToCollection(ref byte[] patch)
        {
            short patchw = BinaryHelper.getInt16(patch[0], patch[1]);
            short patchh = BinaryHelper.getInt16(patch[2], patch[3]);

            TextureCell newCell;
            bool res = allocateAtlasSpace(patchw, patchh, out newCell);

            if (res)
            {
                //uploadImageToAtlas(image, newCell);
                uploadPatchToAtlas(ref patch, newCell);
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

                //Console.WriteLine("{0} {1} {2} {3}", cells[i].w, cells[i].h, cells[i].x, cells[i].y);
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16i, 1, numResources, 0, OpenTK.Graphics.OpenGL.PixelFormat.RgbaInteger, PixelType.Short, data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public int getTextureID(string name)
        {
            int id = 0;
            textureIDList.TryGetValue(name.ToUpper(), out id);

            return id;
        }

        public void readyAtlasCreation()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);
            Bitmap defTexture = new Bitmap("./resources/missingtex.png");
            addImageToCollection(defTexture);
            defTexture.Dispose();

            textureIDList.Add("NULLTEX", 0); lastID++;
        }

        /// <summary>
        /// Called for each archive open in the current map, adds its textures to the resource list and to the atlas
        /// </summary>
        /// <param name="archive"></param>
        public void getTextureList(ResourceFiles.ResourceArchive archive)
        {
            //Try to load a palette from this resource
            if (archive.findResource("PLAYPAL") != null)
            {
                palette = archive.loadResource("PLAYPAL");
            }
            List<ResourceFiles.ResourceFile> lumps = archive.getResourceList(ResourceFiles.ResourceNamespace.NS_TEXTURE);

            for (int i = 0; i < lumps.Count; i++)
            {
                try
                {
                    byte[] data = archive.loadResource(lumps[i].fullname);
                    //Console.WriteLine(lumps[i].fullname);
                    if (data != null)
                    {
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

                            //textureIDList.Add(lumps[i].name.ToUpper(), lastID); lastID++;
                            textureIDList[lumps[i].name.ToUpper()] = lastID; lastID++;
                        }
                        //Load a doom patch
                        else if (format == TextureFormat.FORMAT_PATCH)
                        {
                            byte[] patch = archive.loadResource(lumps[i].fullname);
                            //Console.WriteLine("adding patch {0}", lumps[i].name);
                            addPatchToCollection(ref patch);
                            textureIDList[lumps[i].name.ToUpper()] = lastID; lastID++;
                        }
                    }
                    //if it isn't known, don't add it, possibly add a warning texture if it gets used
                    else
                    {
                        Console.WriteLine("Error loading lump {0} from archive {1}", lumps[i].name, archive.archiveName);
                    }
                }
                catch (Exception exc)
                {
                    //TODO: Error handling
                    Console.WriteLine("Error while processing archive textures {0}: {1}",archive.archiveName,  exc.Message);
                }
            }
            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Cleans up the current atlas
        /// </summary>
        public void cleanup()
        {
            allocated = null;
            GL.DeleteTexture(atlasTextureID);
            GL.DeleteTexture(resourceInfoID);

            this.lastID = 0;
            this.textureIDList = new Dictionary<string, int>();
            this.cells = new List<TextureCell>() ;
        }

        public TextureFormat checkFormat(ref byte[] data)
        {
            //137  80  78  71
            if (data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71)
            {
                return TextureFormat.FORMAT_PNG;
            }
            //Attempt to detect patch through some heruistics
            short w = BinaryHelper.getInt16(data[0], data[1]);
            short h = BinaryHelper.getInt16(data[2], data[3]);
            //Console.WriteLine("{0} {1}", w, h);
            if (w > 0 && h > 0)
            {
                //Console.WriteLine("this is a patch");
                return TextureFormat.FORMAT_PATCH;
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
