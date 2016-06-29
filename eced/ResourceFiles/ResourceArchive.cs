using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    public abstract class ResourceArchive
    {
        public string archiveName; 
        public abstract ResourceFile findResource(string name);
        public abstract List<ResourceFile> getResourceList(ResourceNamespace ns);
        public abstract byte[] loadResource(string name);
        public abstract void pushResource(ResourceFile lump);
        public abstract void closeResource();

        /// <summary>
        /// Opens the file for reading and locks it
        /// </summary>
        public virtual void openFile()
        {
        }

        /// <summary>
        /// Closes the file and release the lock on it
        /// </summary>
        public virtual void closeFile()
        {
        }
    }
}
