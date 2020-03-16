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
        public EditorBrush pairedBrush;
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

            isLocked = false;
        }

        private void TilePaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTileFromPalette(TilePaletteComboBox.SelectedIndex);
        }
    }
}
