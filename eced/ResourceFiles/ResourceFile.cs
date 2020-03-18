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

using eced.ResourceFiles.Formats;

namespace eced.ResourceFiles
{
    public enum ResourceType
    {
        RES_TEXT,
        RES_PIC,
        RES_GENERIC,
        RES_DIRECTORY
    }

    public enum ResourceNamespace
    {
        Global,
        Sprite,
        Texture,
        Flat
    }

    public class ResourceFile
    {
        //public byte[] data;
        /// <summary>
        /// The filename of the individual lump, minus the path
        /// </summary>
        public string name;
        /// <summary>
        /// The full filename of the lump, with the actual path. 
        /// </summary>
        public string fullname;
        public ResourceType type;
        public ResourceNamespace ns;
        public int pointer;
        public int size;
        public ResourceArchive host;
        public LumpFormatType format;
        //public ResourceType type;

        public ResourceFile(string name, int size)
        {
            this.name = name;
            this.size = size;

            format = LumpFormatType.Generic;
        }
    }
}
