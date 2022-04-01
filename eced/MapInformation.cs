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
using eced.GameConfig;

namespace eced
{
    public class MapInformation
    {
        public int sizex = 64;
        public int sizey = 64;
        public int layers = 1;
        public string lumpname = "MAP01";

        public List<ResourceFiles.ArchiveHeader> files = new List<ResourceFiles.ArchiveHeader>();
        
        public GameConfiguration Configuration { get; set; }

        /// <summary>
        /// Finds if the specified resource name exists in this map's resources
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsResource(string name)
        {
            foreach (ResourceFiles.ArchiveHeader file in files)
            {
                if (file.filename.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
