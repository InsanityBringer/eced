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
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

using eced.ResourceFiles.Images;

namespace eced.Renderer
{
    /// <summary>
    /// Represents a single cell on the tilemap texture atlas
    /// </summary>
    public struct TextureCell
    {
        public int x, y;
        public int w, h;
    }

    public struct TextureInformation : IComparable
    {
        public string name;
        public string archive;
        public int width, height;
        public int id;

        public TextureInformation(string name, string archive, int width, int height, int id)
        {
            this.name = name; this.archive = archive; this.width = width; this.height = height; this.id = id;
        }

        public int CompareTo(object obj)
        {
            //Alphabetical sorting
            TextureInformation info = (TextureInformation)obj;
            return name.CompareTo(info.name);
        }
    }

    /// <summary>
    /// Manages textures, responsible for building tilemap texture atlas and filling in details about each texture
    /// </summary>
    // (rewritten for the first time since 2012)
    // (cleaned up and improved in 2020...)
    // (still needs much more improvement... TODO: Move atlas to own class? Sounds better...)
    // (but then what does TextureManager do then?)
    public class TextureManager
    {
        private RendererState state;
        public static Dictionary<string, int> textureList = new Dictionary<string, int>();

        const int baseAtlasSize = 4096;

        public int atlasTextureID;
        public int resourceInfoID;
        public int numberTextureID;
        public int lastID = 0;

        public List<TextureCell> cells;
        public Dictionary<string, TextureInformation> textureIDList = new Dictionary<string, TextureInformation>();
        private TextureRenderTarget renderTarget;

        /// <summary>
        /// The current palette for processing doom format patches
        /// </summary>
        public byte[] palette = new byte[768];

        public TextureManager(RendererState host)
        {
            //Init the palette with grayscale by default
            for (int i = 0; i < 256; i++)
            {
                palette[i * 3] = palette[i * 3 + 1] = palette[i * 3 + 2] = (byte)i;
            }
            state = host;
            renderTarget = new TextureRenderTarget(state);
        }

        public static int LookUpTextureID(String filename)
        {
            if (textureList.ContainsKey(filename))
            {
                return textureList[filename];
            }
            return -1;
        }

        public static int GetTexture(String filename)
        {
            return GetTexture(filename, false, false);
        }

        public static int GetTexture(String filename, bool linear, bool clamp)
        {
            RendererState.ErrorCheck("TextureManager::GetTexture: debug");
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
            RendererState.ErrorCheck("TextureManager::GetTexture: Uploading texture");

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
            if (clamp)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }
            RendererState.ErrorCheck("TextureManager::GetTexture: Setting texture params");

            textureList.Add(filename, id);

            return id;
        }

        public void CreateZoneNumberTexture()
        {
            this.numberTextureID = GetTexture("./Resources/floorfont.png");
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
        public bool AllocateAtlasSpace(int width, int height, out TextureCell res)
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
        public void AllocateAtlasTexture()
        {
            allocated = new int[baseAtlasSize];
            atlasTextureID = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);
            //Console.WriteLine("Got texture {0} for atlas", atlasTextureID);
            //GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rgba8, baseAtlasSize, baseAtlasSize);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, baseAtlasSize, baseAtlasSize, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, (IntPtr)0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            RendererState.ErrorCheck("TextureManager::AllocateAtlasTexture: creating atlas base");

            cells = new List<TextureCell>();
        }

        /// <summary>
        /// Binds and clears the texture atlas
        /// </summary>
        public void InitAtlas()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, atlasTextureID);
            Bitmap defTexture = new Bitmap("./Resources/missingtex.png");
            AddImageToAtlas(PNGCodec.BasicImageFromBitmap(defTexture));
            defTexture.Dispose();

            textureIDList.Add("NULLTEX", new TextureInformation("NULLTEX", "", 32, 32, 0)); lastID++;
        }

        /// <summary>
        /// Called for each archive open in the current map, adds its textures to the resource list and to the atlas
        /// </summary>
        /// <param name="archive"></param>
        public void AddArchiveTextures(ResourceFiles.Archive archive)
        {
            List<ResourceFiles.Lump> lumps = archive.GetResourceList(ResourceFiles.LumpNamespace.Texture);
            BasicImage img;

            for (int i = 0; i < lumps.Count; i++)
            {
                //try
                {
                    byte[] data = archive.LoadLump(lumps[i].fullname);
                    //Console.WriteLine(lumps[i].fullname);
                    if (data != null)
                    {
                        img = ImageDecoder.DecodeLump(lumps[i], data, state.CurrentState.CurrentMapInfo.Palette);
                        AddImageToAtlas(img);
                        //textureIDList[lumps[i].name.ToUpper()] = lastID; lastID++;
                        textureIDList[lumps[i].name.ToUpperInvariant()] = new TextureInformation(lumps[i].name.ToUpperInvariant(), archive.filename, img.x, img.y, lastID); lastID++;
                        //Console.WriteLine("Added texture {0} at position {1}", lumps[i].name.ToUpper(), lastID - 1);
                    }
                    //if it isn't known, don't add it, possibly add a warning texture if it gets used
                    else
                    {
                        Console.WriteLine("Error loading lump {0} from archive {1}", lumps[i].name, archive.filename);
                    }
                }
                /*catch (Exception exc)
                {
                    //TODO: Error handling
                    Console.WriteLine("Error while processing archive texture {4} (type: {5}) from archive {0}: {1} {2}\nStack trace:\n{3}", archive.filename, exc.GetType().ToString(), exc.Message, exc.StackTrace, lumps[i].fullname, lumps[i].format);
                }*/
            }
            //GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Uploads a texture to the atlas at the specified cell block
        /// </summary>
        /// <param name="img">The image to upload</param>
        /// <param name="cell">The location the upload will occur</param>
        public void UploadImageToAtlas(BasicImage img, TextureCell cell)
        {
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, cell.x, cell.y, cell.w, cell.h, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, img.data);

            RendererState.ErrorCheck("TextureManager:UploadImageToAtlas: uploading texture");
        }

        /// <summary>
        /// Adds a basic image to the texture atlas
        /// </summary>
        /// <param name="img">Basic image to upload</param>
        public void AddImageToAtlas(BasicImage img)
        {
            TextureCell newCell;
            bool res = AllocateAtlasSpace(img.x, img.y, out newCell);

            if (res)
            {
                UploadImageToAtlas(img, newCell);
                cells.Add(newCell);
            }
            else
            {
                throw new AtlasFullException("There are too many textures");
            }
        }

        public void GenerateAtlasInfoTexture()
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
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public int GetTextureID(string name)
        {
            int id = 0;
            TextureInformation info;
            if (textureIDList.TryGetValue(name.ToUpper(), out info))
                id = info.id;

            return id;
        }

        /// <summary>
        /// Cleans up the current atlas
        /// </summary>
        public void DestroyAtlas()
        {
            allocated = null;
            if (atlasTextureID != 0)
            {
                GL.DeleteTexture(atlasTextureID);
                GL.DeleteTexture(resourceInfoID);
            }

            this.lastID = 0;
            this.textureIDList = new Dictionary<string, TextureInformation>();
            this.cells = new List<TextureCell>() ;
        }

        public byte[] GetTextureImage(int id)
        {
            return renderTarget.RenderTexture(id);
        }

        public List<TextureInformation> GetTextureList()
        {
            List<TextureInformation> textures = new List<TextureInformation>();
            foreach (TextureInformation info in textureIDList.Values)
                textures.Add(info);

            return textures;
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
