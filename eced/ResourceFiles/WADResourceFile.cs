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
    public class WADResourceFile : ResourceArchive
    {
        public List<ResourceFile> lumps = new List<ResourceFile>();
        public string saveToDirectory = "-";
        public string filename;

        BinaryReader streamreader;
        BinaryWriter streamwriter = null;
        FileStream stream;

        public WADResourceFile()
        {
        }

        public static ResourceArchive loadResourceFile(string filename)
        {
            WADResourceFile wad = new WADResourceFile();
            wad.archiveName = filename;
            wad.filename = filename;
            wad.stream = File.Open(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(wad.stream, Encoding.ASCII);
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
                ResourceNamespace ns = ResourceNamespace.Global;
                //eat null bytes for convenience
                name = name.Trim('\0', ' '); //maybe this will work
                fullname = fullname.Trim('\0'); //try to cut off null bytes at end of fullname

                if (name == "TX_START" || name == "S_START")
                {
                    wad.saveToDirectory = "TEXTURES";
                }
                else if (name == "TX_END" || name == "S_END")
                {
                    //no more saving to textures dir
                    wad.saveToDirectory = "-";
                }
                else if (wad.saveToDirectory == "TEXTURES")
                {
                    fullname = wad.saveToDirectory + "/" + name;
                    ns = ResourceNamespace.Texture;
                }

                ResourceFile lump = new ResourceFile(name, size);
                lump.fullname = fullname;
                lump.pointer = ptr;
                lump.ns = ns;
                lump.size = size;
                wad.lumps.Add(lump);
                Console.WriteLine("{0}, {1} {2}", lump.fullname, lump.pointer, lump.size);
            }

            wad.streamreader = br;

            Console.WriteLine("{0} lumps loaded", wad.lumps.Count);
            wad.ClassifyArchiveLumps();
            wad.CloseFile();

            return wad;
        }

        public override ResourceFile FindResource(string fullname)
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
        
        public override List<ResourceFile> GetResourceList(ResourceNamespace ns)
        {
            if (ns == ResourceNamespace.Global)
                return lumps;

            List<ResourceFile> lumplist = new List<ResourceFile>();

            for (int i = 0; i < lumps.Count; i++)
            {
                    if (lumps[i].ns == ns)
                        lumplist.Add(lumps[i]);
            }

            return lumplist;
        }

        public override void PushResource(ResourceFile resource)
        {
            if (saveToDirectory != "-")
            {
                DirectoryResource directory = (DirectoryResource)FindResource(saveToDirectory);
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

        public override byte[] LoadResource(string name)
        {
            ResourceFile lump = FindResource(name);

            this.streamreader.BaseStream.Seek(lump.pointer, SeekOrigin.Begin);
            byte[] lumpdata = this.streamreader.ReadBytes(lump.size);

            return lumpdata;
        }

        public byte[] LoadLumpByIndex(int index)
        {
            ResourceFile lump = lumps[index];

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
        /// Finds all special lumps associated with a map
        /// </summary>
        /// <param name="name">The mapname to find them for</param>
        /// <returns>Lump indicies of all the special lumps</returns>
        public List<int> findSpecialMapLumps(string name)
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
                for (int li = firstelement+2; li < lastelement; li++)
                {
                    //just add the index to the list
                    foundlumps.Add(li);
                }
            }

            return foundlumps;
        }

        /// <summary>
        /// Delete a map from the wad directory
        /// </summary>
        /// <param name="name"></param>
        public void deleteMap(string name)
        {
            //Find the index of the first element
            int firstelement = -1, lastelement = -1;
            int i;
            for (i = 0; i < lumps.Count; i++)
            {
                if (lumps[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (i != (lumps.Count-1) && lumps[i+1].name.Equals("TEXTMAP", StringComparison.OrdinalIgnoreCase))
                    firstelement = i;
                    break;
                }
            }

            //Map isn't present, so abort
            if (firstelement == -1)
            {
                return;
            }

            //Find the index of the ENDMAP element from this map
            for (; i < lumps.Count; i++)
            {
                if (lumps[i].name.Equals("ENDMAP", StringComparison.OrdinalIgnoreCase))
                {
                    lastelement = i;
                }
            }
            //Delete from the directory
            //Make sure the end is actually present
            if (lastelement != -1)
            {
                lumps.RemoveRange(firstelement, lastelement - firstelement);
            }
        }

        public List<string> findAllMapNames()
        {
            List<string> maplumps = new List<string>();

            //Count up to the amount of lumps - 2 since two additonal lumps are needed to define a map
            for (int i = 0; i < lumps.Count - 2; i++)
            {
                //Check each luimp if a TEXTMAP follows it
                string checkmap = lumps[i].name;
                string nextlump = lumps[i+1].name;

                if (nextlump.Equals("TEXTMAP", StringComparison.OrdinalIgnoreCase))
                {
                    maplumps.Add(checkmap);
                }
            }

            return maplumps;
        }

        /// <summary>
        /// Writes out a new wad, appending the new lumps onto the end of the stack. Expects pointer for each ResourceFile to point to the position of the data in the old archive
        /// Opens and closes the old wad automatically
        /// </summary>
        /// <param name="destfilename">The destination to write the new file</param>
        /// <param name="newLumps">The directory entries of the new lumps to add</param>
        /// <param name="data"></param>
        /// <param name="update">True if the wad should be updated, false if it should be done solely with the new data</param>
        public void updateToNewWad(string destfilename, ref List<ResourceFile> newLumps, ref byte[] data, bool update)
        {
            //Open the file for reading
            if (update)
            {
                OpenFile();
            }

            //Find how large the resultant WAD data will be
            int numLumps = lumps.Count + newLumps.Count;
            int directorySize = numLumps * 16;
            int dataSize = 0;
            foreach (ResourceFile lump in lumps)
            {
                dataSize += lump.size;
            }
            foreach (ResourceFile lump in newLumps)
            {
                dataSize += lump.size;
            }
            int finalSize = directorySize + dataSize + 12;

            //Allocate a block of memory of the bytes
            byte[] block = new byte[finalSize];
            //Write magic number for header
            byte[] intblock = new byte[4];
            block[0] = (byte)'P';
            block[1] = (byte)'W';
            block[2] = (byte)'A';
            block[3] = (byte)'D';

            //Write number of lumps and pointer
            BinaryHelper.getBytes(numLumps, ref intblock);
            Array.Copy(intblock, 0, block, 4, 4);
            BinaryHelper.getBytes(12, ref intblock);
            Array.Copy(intblock, 0, block, 8, 4);

            //Build the directory
            int ptr = 12;
            int dataptr = 12 + directorySize;

            //only try to load old lumps when updating
            if (update)
            {
                foreach (ResourceFile lump in lumps)
                {
                    //copy the data into the data block
                    streamreader.BaseStream.Seek((long)lump.pointer, SeekOrigin.Begin);
                    byte[] lumpdata = streamreader.ReadBytes(lump.size);
                    if (lumpdata.Length != lump.size)
                    {
                        //TODO: Report error more formally
                        Console.WriteLine("ERROR: Loaded less bytes than expected reading lump {0}", lump.name);
                    }
                    Array.Copy(lumpdata, 0, block, dataptr, lump.size);

                    BinaryHelper.getBytes(dataptr, ref intblock);
                    Array.Copy(intblock, 0, block, ptr, 4); ptr += 4;
                    BinaryHelper.getBytes(lump.size, ref intblock);
                    Array.Copy(intblock, 0, block, ptr, 4); ptr += 4;

                    byte[] name = Encoding.ASCII.GetBytes(lump.name);
                    Array.Copy(name, 0, block, ptr, name.Length); ptr += 8;

                    dataptr += lump.size;
                }
            }
            foreach (ResourceFile lump in newLumps)
            {
                //copy the data into the data block
                Array.Copy(data, lump.pointer, block, dataptr, lump.size);

                BinaryHelper.getBytes(dataptr, ref intblock);
                Array.Copy(intblock, 0, block, ptr, 4); ptr += 4;
                BinaryHelper.getBytes(lump.size, ref intblock);
                Array.Copy(intblock, 0, block, ptr, 4); ptr += 4;

                byte[] name = Encoding.ASCII.GetBytes(lump.name);
                Array.Copy(name, 0, block, ptr, name.Length); ptr += 8;

                dataptr += lump.size;
            }

            //Close for reading
            if (update)
            {
                CloseFile();
            }

            //Open the wad for writing
            BinaryWriter bw = new BinaryWriter(File.Open(destfilename, FileMode.Create), Encoding.ASCII);

            //Write the wad
            bw.Write(block);

            bw.Close();

            if (!update)
            {
                this.filename = destfilename;
            }
        }
    }
}
