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

using eced.ResourceFiles;
using eced.GameConfig;

namespace eced
{
    public partial class NewMapDialog : Form
    {
        //private MapInformation currentMapInfo = new MapInformation();
        public MapInformation CurrentMap { get; } = new MapInformation();
        private EditorState editorState;
        public NewMapDialog(EditorState state)
        {
            InitializeComponent();
            editorState = state;

            //This assumes there's more than 0 configurations, but if you get here without that something is seriously wrong.
            GameConfigurationComboBox.Items.Clear();
            foreach (GameConfiguration configuration in editorState.Configurations)
            {
                GameConfigurationComboBox.Items.Add(configuration.Name);
            }
            GameConfigurationComboBox.SelectedIndex = 0;
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
                CurrentMap.files.Add((ResourceFiles.ArchiveHeader)listBox1.Items[i]);
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
            if (CurrentMap.gameConfiguration.UsesVSwap)
                resourceDialog.EnableVSwap(CurrentMap.gameConfiguration.VSwapExtension);

            ArchiveHeader resource;
            
            if (resourceDialog.ShowDialog() == DialogResult.OK)
            {
                resource = resourceDialog.CurrentArchive;
                if (resource.filename != "")
                    listBox1.Items.Add(resource);
            }

            resourceDialog.Dispose();
        }

        bool recursiveHack = false;
        private void GameConfigurationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Hack to handle VSwap archives. These are super configuration-based and likely can't be used between configurations.
            if (!recursiveHack)
            {
                foreach (object hack in listBox1.Items)
                {
                    if (hack is ArchiveHeader)
                    {
                        ArchiveHeader superhack = (ArchiveHeader)hack;
                        if (superhack.format == ResourceFormat.VSwap)
                        {
                            DialogResult res = MessageBox.Show("VSWAP resources are loaded, which may not be compatible with the new configuration. Changing the config will clear these resources. Do you want to continue?",
                                "VSwap info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (res == DialogResult.Yes)
                            {
                                listBox1.Items.Clear();
                                CurrentMap.gameConfiguration = editorState.Configurations[GameConfigurationComboBox.SelectedIndex];
                            }
                            else
                            {
                                recursiveHack = true;
                                GameConfigurationComboBox.SelectedIndex = editorState.Configurations.IndexOf(CurrentMap.gameConfiguration);
                                recursiveHack = false;
                            }
                            return;
                        }
                    }
                }
            }

            CurrentMap.gameConfiguration = editorState.Configurations[GameConfigurationComboBox.SelectedIndex];
        }
    }
}
