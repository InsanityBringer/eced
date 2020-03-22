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

using eced.GameConfig;

namespace eced
{
    public partial class ThingEditor : Form
    {
        public Thing thing;
        public ThingFlags flags;
        private ThingManager thinglist;
        private bool lockangle = false;

        public ThingEditor(Thing thingToEdit, ThingManager thinglist)
        {
            InitializeComponent();
            this.thing = thingToEdit;
            this.flags = new ThingFlags();
            flags.getFlagsFromInt(thing.flags);
            this.cbThingAmbush.Checked = flags.ambush;
            this.cbThingPatrol.Checked = flags.patrol;
            this.cbThingSkill1.Checked = flags.skill1;
            this.cbThingSkill2.Checked = flags.skill2;
            this.cbThingSkill3.Checked = flags.skill3;
            this.cbThingSkill4.Checked = flags.skill4;

            this.thinglist = thinglist;

            for (int x = 0; x < thinglist.idlist.Count; x++)
            {
                ThingDefinition lthing = thinglist.thingList[x];
                listBox1.Items.Add(lthing.Type);
            }

            this.ndThingAngle.Value = thing.angle;
        }

        private void thingBitChange(object sender, EventArgs e)
        {
            flags.ambush = this.cbThingAmbush.Checked;
            flags.patrol = this.cbThingPatrol.Checked;
            flags.skill1 = this.cbThingSkill1.Checked;
            flags.skill2 = this.cbThingSkill2.Checked;
            flags.skill3 = this.cbThingSkill3.Checked;
            flags.skill4 = this.cbThingSkill4.Checked;

            thing.flags = flags.getFlags();
        }

        private void ndThingAngle_ValueChanged(object sender, EventArgs e)
        {
            if (this.ndThingAngle.Value == 360)
                this.ndThingAngle.Value = 0;

            thing.angle = (int)this.ndThingAngle.Value;

            this.lockangle = true;

            if (thing.angle == 0)
                this.rbThingEast.Checked = true;
            else this.rbThingEast.Checked = false;

            if (thing.angle == 90)
                this.rbThingNorth.Checked = true;
            else this.rbThingNorth.Checked = false;

            if (thing.angle == 180)
                this.rbThingWest.Checked = true;
            else this.rbThingWest.Checked = false;

            if (thing.angle == 270)
                this.rbThingSouth.Checked = true;
            else this.rbThingSouth.Checked = false;

            if (thing.angle == 45)
                this.rbThingNE.Checked = true;
            else this.rbThingNE.Checked = false;

            if (thing.angle == 135)
                this.rbThingNW.Checked = true;
            else this.rbThingNW.Checked = false;

            if (thing.angle == 225)
                this.rbThingSW.Checked = true;
            else this.rbThingSW.Checked = false;

            if (thing.angle == 315)
                this.rbThingSE.Checked = true;
            else this.rbThingSE.Checked = false;

            lockangle = false;
        }

        private void rbThingEast_CheckedChanged(object sender, EventArgs e)
        {
            if (lockangle) return;
            if (rbThingEast.Checked)
                this.ndThingAngle.Value = 0;
            if (rbThingNorth.Checked)
                this.ndThingAngle.Value = 90;
            if (rbThingWest.Checked)
                this.ndThingAngle.Value = 180;
            if (rbThingSouth.Checked)
                this.ndThingAngle.Value = 270;

            if (rbThingNE.Checked)
                this.ndThingAngle.Value = 45;
            if (rbThingNW.Checked)
                this.ndThingAngle.Value = 135;
            if (rbThingSW.Checked)
                this.ndThingAngle.Value = 225;
            if (rbThingSE.Checked)
                this.ndThingAngle.Value = 315;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.thing.type = listBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
