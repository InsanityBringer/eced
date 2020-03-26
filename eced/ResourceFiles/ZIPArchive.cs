/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
 *
 *  eced is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *   eced is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with eced.  If not, see <http://www.gnu.org/licenses/>.
 *  -------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using eced.ResourceFiles.Formats;

namespace eced.ResourceFiles
{
    class ZIPArchive : Archive
    {
        private ZipFile archive;
        private List<Lump> lumps = new List<Lump>();

        private string saveToDirectory = "-";

        public static Archive loadResourceFile(string filename)
        {
            ZIPArchive zip = new ZIPArchive();

            zip.archive = new ZipFile(filename);
            zip.filename = filename;

            foreach (ZipEntry entry in zip.archive)
            {
                if (entry.IsFile)
                {
                    string name = entry.Name;
                    //TODO: Should I be a good boy and use 64bit values
                    int size = (int)entry.Size;

                    LumpNamespace type = LumpNamespace.Global;

                    //Find the directory
                    string[] splitname = name.Split('/');
                    string finalname = (splitname[splitname.Length - 1].Split('.'))[0];
                    if (splitname[0].Equals("TEXTURES", StringComparison.OrdinalIgnoreCase))
                    {
                        type = LumpNamespace.Texture;
                    }

                    Console.WriteLine("{0} {1} {2}", name, splitname[0], finalname);
                    Lump lump = new Lump(finalname, size);
                    //lump.internalObject = (Object)entry;
                    lump.fullname = name;
                    lump.pointer = (int)entry.ZipFileIndex;
                    lump.@namespace = type;

                    zip.lumps.Add(lump);
                }
            }

            zip.ClassifyArchiveLumps();
            return zip;
        }

        public override void ClassifyArchiveLumps()
        {
            byte[] data;
            Stream stream;
            for (int i = 0; i < lumps.Count; i++)
            {
                stream = archive.GetInputStream(lumps[i].pointer);
                data = new byte[lumps[i].size];
                stream.Read(data, 0, lumps[i].size);
                LumpClassifier.Classify(lumps[i], data);
            }
        }

        public override Lump FindLump(string fullname)
        {
            //string[] parts = name.Split('\\', '/');
            //return recursiveFind(parts, 0, this.lumps);

            for (int i = 0; i < lumps.Count; i++)
            {
                if (lumps[i].fullname.Equals(fullname, StringComparison.OrdinalIgnoreCase))
                    return lumps[i];
            }

            return null;

        }

        public override List<Lump> GetResourceList(LumpNamespace ns)
        {
            if (ns == LumpNamespace.Global)
                return lumps;

            List<Lump> lumplist = new List<Lump>();

            for (int i = 0; i < lumps.Count; i++)
            {
                if (lumps[i].@namespace == ns)
                    lumplist.Add(lumps[i]);
            }

            return lumplist;
        }

        public override void AddLump(Lump resource)
        {
            if (saveToDirectory != "-")
            {
                DirectoryLump directory = (DirectoryLump)FindLump(saveToDirectory);
                directory.resources.Add(resource);
            }
            else
                lumps.Add(resource);
        }

        public override void CloseResource()
        {
            //this.streamreader.Close();
            this.archive.Close();
        }

        public override byte[] LoadLump(string name)
        {
            Lump lump = FindLump(name);

            //this.streamreader.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
            //ZipEntry entry = (ZipEntry)lump.internalObject;
            BinaryReader br = new BinaryReader(this.archive.GetInputStream(lump.pointer));
            byte[] lumpdata = br.ReadBytes(lump.size);

            return lumpdata;
        }
    }
}
