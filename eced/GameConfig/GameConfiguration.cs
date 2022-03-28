/*  ---------------------------------------------------------------------
 *  Copyright (c) 2022 ISB
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
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace eced.GameConfig
{
    public class GameConfiguration
    {
        public string ConfigPath { get; }
        public string Name { get; private set; }
        public bool UsesVSwap { get; private set; }
        public string VSwapExtension { get; private set; }
        public TileManager TilePalette { get; }
        public TriggerManager TriggerPalette { get; }
        public ThingManager ThingPalette { get; }
        public VSwapNames VSwapNameList { get; }

        public GameConfiguration(string pathToConfig)
        {
            ConfigPath = pathToConfig;
            Name = "Unnamed config";
            UsesVSwap = false;
            ReadConfigFile();

            TilePalette = new TileManager();
            TriggerPalette = new TriggerManager();
            ThingPalette = new ThingManager();
            VSwapNameList = new VSwapNames();

            TilePalette.LoadPalette(Path.Combine(ConfigPath, "tiles.xml"));
            TriggerPalette.LoadTriggerDefinitions(Path.Combine(ConfigPath, "triggers.xml"));
            ThingPalette.LoadThingDefintions(Path.Combine(ConfigPath, "actors.xml"));
            if (UsesVSwap)
                VSwapNameList.LoadVSwapNames(Path.Combine(ConfigPath, "vswap.xml"));
        }

        private void ReadConfigFile()
        {
            StreamReader sr = new StreamReader(Path.Combine(ConfigPath, "config.xml"));
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
                            //Console.WriteLine("Sub Node type: {0}, Data: {1}, Type: {2}", subnode.NodeType.ToString(), subnode.ToString(), subnode.GetType());

                            //TODO: this is kinda disgusting, I need to improve my XML usage.
                            if (subnode is XElement)
                            {
                                XElement elem = (XElement)subnode;

                                switch (elem.Name.LocalName)
                                {
                                    case "name":
                                        Name = elem.Value;
                                        break;
                                    case "vswapext":
                                        UsesVSwap = true;
                                        VSwapExtension = elem.Value;
                                        break;
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
            }
        }
    }
}
