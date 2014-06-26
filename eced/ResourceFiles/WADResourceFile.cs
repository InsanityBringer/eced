using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace eced.ResourceFiles
{
    class WADResourceFile : ResourceArchive
    {
        public List<ResourceFile> lumps = new List<ResourceFile>();
        public string saveToDirectory = "-";

        BinaryReader filestream;

        static ResourceArchive loadResourceFile(string filename)
        {
            WADResourceFile wad = new WADResourceFile();
            BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open), Encoding.ASCII);
            int fourcc = br.ReadInt32();
            int lumps = br.ReadInt32();
            int directory = br.ReadInt32();

            br.BaseStream.Seek(directory, SeekOrigin.Begin);

            for (int i = 0; i < lumps; i++)
            {
                int ptr = br.ReadInt32();
                int size = br.ReadInt32();
                string name = new String(br.ReadChars(8));
                string fullname = name;
                ResourceNamespace ns = ResourceNamespace.NS_GENERIC;
                //eat null bytes for convenience
                name = name.Trim('\0', ' '); //maybe this will work

                if (name == "TX_START")
                {
                    wad.saveToDirectory = "TEXTURES";
                }
                else if (name == "TX_END")
                {
                    //no more saving to textures dir
                    wad.saveToDirectory = "-";
                }
                else if (wad.saveToDirectory == "TEXTURES")
                {
                    fullname = wad.saveToDirectory + "/" + name;
                    ns = ResourceNamespace.NS_TEXTURE;
                }

                ResourceFile lump = new ResourceFile(name, ResourceType.RES_GENERIC, size);
                lump.fullname = fullname;
                lump.pointer = ptr;
                lump.ns = ns;
                wad.lumps.Add(lump);
            }

            wad.filestream = br;

            return (ResourceArchive)wad;
        }

        public override ResourceFile findResource(string fullname)
        {
            //string[] parts = name.Split('\\', '/');
            //return recursiveFind(parts, 0, this.lumps);

            for (int i = 0; i < lumps.Count; i++)
            {
                if (lumps[i].fullname == fullname)
                    return lumps[i];
            }

            return null;
            
        }
        
        public override List<ResourceFile> getResourceList(ResourceNamespace ns)
        {
            if (ns == ResourceNamespace.NS_GENERIC)
                return lumps;

            List<ResourceFile> lumplist = new List<ResourceFile>();

            for (int i = 0; i < lumps.Count; i++)
            {
                    if (lumps[i].ns == ns)
                        lumplist.Add(lumps[i]);
            }

            return lumplist;
        }

        public override void pushResource(ResourceFile resource)
        {
            if (saveToDirectory != "-")
            {
                DirectoryResource directory = (DirectoryResource)findResource(saveToDirectory);
                directory.resources.Add(resource);
            }
            else
                lumps.Add(resource);
        }

        public override void closeResource()
        {
            this.filestream.Close();
        }

        public override byte[] loadResource(string name)
        {
            ResourceFile lump = findResource(name);

            this.filestream.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
            byte[] lumpdata = this.filestream.ReadBytes(lump.size);

            return lumpdata;
        }
    }
}
