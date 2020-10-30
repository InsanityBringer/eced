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
using eced.ResourceFiles.Formats;

namespace eced.ResourceFiles.Images
{
    public static class ImageDecoder
    {
        private static Dictionary<LumpFormatType, ImageCodec> decoderMap;
        public static void Init()
        {
            decoderMap = new Dictionary<LumpFormatType, ImageCodec>();
            decoderMap.Add(LumpFormatType.DoomPatch, new DoomPatchCodec());
            decoderMap.Add(LumpFormatType.DoomFlat, new DoomFlatCodec());
            decoderMap.Add(LumpFormatType.PNG, new PNGCodec());
            decoderMap.Add(LumpFormatType.VSwapTexture, new VSwapWallCodec());
            decoderMap.Add(LumpFormatType.ROTTMask, new ROTTMaskCodec());
            decoderMap.Add(LumpFormatType.ROTTPatch, new ROTTPatchCodec());
            decoderMap.Add(LumpFormatType.ROTTRaw, new VSwapWallCodec());
            decoderMap.Add(LumpFormatType.ROTTPic, new ROTTPicCodec());
        }

        public static BasicImage DecodeLump(Lump lump, byte[] data, byte[] palette)
        {
            return decoderMap[lump.format].DecodeImage(lump, data, palette);
        }
    }
}
