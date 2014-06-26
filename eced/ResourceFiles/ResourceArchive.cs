using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    abstract class ResourceArchive
    {
        public abstract ResourceFile findResource(string name);
        public abstract List<ResourceFile> getResourceList(ResourceNamespace ns);
        public abstract byte[] loadResource(string name);
        public abstract void pushResource(ResourceFile lump);
        public abstract void closeResource();
    }
}
