using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eced
{
    public partial class TriggerEditor : Form
    {
        private Cell cell;
        int cx, cy;
        private TriggerTypeList typelist = new TriggerTypeList();
        public bool locked = false;
        public TriggerEditor(ref Cell cell, int cx, int cy, int cz)
        {
            InitializeComponent();
            this.cell = cell;
            this.cx = cx;
            this.cy = cy;
        }

        private void TriggerEditor_Load(object sender, EventArgs e)
        {
            updateCells();
            this.cbTriggerType.Items.Clear();
            typelist.fillOutComboBox(ref this.cbTriggerType);
        }

        private void updateCells()
        {
            cbTriggerList.Items.Clear();
            for (int i = 0; i < cell.triggerList.Count; i++)
            {
                cbTriggerList.Items.Add(i);
            }
            if (cell.triggerList.Count == 0)
            {
                cbTriggerList.SelectedIndex = -1;
            }
            else
                cbTriggerList.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Trigger newTrigger = new Trigger();
            newTrigger.x = cx;
            newTrigger.y = cy;
            cell.triggerList.Add(newTrigger);
            updateCells();

            if (cbTriggerList.SelectedIndex < 0)
                cbTriggerList.SelectedIndex = 0;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cell.triggerList.Count > 0)
            {
                cell.triggerList.RemoveAt(cbTriggerList.SelectedIndex);
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
                Trigger editTrigger = cell.triggerList[cbTriggerList.SelectedIndex];

                editTrigger.acte = cbTrigEast.Checked;
                editTrigger.actn = cbTrigNorth.Checked;
                editTrigger.acts = cbTrigSouth.Checked;
                editTrigger.actw = cbTrigWest.Checked;

                editTrigger.usemonster = cbUseMonst.Checked;
                editTrigger.useplayer = cbUse.Checked;
                editTrigger.cross = cbCross.Checked;

                editTrigger.repeat = cbRepeat.Checked;
                editTrigger.secret = cbSecret.Checked;

                cell.triggerList[cbTriggerList.SelectedIndex] = editTrigger;
            }
        }

        private void triggerParams(object sender, EventArgs e)
        {
            if (locked) return;
            if (cbTriggerList.SelectedIndex > -1)
            {
                Trigger editTrigger = cell.triggerList[cbTriggerList.SelectedIndex];

                editTrigger.arg0 = (int)ndParam1.Value;
                editTrigger.arg1 = (int)ndParam2.Value;
                editTrigger.arg2 = (int)ndParam3.Value;
                editTrigger.arg3 = (int)ndParam4.Value;
                editTrigger.arg4 = (int)ndParam5.Value;

                cell.triggerList[cbTriggerList.SelectedIndex] = editTrigger;
            }
        }

        private void cbTriggerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTriggerList.SelectedIndex < 0) return;
            locked = true;

            Trigger editTrigger = cell.triggerList[cbTriggerList.SelectedIndex];

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

            cbTriggerType.SelectedIndex = (editTrigger.action - 1);

            locked = false;
        }

        private void cbTriggerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!locked)
            {
                Trigger editTrigger = cell.triggerList[cbTriggerList.SelectedIndex];
                editTrigger.action = (cbTriggerType.SelectedIndex + 1);
            }
        }

        public Cell getNewCell()
        {
            return this.cell;
        }
    }
}
