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
using System.Windows.Forms;

namespace eced
{
    public partial class NewMapDialog : Form
    {
        //private MapInformation currentMapInfo = new MapInformation();
        public MapInformation CurrentMap { get; } = new MapInformation();
        public NewMapDialog()
        {
            InitializeComponent();
        }

        private void txtMapName_TextChanged(object sender, EventArgs e)
        {
            CurrentMap.lumpname = ((TextBox)sender).Text;
        }

        private void nudXSize_ValueChanged(object sender, EventArgs e)
        {
            CurrentMap.sizex = (int)((NumericUpDown)sender).Value;
        }

        private void nudYSize_ValueChanged(object sender, EventArgs e)
        {
            CurrentMap.sizey = (int)((NumericUpDown)sender).Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                CurrentMap.files.Add((ResourceFiles.ResourceArchiveHeader)listBox1.Items[i]);
            }
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void nudLayerCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentMap.layers = (int)((NumericUpDown)sender).Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddResourceDialog resourceDialog = new AddResourceDialog();
            if (resourceDialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(resourceDialog.getResource());
            }

            resourceDialog.Dispose();
        }
    }
}
