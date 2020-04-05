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
using System.Collections.Generic;
using System.Windows.Forms;

using eced.GameConfig;

namespace eced
{
    public partial class ThingEditor : Form
    {
        public List<Thing> thingList;
        public ThingFlags flags;
        private EditorState state;
        private ThingManager thinglist;
        private bool lockangle = false;
        private bool angleChanged = false; //hack needed since spinner can't not have a value
        private bool isLocked = false;

        public ThingEditor(EditorState state, List<Thing> thingToEdit)
        {
            InitializeComponent();
            this.thingList = thingToEdit;
            this.state = state;

            isLocked = true;
            this.thinglist = state.ThingList;

            foreach (ThingDefinition thingDef in thinglist.thingList)
            {
                ThingTypeComboBox.Items.Add(thingDef.Type);
            }

            //TODO: Make this less terrible
            AmbushCheckBox.Checked = thingList[0].ambush;
            PatrolCheckBox.Checked = thingList[0].patrol;
            Skill1CheckBox.Checked = thingList[0].skill1;
            Skill2CheckBox.Checked = thingList[0].skill2;
            Skill3CheckBox.Checked = thingList[0].skill3;
            Skill4CheckBox.Checked = thingList[0].skill4;
            XPosTextBox.Text = ((int)(thingList[0].x * state.CurrentLevel.TileSize)).ToString();
            YPosTextBox.Text = ((int)(thingList[0].y * state.CurrentLevel.TileSize)).ToString();
            ZPosTextBox.Text = ((int)(thingList[0].z * state.CurrentLevel.TileSize)).ToString();
            ThingTypeComboBox.Text = thingList[0].type;
            //having multiple loops here isn't great, but it means they can break easier and no state has to be saved
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].ambush != AmbushCheckBox.Checked)
                {
                    AmbushCheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].patrol != PatrolCheckBox.Checked)
                {
                    PatrolCheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].skill1 != Skill1CheckBox.Checked)
                {
                    Skill1CheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].skill2 != Skill2CheckBox.Checked)
                {
                    Skill2CheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].skill3 != Skill3CheckBox.Checked)
                {
                    Skill3CheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].skill4 != Skill4CheckBox.Checked)
                {
                    Skill4CheckBox.CheckState = CheckState.Indeterminate;
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (((int)(thingList[i].x * state.CurrentLevel.TileSize)).ToString() != XPosTextBox.Text)
                {
                    XPosTextBox.Text = "";
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (((int)(thingList[i].y * state.CurrentLevel.TileSize)).ToString() != YPosTextBox.Text)
                {
                    YPosTextBox.Text = "";
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (((int)(thingList[i].z * state.CurrentLevel.TileSize)).ToString() != ZPosTextBox.Text)
                {
                    ZPosTextBox.Text = "";
                    break;
                }
            }
            for (int i = 1; i < thingList.Count; i++)
            {
                if (thingList[i].type != ThingTypeComboBox.Text)
                {
                    ThingTypeComboBox.Text = "";
                    break;
                }
            }
            isLocked = false;

            //TODO: I don't think you can't have a value on a spinner, so
            this.ThingAngleSpinner.Value = thingList[0].angle;
        }

        private void thingBitChange(object sender, EventArgs e)
        {
        }

        private void ndThingAngle_ValueChanged(object sender, EventArgs e)
        {
            angleChanged = true;
            if (this.ThingAngleSpinner.Value == 360)
                this.ThingAngleSpinner.Value = 0;

            rbThingEast.Checked = false;
            rbThingNorth.Checked = false;
            rbThingWest.Checked = false;
            rbThingSouth.Checked = false;
            rbThingNE.Checked = false;
            rbThingNW.Checked = false;
            rbThingSW.Checked = false;
            rbThingSE.Checked = false;

            this.lockangle = true;

            if (ThingAngleSpinner.Value == 0)
                this.rbThingEast.Checked = true;
            else if (ThingAngleSpinner.Value == 90)
                this.rbThingNorth.Checked = true;
            else if (ThingAngleSpinner.Value == 180)
                this.rbThingWest.Checked = true;
            else if (ThingAngleSpinner.Value == 270)
                this.rbThingSouth.Checked = true;
            else if (ThingAngleSpinner.Value == 45)
                this.rbThingNE.Checked = true;
            else if (ThingAngleSpinner.Value == 135)
                this.rbThingNW.Checked = true;
            else if (ThingAngleSpinner.Value == 225)
                this.rbThingSW.Checked = true;
            else if (ThingAngleSpinner.Value == 315)
                this.rbThingSE.Checked = true;

            lockangle = false;
        }

        private void rbThingEast_CheckedChanged(object sender, EventArgs e)
        {
            angleChanged = true;
            if (lockangle) return;
            if (rbThingEast.Checked)
                this.ThingAngleSpinner.Value = 0;
            else if (rbThingNorth.Checked)
                this.ThingAngleSpinner.Value = 90;
            else if (rbThingWest.Checked)
                this.ThingAngleSpinner.Value = 180;
            else if (rbThingSouth.Checked)
                this.ThingAngleSpinner.Value = 270;

            else if (rbThingNE.Checked)
                this.ThingAngleSpinner.Value = 45;
            else if (rbThingNW.Checked)
                this.ThingAngleSpinner.Value = 135;
            else if (rbThingSW.Checked)
                this.ThingAngleSpinner.Value = 225;
            else if (rbThingSE.Checked)
                this.ThingAngleSpinner.Value = 315;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ApplyChanges()
        {
            foreach (Thing thing in thingList)
            {
                int tx, ty, tz;
                if (XPosTextBox.Text != "" && int.TryParse(XPosTextBox.Text, out tx))
                    thing.x = (float)tx / state.CurrentLevel.TileSize;
                if (YPosTextBox.Text != "" && int.TryParse(YPosTextBox.Text, out ty))
                    thing.y = (float)ty / state.CurrentLevel.TileSize;
                if (ZPosTextBox.Text != "" && int.TryParse(ZPosTextBox.Text, out tz))
                    thing.z = (float)tz / state.CurrentLevel.TileSize;
                if (ThingTypeComboBox.Text != "")
                    thing.type = ThingTypeComboBox.Text;
                if (AmbushCheckBox.CheckState != CheckState.Indeterminate)
                    thing.ambush = AmbushCheckBox.Checked;
                if (PatrolCheckBox.CheckState != CheckState.Indeterminate)
                    thing.patrol = PatrolCheckBox.Checked;
                if (Skill1CheckBox.CheckState != CheckState.Indeterminate)
                    thing.skill1 = Skill1CheckBox.Checked;
                if (Skill2CheckBox.CheckState != CheckState.Indeterminate)
                    thing.skill2 = Skill2CheckBox.Checked;
                if (Skill3CheckBox.CheckState != CheckState.Indeterminate)
                    thing.skill3 = Skill3CheckBox.Checked;
                if (Skill4CheckBox.CheckState != CheckState.Indeterminate)
                    thing.skill4 = Skill4CheckBox.Checked;
                if (angleChanged)
                    thing.angle = (int)ThingAngleSpinner.Value;
            }
        }
    }
}
