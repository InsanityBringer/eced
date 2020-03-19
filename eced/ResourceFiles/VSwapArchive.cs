using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using eced.GameConfig;
using eced.ResourceFiles.Formats;

namespace eced.ResourceFiles
{
    public class VSwapArchive : Archive
    {
        private List<Lump> lumps = new List<Lump>();
        private BinaryReader br;
        private string filename;

        public static VSwapArchive OpenArchive(string filename, VSwapNames namelist)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(filename));

            int numLumps = br.ReadInt16();
            int firstSprite = br.ReadInt16();
            int firstSound = br.ReadInt16();

            VSwapArchive archive = new VSwapArchive();
            archive.filename = filename;

            //not interested in sounds atm, so read only up to the first sound
            int localID; //Local ID in wall, sprite, or sound chunk. 
            int pointer;
            Lump lump;
            string name, directory;
            LumpNamespace ns;
            LumpFormatType format;
            for (int i = 0; i < numLumps; i++)
            {
                pointer = br.ReadInt32();
                if (i < firstSound)
                {
                    if (i < firstSprite) localID = i;
                    else localID = i - firstSprite;

                    if (i < firstSprite)
                    {
                        name = namelist.WallNames[localID];
                        ns = LumpNamespace.Texture;
                        format = LumpFormatType.VSwapTexture;
                    }
                    else if (i < firstSound)
                    {
                        name = namelist.SpriteNames[localID];
                        ns = LumpNamespace.Sprite;
                        format = LumpFormatType.VSwapSprite;
                    }
                    else
                    {
                        name = "unknown";
                        ns = LumpNamespace.Global;
                        format = LumpFormatType.Generic;
                    }

                    lump = new Lump(name, 0);
                    lump.pointer = pointer;
                    lump.@namespace = ns;
                    lump.format = format;
                    lump.fullname = name;

                    archive.lumps.Add(lump);
                }
            }

            int size;
            for (int i = 0; i < firstSound; i++)
            {
                size = br.ReadInt16();
                archive.lumps[i].size = size;
            }

            archive.br = br;

            return archive;
        }
        public override void CloseResource()
        {
            br.Close();
            br.Dispose();
        }

        public override Lump FindResource(string name)
        {
            foreach (Lump lump in lumps)
            {
                if (lump.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return lump;
            }
            return null;
        }

        public override List<Lump> GetResourceList(LumpNamespace ns)
        {
            if (ns == LumpNamespace.Global) return this.lumps;

            List<Lump> lumps = new List<Lump>();
            foreach (Lump lump in this.lumps)
            {
                if (lump.@namespace == ns)
                    lumps.Add(lump);
            }

            return lumps;
        }

        public override byte[] LoadResource(string name)
        {
            byte[] contents;
            Lump lump = FindResource(name);

            if (lump == null) return null;

            br.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
            contents = br.ReadBytes(lump.size);

            return contents;
        }

        public override void PushResource(Lump lump)
        {
            //yeah having this as an interface was a good idea...
            throw new Exception("whyyy");
        }

        public override void OpenFile()
        {
            br = new BinaryReader(File.OpenRead(filename));
        }

        public override void CloseFile()
        {
            CloseResource();
        }
    }
}
