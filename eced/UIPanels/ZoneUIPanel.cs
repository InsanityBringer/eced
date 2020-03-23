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
        public ZoneUIPanel()
        {
            InitializeComponent();
        }

        public void SetPairedBrush(FloodBrush brush)
        {
            pairedBrush = brush;
        }

        public void SetZones(List<Zone> zones)
        {
            ZoneListBox.Items.Clear();
            ZoneListBox.Items.Add("Automatic");
            for (int x = 0; x < zones.Count; x++)
            {
                ZoneListBox.Items.Add(String.Format("Zone {0}", x));
            }
        }

        private void ZoneListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pairedBrush.setCode = ZoneListBox.SelectedIndex - 1;
        }
    }
}
