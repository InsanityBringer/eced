using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    public enum ResourceFormat
    {
        FORMAT_WAD,
        FORMAT_ZIP
    }

    public class ResourceArchiveHeader
    {
        public string filename = "";
        public ResourceFormat format;

        public override string ToString()
        {
            return filename;
        }
    }
}
