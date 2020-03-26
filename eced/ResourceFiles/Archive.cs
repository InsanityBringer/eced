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

namespace eced.ResourceFiles
{
    public abstract class Archive
    {
        public string filename; 
        public abstract Lump FindLump(string name);
        public abstract List<Lump> GetResourceList(LumpNamespace ns);
        public abstract byte[] LoadLump(string name);
        public abstract void AddLump(Lump lump);
        public abstract void CloseResource();

        /// <summary>
        /// Opens the file for reading and locks it
        /// </summary>
        public virtual void OpenFile()
        {
        }

        /// <summary>
        /// Closes the file and release the lock on it
        /// </summary>
        public virtual void CloseFile()
        {
        }

        public virtual void ClassifyArchiveLumps()
        {
        }
    }
}
