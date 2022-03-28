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

using eced.ResourceFiles;
using eced.GameConfig;

namespace eced
{
    public partial class OpenMapDialog : Form
    {
        public MapInformation CurrentMap { get; } = new MapInformation();
        public int CurrentMapIndex { get; private set; }
        private WADArchive archive;
        private List<int> mapIndicies;
        private EditorState editorState;
        public OpenMapDialog(WADArchive archive, List<int> mapIndicies, EditorState state)
        {
            this.archive = archive;
            this.mapIndicies = mapIndicies;
            InitializeComponent();
            for (int i = 0; i < mapIndicies.Count; i++)
            {
                MapsListBox.Items.Add(archive.lumps[mapIndicies[i]].name);
            }
            MapsListBox.SelectedIndex = 0;
            editorState = state;

            //This assumes there's more than 0 configurations, but if you get here without that something is seriously wrong.
            GameConfigurationComboBox.Items.Clear();
            foreach (GameConfiguration configuration in editorState.Configurations)
            {
                GameConfigurationComboBox.Items.Add(configuration.Name);
            }
            GameConfigurationComboBox.SelectedIndex = 0;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddResourceDialog resourceDialog = new AddResourceDialog();
            if (CurrentMap.gameConfiguration.UsesVSwap)
                resourceDialog.EnableVSwap(CurrentMap.gameConfiguration.VSwapExtension);

            ArchiveHeader resource;

            if (resourceDialog.ShowDialog() == DialogResult.OK)
            {
                resource = resourceDialog.CurrentArchive;
                if (resource.filename != "")
                    ResourceListBox.Items.Add(resource);
            }

            resourceDialog.Dispose();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (ResourceListBox.SelectedIndex >= 0)
                ResourceListBox.Items.RemoveAt(ResourceListBox.SelectedIndex);
        }

        private void OpenMapDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                ArchiveHeader header = new ArchiveHeader();
                header.filename = archive.filename;
                header.format = ResourceFormat.Wad;
                CurrentMap.files.Add(header);

                for (int i = 0; i < ResourceListBox.Items.Count; i++)
                {
                    if (ResourceListBox.Items[i] is ArchiveHeader)
                    {
                        CurrentMap.files.Add((ArchiveHeader)ResourceListBox.Items[i]);
                    }
                }
                CurrentMapIndex = mapIndicies[MapsListBox.SelectedIndex];
            }
        }

        bool recursiveHack = false;
        private void GameConfigurationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Hack to handle VSwap archives. These are super configuration-based and likely can't be used between configurations.
            if (!recursiveHack)
            {
                foreach (object hack in ResourceListBox.Items)
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
                                ResourceListBox.Items.Clear();
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
