using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eced
{
    public partial class MapPropertiesDialog : Form
    {
        private Level level;
        public MapPropertiesDialog(Level level)
        {
            InitializeComponent();
            this.level = level;
            ExperimentalCheckBox.Checked = level.Experimental;
            if (!ExperimentalCheckBox.Checked)
            {
                DefaultLightSpinner.Enabled = false;
                VisibilitySpinner.Enabled = false;
            }
            MapNameTextBox.Text = level.Name;
            TileSizeSpinner.Value = level.TileSize;
            DefaultLightSpinner.Value = level.Brightness;
            VisibilitySpinner.Value = (decimal)level.Visibility;
        }

        private void ExperimentalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!ExperimentalCheckBox.Checked)
            {
                DefaultLightSpinner.Enabled = false;
                VisibilitySpinner.Enabled = false;
            }
            else
            {
                DefaultLightSpinner.Enabled = true;
                VisibilitySpinner.Enabled = true;
            }
        }

        public void ApplyChanges()
        {
            level.Experimental = ExperimentalCheckBox.Checked;
            if (level.Experimental)
            {
                level.Brightness = (int)DefaultLightSpinner.Value;
                level.Visibility = (float)VisibilitySpinner.Value;
            }
            level.Name = MapNameTextBox.Text;
            level.TileSize = (int)TileSizeSpinner.Value;
        }
    }
}
