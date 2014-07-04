using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    public class DirectoryResource : ResourceFile
    {
        public List<ResourceFile> resources;

        public DirectoryResource(string name, ResourceType type, int size) : base(name, type, size) { }
    }
}
