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
        public string TextureName
        {
            get
            {
                return textureView.SelectedTextureName;
            }
            set
            {
                textureView.SelectedTextureName = value;
            }
        }
        public TextureBrowser(TextureCache cache)
        {
            InitializeComponent();
            //AGH
            textureView = new TextureView();
            textureView.Location = SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Location;
            textureView.Size = SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Size;
            textureView.Anchor = SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Anchor;
            textureView.Cache = cache;
            textureView.TextureList = cache.TexturesManager.GetTextureList();
            Controls.Add(textureView);
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            textureView.SearchString = SearchTextBox.Text;
        }
    }
}
