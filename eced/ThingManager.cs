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
using System.IO;
using System.Xml.Linq;

namespace eced
{
    public class ThingManager
    {
        public Dictionary<int, ThingDefinition> thinglist = new Dictionary<int, ThingDefinition>();
        public List<int> idlist = new List<int>();

        private string filename;
        private ThingDefinition unknownThing;

        public ThingManager(String filename)
        {
            this.filename = filename;
            this.unknownThing = new ThingDefinition();
            this.unknownThing.setData("16", "16", "UnknownThing", "Unknown", "-1");
        }

        public ThingDefinition getUnknownThing()
        {
            return this.unknownThing;
        }

        public void processData()
        {
            //List<ThingListElement> list = new List<ThingListElement>();

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
                                XContainer thingdata = (XContainer)subnode;
                                int id = 0;
                                try
                                {
                                    id = Int32.Parse(thingdata.Element("id").Value);

                                    ThingDefinition thing = new ThingDefinition();

                                    thing.setData(thingdata.Element("radius").Value, thingdata.Element("height").Value,
                                        thingdata.Element("name").Value, thingdata.Element("type").Value, thingdata.Element("id").Value);

                                    thinglist.Add(id, thing);

                                    //ThingListElement listelement;
                                    //listelement.thing = thing;
                                    //listelement.id = id;
                                    idlist.Add(id);
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine("Failure when parsing thing type");
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
                Console.WriteLine("{0} thing types successfully registered", this.thinglist.Count);
            }

            //return list;
        }
    }
}
