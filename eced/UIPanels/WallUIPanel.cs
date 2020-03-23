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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using eced.Brushes;

namespace eced.UIPanels
{
    public partial class WallUIPanel : UserControl
    {
        public Tile CurrentTile { get; private set; } = new Tile();
        private EditorBrush pairedBrush;
        private List<Tile> palette;
        private bool isLocked = false;
        public WallUIPanel()
        {
            InitializeComponent();
        }

        public void SetPairedBrush(EditorBrush brush)
        {
            pairedBrush = brush;
            if (brush.normalTile != null)
            {
                isLocked = true;
                NorthTexTextBox.Text = brush.normalTile.NorthTex;
                SouthTexTextBox.Text = brush.normalTile.SouthTex;
                EastTexTextBox.Text = brush.normalTile.EastTex;
                WestTexTextBox.Text = brush.normalTile.WestTex;
                MapTexTextBox.Text = brush.normalTile.MapTex;
                NorthBlockCheckBox.Checked = brush.normalTile.NorthBlock;
                SouthBlockCheckBox.Checked = brush.normalTile.SouthBlock;
                EastBlockCheckBox.Checked = brush.normalTile.EastBlock;
                WestBlockCheckBox.Checked = brush.normalTile.WestBlock;
                InsetHorizontalCheckBox.Checked = brush.normalTile.HorizOffset;
                InsetVerticalCheckBox.Checked = brush.normalTile.VerticalOffset;
                SndSeqTextBox.Text = brush.normalTile.SoundSequence;
                isLocked = false;
            }
            else
            {
                TilePaletteComboBox.SelectedIndex = 0;
            }
        }

        public void SetPalette(List<Tile> tiles)
        {
            palette = tiles;

            string[] names = new string[tiles.Count];
            for (int i = 0; i < tiles.Count; i++)
            {
                names[i] = tiles[i].Name;
            }
            TilePaletteComboBox.Items.Clear();
            TilePaletteComboBox.Items.AddRange(names);
        }

        private void TileTexture_TextChanged(object sender, EventArgs e)
        {
            if (isLocked) return;

            pairedBrush.normalTile = pairedBrush.normalTile.ChangeTextures(NorthTexTextBox.Text, SouthTexTextBox.Text, EastTexTextBox.Text, WestTexTextBox.Text, MapTexTextBox.Text);
        }

        private void TileBlocking_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;

            pairedBrush.normalTile = pairedBrush.normalTile.ChangeBlocking(NorthBlockCheckBox.Checked, SouthBlockCheckBox.Checked, EastBlockCheckBox.Checked, WestBlockCheckBox.Checked);
        }

        private void InsetHorizontalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;

            pairedBrush.normalTile = pairedBrush.normalTile.ChangeInset(InsetHorizontalCheckBox.Checked, InsetVerticalCheckBox.Checked);
        }

        public void SetTileFromPalette(int id)
        {
            isLocked = true;
            pairedBrush.normalTile = palette[id];

            NorthTexTextBox.Text = pairedBrush.normalTile.NorthTex;
            SouthTexTextBox.Text = pairedBrush.normalTile.SouthTex;
            EastTexTextBox.Text = pairedBrush.normalTile.EastTex;
            WestTexTextBox.Text = pairedBrush.normalTile.WestTex;
            MapTexTextBox.Text = pairedBrush.normalTile.MapTex;

            NorthBlockCheckBox.Checked = pairedBrush.normalTile.NorthBlock;
            SouthBlockCheckBox.Checked = pairedBrush.normalTile.SouthBlock;
            EastBlockCheckBox.Checked = pairedBrush.normalTile.EastBlock;
            WestBlockCheckBox.Checked = pairedBrush.normalTile.WestBlock;

            InsetHorizontalCheckBox.Checked = pairedBrush.normalTile.HorizOffset;
            InsetVerticalCheckBox.Checked = pairedBrush.normalTile.VerticalOffset;

            SndSeqTextBox.Text = pairedBrush.normalTile.SoundSequence;

            isLocked = false;
        }

        private void TilePaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTileFromPalette(TilePaletteComboBox.SelectedIndex);
        }

        private void SndSeqTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked) return;

            pairedBrush.normalTile = pairedBrush.normalTile.ChangeSoundSequence(SndSeqTextBox.Text);
        }
    }
}
