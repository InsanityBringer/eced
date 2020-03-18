using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    public class ImageResource : ResourceFile
    {
        public int width, height;

        public ImageResource(string name, int size) : base(name, size) { }
    }
}
