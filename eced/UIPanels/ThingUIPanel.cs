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
            comboBox1.Items.Clear();
            palette = things;
            string[] names = new string[things.Count];
            for (int i = 0; i < things.Count; i++)
            {
                names[i] = things[i].Type;
            }
            comboBox1.Items.AddRange(names);
            comboBox1.SelectedIndex = 0;
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
                pairedBrush.thing = palette[comboBox1.SelectedIndex];
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
    }
}
