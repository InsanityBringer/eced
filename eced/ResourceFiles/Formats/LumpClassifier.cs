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

using System.Collections.Generic;

namespace eced.ResourceFiles.Formats
{
    public enum LumpFormatType
    {
        Generic,
        VSwapTexture,
        VSwapSprite,
        DoomPatch,
        DoomFlat,
        PNG,
    }

    public class LumpClassifierEntry
    {
        public LumpFormat classifier;
        public LumpFormatType format;

        public LumpClassifierEntry(LumpFormat classifier, LumpFormatType format)
        {
            this.classifier = classifier;
            this.format = format;
        }
    }

    public static class LumpClassifier
    {
        static List<LumpClassifierEntry> formats;

        public static void Init()
        {
            formats = new List<LumpClassifierEntry>();

            //This serves as the registry of detected lump formats.
            //For the moment, this is just image formats used by ImageDecoder.
            formats.Add(new LumpClassifierEntry(new PNGLumpFormat(), LumpFormatType.PNG));
            formats.Add(new LumpClassifierEntry(new PatchLumpFormat(), LumpFormatType.DoomPatch));
        }

        public static void Classify(ResourceFile lump, byte[] data)
        {
            foreach (LumpClassifierEntry format in formats)
            {
                if (format.classifier.Classify(lump, data))
                {
                    lump.format = format.format;
                    return;
                }
                lump.format = LumpFormatType.Generic;
            }
        }
    }
}
