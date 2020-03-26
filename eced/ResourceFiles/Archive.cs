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
