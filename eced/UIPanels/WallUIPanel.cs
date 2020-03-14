using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eced.UIPanels
{
    public partial class WallUIPanel : UserControl
    {
        public Tile CurrentTile { get; private set; } = new Tile();
        private List<Tile> palette;
        private bool isLocked = false;
        public WallUIPanel()
        {
            InitializeComponent();
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

            CurrentTile = CurrentTile.ChangeTextures(NorthTexTextBox.Text, SouthTexTextBox.Text, EastTexTextBox.Text, WestTexTextBox.Text, MapTexTextBox.Text);
        }

        private void TileBlocking_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;

            CurrentTile = CurrentTile.ChangeBlocking(NorthBlockCheckBox.Checked, SouthBlockCheckBox.Checked, EastBlockCheckBox.Checked, WestBlockCheckBox.Checked);
        }

        private void InsetHorizontalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;

            CurrentTile = CurrentTile.ChangeInset(InsetHorizontalCheckBox.Checked, InsetVerticalCheckBox.Checked);
        }

        private void TilePaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            CurrentTile = palette[TilePaletteComboBox.SelectedIndex];

            NorthTexTextBox.Text = CurrentTile.NorthTex;
            SouthTexTextBox.Text = CurrentTile.SouthTex;
            EastTexTextBox.Text = CurrentTile.EastTex;
            WestTexTextBox.Text = CurrentTile.WestTex;
            MapTexTextBox.Text = CurrentTile.MapTex;

            NorthBlockCheckBox.Checked = CurrentTile.NorthBlock;
            SouthBlockCheckBox.Checked = CurrentTile.SouthBlock;
            EastBlockCheckBox.Checked = CurrentTile.EastBlock;
            WestBlockCheckBox.Checked = CurrentTile.WestBlock;

            InsetHorizontalCheckBox.Checked = CurrentTile.HorizOffset;
            InsetVerticalCheckBox.Checked = CurrentTile.VerticalOffset;

            isLocked = false;
        }
    }
}
