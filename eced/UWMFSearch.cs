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

//Uses code from CodeImp's DoomBuilder 2 project

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeImp.DoomBuilder.IO;

namespace eced
{
    class UWMFSearch
    {
        //why is this not a dictionary
        /// <summary>
        /// Looks up a specific key in a UniversalCollection
        /// </summary>
        /// <param name="key">The key to find</param>
        /// <returns></returns>
        public static UniversalEntry findElement(string key, UniversalCollection collection)
        {
            for (int x = 0; x < collection.Count; x++)
            {
                if (collection[x].Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                    return collection[x];
            }
            return null;
        }

        public static bool getBoolTag(UniversalCollection collection, string key, bool def)
        {
            UniversalEntry entry = findElement(key, collection);

            if (entry == null)
                return def;

            return (bool)entry.Value;
        }

        public static int getIntTag(UniversalCollection collection, string key, int def)
        {
            UniversalEntry entry = findElement(key, collection);

            if (entry == null)
                return def;

            return (int)entry.Value;
        }

        public static double getFloatTag(UniversalCollection collection, string key, double def)
        {
            UniversalEntry entry = findElement(key, collection);

            if (entry == null)
                return def;

            if (entry.Value is int) //weirdness from the parser
            {
                int value = (int)entry.Value;
                return (double)value;
            }

            return (double)entry.Value;
        }

        public static string getStringTag(UniversalCollection collection, string key, string def)
        {
            UniversalEntry entry = findElement(key, collection);

            if (entry == null)
                return def;

            return (string)entry.Value;
        }
    }
}
