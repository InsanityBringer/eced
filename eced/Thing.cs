﻿/*  ---------------------------------------------------------------------
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
    [Flags]
    public enum ThingFlagsBits
    {
        FLAG_AMBUSH = 1,
        FLAG_PATROL = 2,
        FLAG_SKILL1 = 4,
        FLAG_SKILL2 = 8,
        FLAG_SKILL3 = 16,
        FLAG_SKILL4 = 32
    }

    /// <summary>
    /// Convenience class for setting flags
    /// </summary>
    public class ThingFlags //this is the most useless thing ever...
    {
        public int angle;
        public bool ambush = false, patrol = false, skill1 = true, skill2 = true, skill3 = true, skill4 = true;

        public void getFlagsFromInt(int flags)
        {
            ambush = CheckFlags(ThingFlagsBits.FLAG_AMBUSH, flags);
            patrol = CheckFlags(ThingFlagsBits.FLAG_PATROL, flags);
            skill1 = CheckFlags(ThingFlagsBits.FLAG_SKILL1, flags);
            skill2 = CheckFlags(ThingFlagsBits.FLAG_SKILL2, flags);
            skill3 = CheckFlags(ThingFlagsBits.FLAG_SKILL3, flags);
            skill4 = CheckFlags(ThingFlagsBits.FLAG_SKILL4, flags);
        }

        public int getFlags()
        {
            int flags = 0;
            if (ambush)
                flags |= (int)ThingFlagsBits.FLAG_AMBUSH;
            if (patrol)
                flags |= (int)ThingFlagsBits.FLAG_PATROL;
            if (skill1)
                flags |= (int)ThingFlagsBits.FLAG_SKILL1;
            if (skill2)
                flags |= (int)ThingFlagsBits.FLAG_SKILL2;
            if (skill3)
                flags |= (int)ThingFlagsBits.FLAG_SKILL3;
            if (skill4)
                flags |= (int)ThingFlagsBits.FLAG_SKILL4;

            return flags;
        }

        public bool CheckFlags(ThingFlagsBits flag, int value)
        {
            return (value & (int)flag) != 0;
        }
    }

    public class Thing
    {
        public float x, y, z;

        public int angle;
        public string type;
        public int flags;

        public UniversalCollection allkeys;

        public bool highlighted = false;
        public bool selected = false;
        public bool moving = false;

        public bool ambush = false;
        public bool patrol = false;
        public bool skill1 = true;
        public bool skill2 = true;
        public bool skill3 = true;
        public bool skill4 = true;

        public string Serialize()
        {
            StringBuilder stringmaker = new StringBuilder();
            stringmaker.Append("thing\n{\n");
            stringmaker.AppendFormat("\tx = {0:N2};", x);
            stringmaker.AppendFormat("\n\ty = {0:N2};", y);
            stringmaker.AppendFormat("\n\tz = {0:N2};", z);

            stringmaker.Append("\n\tangle = "); stringmaker.Append(angle); stringmaker.Append(";");
            stringmaker.Append("\n\ttype = \""); stringmaker.Append(type); stringmaker.Append("\";");
            stringmaker.Append("\n\tambush = "); stringmaker.Append(ambush.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tpatrol = "); stringmaker.Append(patrol.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tskill1 = "); stringmaker.Append(skill1.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tskill2 = "); stringmaker.Append(skill2.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tskill3 = "); stringmaker.Append(skill3.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tskill4 = "); stringmaker.Append(skill4.ToString().ToLower()); stringmaker.Append(";");

            stringmaker.Append("\n}");

            return stringmaker.ToString();
        }

        public bool checkFlag(ThingFlagsBits flag)
        {
            return (flags & (int)flag) != 0;
        }

        public static Thing Reconstruct(UniversalCollection data)
        {
            Thing thing = new Thing();

            thing.allkeys = data;

            thing.x = (float)UWMFSearch.getFloatTag(data, "x", 0.0f);
            thing.y = (float)UWMFSearch.getFloatTag(data, "y", 0.0f);
            thing.z = (float)UWMFSearch.getFloatTag(data, "z", 0.0f);

            thing.angle = UWMFSearch.getIntTag(data, "angle", 0);

            thing.type = UWMFSearch.getStringTag(data, "type", "Unknown");

            thing.ambush = UWMFSearch.getBoolTag(data, "ambush", false);
            thing.patrol = UWMFSearch.getBoolTag(data, "patrol", false);
            thing.skill1 = UWMFSearch.getBoolTag(data, "skill1", false);
            thing.skill2 = UWMFSearch.getBoolTag(data, "skill2", false);
            thing.skill3 = UWMFSearch.getBoolTag(data, "skill3", false);
            thing.skill4 = UWMFSearch.getBoolTag(data, "skill4", false);

            return thing;
        }
    }
}
