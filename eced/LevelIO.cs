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
using CodeImp.DoomBuilder.IO;

namespace eced
{
    class LevelIO
    {
        public static Level makeNewLevel(UniversalCollection data)
        {
            int width = UWMFSearch.getIntTag(data, "width", 64);
            int height = UWMFSearch.getIntTag(data, "height", 64);
            Level level = new Level(width, height, 1, null);
            for (int x = 0; x < data.Count; x++)
            {
                UniversalEntry entry = data[x];

                switch (entry.Key)
                {
                    case "tile":
                        Console.WriteLine("processing tile");
                        Tile tile = Tile.Deserialize((UniversalCollection)entry.Value);
                        level.AddTile(tile);
                        break;
                    case "sector":
                        Console.WriteLine("processing sector");
                        Sector sector = Sector.Deserialize((UniversalCollection)entry.Value);
                        //no sector management, heh
                        break;
                    case "plane":
                        Console.WriteLine("processing plane");
                        Plane plane = Plane.Reconstruct(level, (UniversalCollection)entry.Value);
                        //no plane management, heh
                        break;
                    case "zone":
                        Console.WriteLine("processing zone");
                        level.AddZone(new Zone());
                        break;
                    case "thing":
                        Console.WriteLine("processing thing");
                        Thing thing = Thing.Reconstruct((UniversalCollection)entry.Value);
                        level.AddThing(thing);
                        break;
                    case "trigger":
                        Console.WriteLine("processing trigger");
                        Trigger trigger = Trigger.Reconstruct((UniversalCollection)entry.Value);
                        level.AddTrigger(trigger.x, trigger.y, trigger.z, trigger);
                        break;
                    case "planemap": //special handling ahoy
                        Console.WriteLine("processing planemap");
                        level.SetTempPlaneMap((List<NumberCell>)UWMFSearch.findElement("planedata", (UniversalCollection)entry.Value).Value);
                        break;
                }
            }

            level.ProcessPlanemap();

            return level;
        }
    }
}
