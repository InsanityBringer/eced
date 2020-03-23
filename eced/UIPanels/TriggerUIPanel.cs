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
using System.Windows.Forms;
using eced.Brushes;

namespace eced.UIPanels
{
    public partial class TriggerUIPanel : UserControl
    {
        TriggerManager triggerList;
        TriggerBrush pairedBrush;
        public TriggerUIPanel()
        {
            InitializeComponent();
        }

        public void SetPairedBrush(TriggerBrush brush)
        {
            pairedBrush = brush;
        }

        public void SetTriggerList(TriggerManager triggers)
        {
            triggerList = triggers;
            TriggerComboBox.Items.Clear();
            string[] triggerNames = new string[triggers.triggerList.Count];

            for (int i = 0; i < triggerNames.Length; i++)
            {
                triggerNames[i] = triggers.triggerList[i].name;
            }
            TriggerComboBox.Items.AddRange(triggerNames);
            TriggerComboBox.SelectedIndex = 0;
        }

        public void SetTrigger(string trigger)
        {
            if (triggerList == null) return;

            if (triggerList.triggerMapping.ContainsKey(trigger))
            {
                TriggerType newTrigger = triggerList.triggerList[triggerList.triggerMapping[trigger]];
                Arg1Label.Text = newTrigger.p1;
                Arg2Label.Text = newTrigger.p2;
                Arg3Label.Text = newTrigger.p3;
                Arg4Label.Text = newTrigger.p4;
                Arg5Label.Text = newTrigger.p5;
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

        private void ParamSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (pairedBrush == null) return;
            pairedBrush.trigger.arg0 = (int)Arg1Spinner.Value;
            pairedBrush.trigger.arg1 = (int)Arg2Spinner.Value;
            pairedBrush.trigger.arg2 = (int)Arg3Spinner.Value;
            pairedBrush.trigger.arg3 = (int)Arg4Spinner.Value;
            pairedBrush.trigger.arg4 = (int)Arg5Spinner.Value;
        }

        private void DirectionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (pairedBrush == null) return;
            pairedBrush.trigger.actn = NorthCheckbox.Checked;
            pairedBrush.trigger.acte = EastCheckbox.Checked;
            pairedBrush.trigger.acts = SouthCheckbox.Checked;
            pairedBrush.trigger.actw = WestCheckbox.Checked;
        }

        private void ActivationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (pairedBrush == null) return;
            pairedBrush.trigger.usemonster = MonsterCheckbox.Checked;
            pairedBrush.trigger.useplayer = PlayerUseCheckbox.Checked;
            pairedBrush.trigger.cross = PlayerCrossCheckbox.Checked;
        }

        private void RepeatCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (pairedBrush == null) return;
            pairedBrush.trigger.repeat = RepeatCheckbox.Checked;
            pairedBrush.trigger.secret = SecretCheckbox.Checked;
        }

        private void TriggerComboBox_TextChanged(object sender, EventArgs e)
        {
            if (pairedBrush == null) return;
            pairedBrush.trigger.type = TriggerComboBox.Text;
            SetTrigger(TriggerComboBox.Text);
        }
    }
}
