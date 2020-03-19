using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.ResourceFiles
{
    //TODO: get rid of this file
    public enum ResourceFormat
    {
        Wad,
        Zip,
        VSwap
    }

    public class ArchiveHeader
    {
        public string filename = "";
        public ResourceFormat format;

        public override string ToString()
        {
            return filename;
        }
    }
}
