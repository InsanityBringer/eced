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
    public partial class ZoneUIPanel : UserControl
    {
        private FloodBrush pairedBrush;
        private EditorState state; //need state for color chart
        private int lastzone = -1;
        public ZoneUIPanel(EditorState state)
        {
            InitializeComponent();
            this.state = state;
        }

        public void SetPairedBrush(FloodBrush brush)
        {
            pairedBrush = brush;
        }

        public void SetZones(List<Zone> zones)
        {
            ZoneListView.Items.Clear();
            ZoneListView.Items.Add(new ListViewItem("Automatic"));
            ListViewItem item;
            for (int x = 0; x < zones.Count; x++)
            {
                item = new ListViewItem(string.Format("Zone {0}", x));
                item.BackColor = Color.FromArgb(state.Colors.Colors[x]);
                ZoneListView.Items.Add(item);
            }
        }

        public void UpdateZoneHighlight(int newzone)
        {
            if (newzone != lastzone) //don't cause too many updates
            {
                lastzone = newzone;
                if (newzone == -1)
                {
                    HighlightedLabel.BackColor = Color.FromKnownColor(KnownColor.Control);
                    HighlightedLabel.Text = "Unzoned";
                }
                else
                {
                    HighlightedLabel.BackColor = Color.FromArgb(state.Colors.Colors[newzone]);
                    HighlightedLabel.Text = string.Format("Zone {0}", newzone);
                }
            }
        }

        private void ZoneListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ZoneListView.SelectedIndices.Count > 0)
                pairedBrush.setCode = ZoneListView.SelectedIndices[0] - 1;
        }

        private void ZoneListView_Resize(object sender, EventArgs e)
        {
            //Would you look at the time, it's already hack-o-clock!
            ZoneListView.Columns[0].Width = ZoneListView.Width - ZoneListView.Margin.Right - ZoneListView.Margin.Left;
        }
    }
}
