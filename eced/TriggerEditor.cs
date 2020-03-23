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
    public partial class TriggerEditor : Form
    {
        private TriggerList triggerList;
        private TriggerManager typelist = new TriggerManager();
        public bool locked = false;
        public TriggerEditor(TriggerList triggerList, TriggerManager typelist)
        {
            InitializeComponent();
            this.triggerList = triggerList;
            this.typelist = typelist;
        }

        private void TriggerEditor_Load(object sender, EventArgs e)
        {
            UpdateCells();
            this.TriggerComboBox.Items.Clear();
            SetTriggerList(typelist);
        }

        public void SetTriggerList(TriggerManager triggerManager)
        {
            TriggerComboBox.Items.Clear();
            string[] nameArray = new string[triggerManager.triggerList.Count];
            for (int i = 0; i < nameArray.Length; i++)
            {
                nameArray[i] = triggerManager.triggerList[i].name;
            }
            TriggerComboBox.Items.AddRange(nameArray);
        }

        private void UpdateCells()
        {
            cbTriggerList.Items.Clear();
            for (int i = 0; i < triggerList.Triggers.Count; i++)
            {
                cbTriggerList.Items.Add(i);
            }
            if (triggerList.Triggers.Count == 0)
            {
                cbTriggerList.SelectedIndex = -1;
            }
            else
                cbTriggerList.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Trigger newTrigger = new Trigger();
            newTrigger.x = triggerList.pos.x;
            newTrigger.y = triggerList.pos.y;
            newTrigger.z = triggerList.pos.z;
            triggerList.Triggers.Add(newTrigger);
            UpdateCells();

            if (cbTriggerList.SelectedIndex < 0)
                cbTriggerList.SelectedIndex = 0;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (triggerList.Triggers.Count > 0)
            {
                triggerList.Triggers.RemoveAt(cbTriggerList.SelectedIndex);
                cbTriggerList.Items.RemoveAt(cbTriggerList.SelectedIndex);
                if (cbTriggerList.Items.Count == 0)
                    cbTriggerList.SelectedIndex = -1;
            }
        }

        private void triggerThingFlags(object sender, EventArgs e)
        {
            if (locked) return;
            if (cbTriggerList.SelectedIndex > -1)
            {
                Trigger editTrigger = triggerList.Triggers[cbTriggerList.SelectedIndex];

                editTrigger.acte = cbTrigEast.Checked;
                editTrigger.actn = cbTrigNorth.Checked;
                editTrigger.acts = cbTrigSouth.Checked;
                editTrigger.actw = cbTrigWest.Checked;

                editTrigger.usemonster = cbUseMonst.Checked;
                editTrigger.useplayer = cbUse.Checked;
                editTrigger.cross = cbCross.Checked;

                editTrigger.repeat = cbRepeat.Checked;
                editTrigger.secret = cbSecret.Checked;
            }
        }

        private void triggerParams(object sender, EventArgs e)
        {
            if (locked) return;
            if (cbTriggerList.SelectedIndex > -1)
            {
                Trigger editTrigger = triggerList.Triggers[cbTriggerList.SelectedIndex];

                editTrigger.arg0 = (int)ndParam1.Value;
                editTrigger.arg1 = (int)ndParam2.Value;
                editTrigger.arg2 = (int)ndParam3.Value;
                editTrigger.arg3 = (int)ndParam4.Value;
                editTrigger.arg4 = (int)ndParam5.Value;

                triggerList.Triggers[cbTriggerList.SelectedIndex] = editTrigger;
            }
        }

        private void cbTriggerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTriggerList.SelectedIndex < 0) return;
            locked = true;

            Trigger editTrigger = triggerList.Triggers[cbTriggerList.SelectedIndex];

            cbTrigEast.Checked = editTrigger.acte;
            cbTrigNorth.Checked = editTrigger.actn;
            cbTrigSouth.Checked = editTrigger.acts;
            cbTrigWest.Checked = editTrigger.actw;

            cbUse.Checked = editTrigger.useplayer;
            cbUseMonst.Checked = editTrigger.usemonster;
            cbCross.Checked = editTrigger.cross;

            cbRepeat.Checked = editTrigger.repeat;
            cbSecret.Checked = editTrigger.secret;

            ndParam1.Value = editTrigger.arg0;
            ndParam2.Value = editTrigger.arg1;
            ndParam3.Value = editTrigger.arg2;
            ndParam4.Value = editTrigger.arg3;
            ndParam5.Value = editTrigger.arg4;

            TriggerComboBox.Text = editTrigger.type;

            locked = false;
        }

        private void cbTriggerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!locked)
            {
                Trigger editTrigger = triggerList.Triggers[cbTriggerList.SelectedIndex];
                editTrigger.type = TriggerComboBox.Text;
                SetTriggerArgNames();
            }
        }

        private void SetTriggerArgNames()
        {
            if (typelist.triggerMapping.ContainsKey(TriggerComboBox.Name))
            {
                TriggerType trig = typelist.triggerList[typelist.triggerMapping[TriggerComboBox.Text]];
                Arg1Label.Text = trig.p1;
                Arg2Label.Text = trig.p2;
                Arg3Label.Text = trig.p3;
                Arg4Label.Text = trig.p4;
                Arg5Label.Text = trig.p5;
            }
            else
            {
                Arg1Label.Text = "Arg 1";
                Arg2Label.Text = "Arg 2";
                Arg3Label.Text = "Arg 3";
                Arg4Label.Text = "Arg 4";
                Arg5Label.Text = "Arg 5";
            }
        }

        private void TriggerComboBox_TextChanged(object sender, EventArgs e)
        {
            SetTriggerArgNames();
            if (!locked)
            {
                Trigger editTrigger = triggerList.Triggers[cbTriggerList.SelectedIndex];
                editTrigger.type = TriggerComboBox.Text;
            }
        }
    }
}
