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
    public static class LumpClassifier
    {
        static List<LumpFormat> formats;

        public static void Init()
        {
            formats = new List<LumpFormat>();

            //This serves as the registry of detected lump formats.
            //For the moment, this is just image formats used by ImageDecoder.
            formats.Add(new PNGLumpFormat());
            formats.Add(new PatchLumpFormat());
        }

        public static void Classify(ResourceFile lump, byte[] data)
        {
            foreach (LumpFormat format in formats)
            {
                if (format.Classify(lump, data))
                    return;
            }
        }
    }
}
