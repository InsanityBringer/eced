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
using System.Text;
using System.IO;
using eced.ResourceFiles.Formats;

namespace eced.ResourceFiles
{
    public class WADArchive : Archive
    {
        public List<Lump> lumps = new List<Lump>();
        public string saveToDirectory = "-";

        BinaryReader streamreader;
        FileStream stream;

        public WADArchive()
        {
        }

        public static Archive loadResourceFile(string filename)
        {
            WADArchive wad = new WADArchive();
            wad.filename = filename;
            wad.stream = File.Open(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(wad.stream, Encoding.ASCII);
            int fourcc = br.ReadInt32();
            int lumps = br.ReadInt32();
            int directory = br.ReadInt32();

            br.BaseStream.Seek(directory, SeekOrigin.Begin);
            LumpNamespace ns = LumpNamespace.Global;

            for (int i = 0; i < lumps; i++)
            {
                int ptr = br.ReadInt32();
                int size = br.ReadInt32();
                string name = new String(br.ReadChars(8));
                string fullname = name;
                //eat null bytes for convenience
                name = name.Trim('\0', ' '); //maybe this will work
                fullname = fullname.Trim('\0'); //try to cut off null bytes at end of fullname

                //TODO: Better namespacing system
                if (name == "TX_START")
                {
                    ns = LumpNamespace.Texture;
                }
                else if (name == "TX_END")
                {
                    ns = LumpNamespace.Global;
                }
                else if (name == "S_START")
                {
                    ns = LumpNamespace.Sprite;
                }
                else if (name == "S_END")
                {
                    ns = LumpNamespace.Global;
                }
                else if (name == "F_START")
                {
                    ns = LumpNamespace.Flat;
                }
                else if (name == "F_END")
                {
                    ns = LumpNamespace.Global;
                }

                Lump lump = new Lump(name, size);
                lump.fullname = fullname;
                lump.pointer = ptr;
                if (size != 0) //hack to avoid including the namespace markers themselves
                    lump.@namespace = ns;
                lump.size = size;
                wad.lumps.Add(lump);
                //Console.WriteLine("{0}, {1} {2}", lump.fullname, lump.pointer, lump.size);
            }

            wad.streamreader = br;

            //Console.WriteLine("{0} lumps loaded", wad.lumps.Count);
            wad.ClassifyArchiveLumps();
            wad.CloseFile();

            return wad;
        }

        public override Lump FindLump(string fullname)
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
        
        public override List<Lump> GetResourceList(LumpNamespace ns)
        {
            if ((ns & LumpNamespace.Global) != 0)
                return lumps;

            List<Lump> lumplist = new List<Lump>();

            for (int i = 0; i < lumps.Count; i++)
            {
                    if ((lumps[i].@namespace & ns) != 0)
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
            this.streamreader.Close();
            this.streamreader.Dispose();
        }

        /// <summary>
        /// Opens the resource file for reading, locking it
        /// Close when done to avoid keeping the file exclusively loaded
        /// </summary>
        public override void OpenFile()
        {
            this.streamreader = new BinaryReader(File.Open(filename, FileMode.Open));
        }

        /// <summary>
        /// Closes the resource file, releasing the lock on it
        /// </summary>
        public override void CloseFile()
        {
            this.CloseResource();
        }

        public override byte[] LoadLump(string name)
        {
            Lump lump = FindLump(name);

            this.streamreader.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
            byte[] lumpdata = this.streamreader.ReadBytes(lump.size);

            return lumpdata;
        }

        public byte[] LoadLumpByIndex(int index)
        {
            Lump lump = lumps[index];

            this.streamreader.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
            byte[] lumpdata = this.streamreader.ReadBytes(lump.size);

            return lumpdata;
        }

        public override void ClassifyArchiveLumps()
        {
            byte[] data;
            for (int i = 0; i < lumps.Count; i++)
            {
                //TODO: This should be done more cleanly to avoid having to load every lump
                data = LoadLumpByIndex(i);
                LumpClassifier.Classify(lumps[i], data);
            }
        }

        /// <summary>
        /// Finds all lumps associated with a map
        /// </summary>
        /// <param name="name">The mapname to find them for</param>
        /// <returns>Lump indicies of all the special lumps</returns>
        public List<int> FindMapLumps(string name)
        {
            List<int> foundlumps = new List<int>();
            int firstelement = -1, lastelement = -1;
            int i;
            for (i = 0; i < lumps.Count; i++)
            {
                if (lumps[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (i != (lumps.Count - 1) && lumps[i + 1].name.Equals("TEXTMAP", StringComparison.OrdinalIgnoreCase))
                        firstelement = i;
                    break;
                }
            }

            //Map isn't present, so abort
            if (firstelement == -1)
            {
                return foundlumps;
            }

            //Find the index of the ENDMAP element from this map
            for (; i < lumps.Count; i++)
            {
                if (lumps[i].name.Equals("ENDMAP", StringComparison.OrdinalIgnoreCase))
                {
                    lastelement = i;
                }
            }

            //make sure there actually is a map
            if (lastelement != -1)
            {
                for (int li = firstelement; li < lastelement; li++)
                {
                    //just add the index to the list
                    foundlumps.Add(li);
                }
            }

            return foundlumps;
        }

        public List<int> FindMapLumps()
        {
            List<int> maplumps = new List<int>();

            //Count up to the amount of lumps - 2 since two additonal lumps are needed to define a map
            for (int i = 0; i < lumps.Count - 2; i++)
            {
                //Check each luimp if a TEXTMAP follows it
                string checkmap = lumps[i].name;
                string nextlump = lumps[i+1].name;

                if (nextlump.Equals("TEXTMAP", StringComparison.OrdinalIgnoreCase))
                {
                    maplumps.Add(i);
                }
            }

            return maplumps;
        }

        public void Save(string newfilename = "")
        {
            string tempFilename;

            //Save the new file to a temporary filename
            if (newfilename == "")
                tempFilename = Path.ChangeExtension(filename, "new");
            else
                tempFilename = Path.ChangeExtension(newfilename, "new");

            //If the source file isn't open, load it for reading
            //TODO: weird things will happen if the file is deleted at some point, needs fix...
            if (File.Exists(filename))
            {
                if (streamreader != null)
                    OpenFile();
            }

            BinaryWriter bw = new BinaryWriter(File.Open(tempFilename, FileMode.Create));
            bw.Write(0x44415750); //IWAD header
            bw.Write(lumps.Count); //number of lumps
            int directoryOffset = 12;
            foreach (Lump lump in lumps)
                directoryOffset += lump.size;
            bw.Write(directoryOffset);

            byte[] data;
            //Write the data block
            foreach (Lump lump in lumps)
            {
                if (lump.size != 0)
                {
                    if (File.Exists(filename))
                    {
                        if (lump.Data == null)
                        {
                            streamreader.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
                            data = streamreader.ReadBytes(lump.size);
                        }
                        else data = lump.Data;
                    }
                    else
                    {
                        if (lump.Data == null)
                            throw new Exception("WADArchive::Save: Trying to write lump without 0 size or cached data to new WAD file");
                        else
                            data = lump.Data;
                    }
                    lump.pointer = (int)bw.BaseStream.Position;

                    bw.Write(data);

                    lump.ClearCache(); //Free the cached data
                }
            }
            //Write the directory
            foreach (Lump lump in lumps)
            {
                bw.Write(lump.pointer);
                bw.Write(lump.size);
                for (int i = 0; i < 8; i++)
                {
                    if (i < lump.name.Length)
                        bw.Write((byte)lump.name[i]);
                    else bw.Write((byte)0);
                }
            }

            bw.Flush();
            bw.Close();
            bw.Dispose();

            //.new file is successfully written, now move the old file over to a backup extension and create
            if (File.Exists(filename))
            {
                CloseFile();
            }

            if (newfilename != "")
            {
                filename = newfilename;
            }

            string backupFilename = Path.ChangeExtension(tempFilename, "bak");
            try
            {
                File.Delete(backupFilename);
            }
            catch (FileNotFoundException) { } //checking existence then opening opens up 1 in a million race condition...

            try
            {
                File.Move(filename, backupFilename);
            }
            catch (FileNotFoundException) { }

            File.Move(tempFilename, filename);
        }
    }
}
