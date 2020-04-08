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
    public partial class ThingUIPanel : UserControl
    {
        private List<ThingDefinition> palette;
        public ThingBrush pairedBrush;

        private bool locked = false;
        
        public ThingUIPanel()
        {
            InitializeComponent();
        }

        public void AddThings(List<ThingDefinition> things)
        {
            ThingTypeComboBox.Items.Clear();
            palette = things;
            string[] names = new string[things.Count];
            for (int i = 0; i < things.Count; i++)
            {
                names[i] = things[i].Type;
            }
            ThingTypeComboBox.Items.AddRange(names);
            ThingTypeComboBox.SelectedIndex = 0;
        }

        public void SetPairedBrush(ThingBrush brush)
        {
            pairedBrush = brush;
            UpdateBrush();
        }

        private void UpdateBrush()
        {
            if (pairedBrush != null)
            {
                pairedBrush.flags.angle = (int)ThingAngleSpinner.Value;
                pairedBrush.ThingType = ThingTypeComboBox.Text;
            }
        }

        private void AngleRadioBox_CheckedChanged(object sender, EventArgs e)
        {
            locked = true;
            Control control = (Control)sender;
            ThingAngleSpinner.Value = decimal.Parse((string)control.Tag);
            locked = false;
            UpdateBrush();
        }

        private void ThingAngleSpinner_ValueChanged(object sender, EventArgs e)
        {
            //todo: optimize
            locked = true;
            if (ThingAngleSpinner.Value == 0)
                this.rbThingEast.Checked = true;
            else this.rbThingEast.Checked = false;

            if (ThingAngleSpinner.Value == 90)
                this.rbThingNorth.Checked = true;
            else this.rbThingNorth.Checked = false;

            if (ThingAngleSpinner.Value == 180)
                this.rbThingWest.Checked = true;
            else this.rbThingWest.Checked = false;

            if (ThingAngleSpinner.Value == 270)
                this.rbThingSouth.Checked = true;
            else this.rbThingSouth.Checked = false;

            if (ThingAngleSpinner.Value == 45)
                this.rbThingNE.Checked = true;
            else this.rbThingNE.Checked = false;

            if (ThingAngleSpinner.Value == 135)
                this.rbThingNW.Checked = true;
            else this.rbThingNW.Checked = false;

            if (ThingAngleSpinner.Value == 225)
                this.rbThingSW.Checked = true;
            else this.rbThingSW.Checked = false;

            if (ThingAngleSpinner.Value == 315)
                this.rbThingSE.Checked = true;
            else this.rbThingSE.Checked = false;
            locked = false;
            UpdateBrush();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBrush();
        }

        private void cbThingSkill4_CheckedChanged(object sender, EventArgs e)
        {
            pairedBrush.flags.skill4 = cbThingSkill4.Checked;
        }

        private void cbThingPatrol_CheckedChanged(object sender, EventArgs e)
        {
            pairedBrush.flags.patrol = cbThingPatrol.Checked;
        }

        private void cbThingSkill1_CheckedChanged(object sender, EventArgs e)
        {
            pairedBrush.flags.skill1 = cbThingSkill1.Checked;
        }

        private void cbThingSkill2_CheckedChanged(object sender, EventArgs e)
        {
            pairedBrush.flags.skill2 = cbThingSkill2.Checked;
        }

        private void cbThingSkill3_CheckedChanged(object sender, EventArgs e)
        {
            pairedBrush.flags.skill3 = cbThingSkill3.Checked;
        }

        private void cbThingAmbush_CheckedChanged(object sender, EventArgs e)
        {
            pairedBrush.flags.ambush = cbThingAmbush.Checked;
        }

        private void ThingTypeComboBox_TextChanged(object sender, EventArgs e)
        {
            UpdateBrush();
        }
    }
}
