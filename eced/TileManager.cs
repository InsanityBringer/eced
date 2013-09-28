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
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Drawing;

namespace eced
{
    class TileManager
    {
        public List<Tile> tileset = new List<Tile>();
        //public Dictionary<int, Tile> tileset = new Dictionary<int, Tile>();

        public static Tile tile1 = new Tile(0);
        public static Tile tile2 = new Tile(7);

        /// <summary>
        /// The filename where the main XML data is stored.
        /// </summary>
        private string filename;

        public TileManager(string filename)
        {
            this.filename = filename;
        }

        public Bitmap fillOutTiles(Bitmap atlas)
        {
            Bitmap selection = new Bitmap(128, 192);

            Graphics g = Graphics.FromImage(selection);

            for (int x = 0; x < tileset.Count; x++)
            {
                Tile tile = tileset[x];

                int sx = (tile.id % 16)* 8;
                int sy = (tile.id / 16) * 8;

                Rectangle srcRect = new Rectangle(sx, sy, 8, 8);
                sx = (x % (128 / 16)) * 16;
                sy = (x / (128 / 16)) * 16;

                Rectangle destRect = new Rectangle(sx, sy, 16, 16);

                g.DrawImage(atlas, destRect, srcRect, GraphicsUnit.Pixel);
            }

            return selection;
        }

        /// <summary>
        /// Loads the default tileset from an XML file
        /// </summary>
        public void loadTileset()
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            try
            {
                XDocument doc = XDocument.Load(sr);
                IEnumerable<XNode> nodelist = doc.Nodes();
                foreach (XNode node in nodelist)
                {
                    Console.WriteLine("Node type: {0}", node.NodeType.ToString());
                    if (node is XContainer)
                    {
                        XContainer container = (XContainer)node;
                        IEnumerable<XNode> sublist = container.Nodes();

                        foreach (XNode subnode in sublist)
                        {
                            Console.WriteLine("Sub Node type: {0}, Data: {1}", subnode.NodeType.ToString(), subnode.ToString());
                            if (subnode is XContainer)
                            {
                                XContainer tiledata = (XContainer)subnode;
                                int id = 0;
                                try
                                {
                                    id = Int32.Parse(tiledata.Element("id").Value);

                                    Tile tile = new Tile(id);

                                    tile.processData(tiledata.Element("texn").Value, tiledata.Element("texs").Value, tiledata.Element("texe").Value, tiledata.Element("texw").Value,
                                        tiledata.Element("offh").Value, tiledata.Element("offv").Value,
                                        tiledata.Element("blockn").Value, tiledata.Element("blockn").Value, tiledata.Element("blockn").Value, tiledata.Element("blockn").Value);

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
