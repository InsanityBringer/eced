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
using System.Xml.Linq;
using System.IO;

namespace eced.GameConfig
{
    public class VSwapNames
    {
        public List<string> WallNames { get; } = new List<string>();
        public List<string> SpriteNames { get; } = new List<string>();

        public void LoadVSwapNames(string filename)
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            try
            {
                XDocument doc = XDocument.Load(sr);
                IEnumerable<XNode> nodelist = doc.Nodes();
                foreach (XNode node in nodelist)
                {
                    if (node is XContainer)
                    {
                        XContainer container = (XContainer)node;
                        IEnumerable<XNode> sublist = container.Nodes();

                        foreach (XElement elem in container.Elements())
                        {
                            switch (elem.Name.LocalName)
                            {
                                case "wall":
                                    WallNames.Add((string)elem);
                                    break;
                                case "sprite":
                                    SpriteNames.Add((string)elem);
                                    break;
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
            }

            //return list;
        }
    }
}
