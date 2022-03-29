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
using System.Windows.Forms;

using eced.Brushes;

namespace eced.UIPanels
{
    public partial class SectorUIPanel : UserControl
    {
        public SectorBrush pairedBrush;

        public TextureCache Cache { get; set; }
        public SectorUIPanel()
        {
            InitializeComponent();
        }

        private void FloorTexTextBox_TextChanged(object sender, EventArgs e)
        {
            if (pairedBrush != null)
            {
                pairedBrush.currentSector = pairedBrush.currentSector.ChangeTextures(FloorTexTextBox.Text, CeilTexTextBox.Text);
            }
        }

        private void nudSectorLight_ValueChanged(object sender, EventArgs e)
        {
            if (pairedBrush != null)
            {
                pairedBrush.currentSector = pairedBrush.currentSector.ChangeLight((int)nudSectorLight.Value);
            }
        }

        private void PickFloorButton_Click(object sender, EventArgs e)
        {
            TextureBrowser browser = new TextureBrowser(Cache);
            //horrible hack
            if (sender == PickFloorButton)
                browser.TextureName = FloorTexTextBox.Text;
            else
                browser.TextureName = CeilTexTextBox.Text;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                if (sender == PickFloorButton)
                    FloorTexTextBox.Text = browser.TextureName;
                else
                    CeilTexTextBox.Text = browser.TextureName;
            }
        }
    }
}
