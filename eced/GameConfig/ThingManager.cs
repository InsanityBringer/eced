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
using System.IO;
using System.Xml.Linq;

namespace eced.GameConfig
{
    public class ThingManager
    {
        public List<ThingDefinition> thingList = new List<ThingDefinition>();
        public Dictionary<string, int> idToThingListMapping = new Dictionary<string, int>();
        public List<int> idlist = new List<int>();

        private ThingDefinition unknownThing;

        public ThingManager()
        {
            this.unknownThing = new ThingDefinition();
            this.unknownThing.SetData("16", "16", "Unknown thing", "Unknown", 64, 64, 64);
        }

        public ThingDefinition GetUnknownThing()
        {
            return this.unknownThing;
        }

        public ThingDefinition GetThingDef(string type)
        {
            if (idToThingListMapping.ContainsKey(type)) return thingList[idToThingListMapping[type]];
            return unknownThing;
        }

        public void LoadThingDefintions(string filename)
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

                        foreach (XNode subnode in sublist)
                        {
                            if (subnode is XContainer)
                            {
                                XContainer thingdata = (XContainer)subnode;
                                try
                                {
                                    ThingDefinition thing = ThingDefinition.FromXContainer(thingdata);
                                    thingList.Add(thing);
                                    idToThingListMapping.Add(thing.Type, thingList.Count - 1);
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
                Console.WriteLine("{0} thing types successfully registered", this.thingList.Count);
            }

            //return list;
        }
    }
}
