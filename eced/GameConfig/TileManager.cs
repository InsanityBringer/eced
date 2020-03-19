/*  ---------------------------------------------------------------------
 *  Copyright (c) 2013 ISB
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
using System.Xml.Linq;
using System.IO;

namespace eced.GameConfig
{
    public class TileManager
    {
        public List<Tile> tileset = new List<Tile>();

        /// <summary>
        /// The filename where the main XML data is stored.
        /// </summary>
        private string filename;

        public TileManager(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Loads the default tileset from an XML file
        /// </summary>
        public void LoadPalette()
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            try
            {
                XDocument doc = XDocument.Load(sr);
                IEnumerable<XNode> nodelist = doc.Nodes();
                foreach (XNode node in nodelist)
                {
                    //Console.WriteLine("Node type: {0}", node.NodeType.ToString());
                    if (node is XContainer)
                    {
                        XContainer container = (XContainer)node;
                        IEnumerable<XNode> sublist = container.Nodes();

                        foreach (XNode subnode in sublist)
                        {
                            //Console.WriteLine("Sub Node type: {0}, Data: {1}", subnode.NodeType.ToString(), subnode.ToString());
                            if (subnode is XContainer)
                            {
                                XContainer tiledata = (XContainer)subnode;
                                int id = 0;
                                try
                                {
                                    //id = Int32.Parse(tiledata.Element("id").Value);
                                    Tile tile = Tile.FromXMLContainer(tiledata);
                                    tileset.Add(tile);
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine("Failure when parsing tile type");
                                    Console.WriteLine(exc.Message);
                                    Console.WriteLine(exc.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failure in reading: " + exc.Message);
            }
            finally
            {
                sr.Close();
                Console.WriteLine("{0} tile types successfully registered", tileset.Count);
            }
        }
    }
}
