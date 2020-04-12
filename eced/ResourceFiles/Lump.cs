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
using eced.ResourceFiles.Formats;

namespace eced.ResourceFiles
{
    [Flags] //make flags so you can ask for multiple namespaces at once
    public enum LumpNamespace
    {
        Global = 0,
        Sprite = 1,
        Texture = 2,
        Flat = 4,

        //Special bits
        Rott = 16384
    }

    public class Lump
    {
        /// <summary>
        /// The filename of the individual lump, minus the path
        /// </summary>
        public string name;
        /// <summary>
        /// The full filename of the lump, with the actual path. 
        /// </summary>
        public string fullname;
        public LumpNamespace @namespace;
        public int pointer;
        public int size;
        public Archive host;
        public LumpFormatType format;

        /// <summary>
        /// Local data for this lump. Set to not null if local data should be used instead of saved data
        /// </summary>
        public byte[] Data { get; private set; } = null;

        public Lump(string name, int size)
        {
            this.name = name;
            this.size = size;

            format = LumpFormatType.Generic;
        }

        public void SetCachedData(byte[] newdata)
        {
            Data = newdata;
            size = newdata.Length;
            pointer = -1;
        }

        public void ClearCache()
        {
            Data = null;
        }
    }
}
