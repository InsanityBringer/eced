using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace eced.ResourceFiles
{
    public enum ResourceType
    {
        RES_TEXT,
        RES_PIC,
        RES_GENERIC,
        RES_DIRECTORY
    }

    public enum LumpFormatType
    {
        Generic,
        VSwapTexture,
        VSwapSprite,
        DoomPatch,
        DoomFlat,
        PNG,
    }

    public enum ResourceNamespace
    {
        NS_GENERIC,
        NS_SPRITE,
        NS_TEXTURE
    }

    public class ResourceFile
    {
        //public byte[] data;
        /// <summary>
        /// The filename of the individual lump, minus the path
        /// </summary>
        public string name;
        /// <summary>
        /// The full filename of the lump, with the actual path. 
        /// </summary>
        public string fullname;
        public ResourceType type;
        public ResourceNamespace ns;
        public int pointer;
        public int size;
        public ResourceArchive host;
        public LumpFormatType format;
        //public ResourceType type;

        public ResourceFile(string name, ResourceType type, int size)
        {
            this.name = name;
            this.type = type;
            this.size = size;

            format = LumpFormatType.Generic;
        }
    }
}
