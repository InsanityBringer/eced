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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using eced.Renderer;
using eced.ResourceFiles.Images;

namespace eced
{
    public partial class TextureBrowser : Form
    {
        private TextureView textureView;
        public TextureBrowser()
        {
            InitializeComponent();
            //AGH
            textureView = new TextureView();
            textureView.Location = SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Location;
            textureView.Size = SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Size;
            textureView.Anchor = SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Anchor;
            Controls.Add(textureView);
        }
    }

    public class TextureCache
    {
        private Dictionary<int, Bitmap> cache = new Dictionary<int, Bitmap>();
        private TextureManager textureManager;
        public ImageList CurrentImageSet { get; }

        public TextureCache(TextureManager manager)
        {
            textureManager = manager;
        }

        public void Purge()
        {
            //Need to make sure all the bitmaps are disposed of
            CurrentImageSet.Images.Clear();
            cache.Clear();
        }

        public bool RequestImage(int id, out Bitmap image)
        {
            image = new Bitmap(64, 64);
            return false;
        }
    }
}
