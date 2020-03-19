using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    public abstract class Archive
    {
        public string archiveName; 
        public abstract Lump FindResource(string name);
        public abstract List<Lump> GetResourceList(LumpNamespace ns);
        public abstract byte[] LoadResource(string name);
        public abstract void PushResource(Lump lump);
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
