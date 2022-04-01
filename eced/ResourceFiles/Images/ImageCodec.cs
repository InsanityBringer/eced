/*  ---------------------------------------------------------------------
 *  Copyright (c) 2022 ISB
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

namespace eced.ResourceFiles.Images
{
    /// <summary>
    /// Represents a texture. Can be scaled, and eventually 8 or 32-bit formats. 
    /// </summary>
    public class BasicImage
    {
        /// <summary>
        /// Width of the bitmap data.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Height of the bitmap data.
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Array of pixel data, as 32-bit RGBA.
        /// </summary>
        public int[] Data { get; }
        /// <summary>
        /// Scale of the texture in the X axis, such that the final resolution is Width / ScaleX.
        /// </summary>
        public float ScaleX { get; set; } = 1.0f;
        /// <summary>
        /// Scale of the texture in the Y axis, such that the final resolution is Height / ScaleY.
        /// </summary>
        public float ScaleY { get; set; } = 1.0f;
        /// <summary>
        /// Amount of map units in the XY plane this texture occupies. 
        /// </summary>
        public int MapWidth
        {
            get
            {
                return (int)(Width / ScaleX);
            }
        }
        /// <summary>
        /// Amount of map units in the Z axis this texture occupies. 
        /// </summary>
        public int MapHeight
        {
            get
            {
                return (int)(Height / ScaleY);
            }
        }

        public BasicImage(int width, int height)
        {
            Width = width;
            Height = height;

            Data = new int[Width * Height];
        }
    }

    //codec makes these classes sound more involved than they actually are, but I can't think of a better name when the thing that manages these is already ImageDecoder
    public abstract class ImageCodec
    {
        public abstract BasicImage DecodeImage(Lump lump, byte[] data, byte[] palette);
    }
}
