using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eced.UIPanels
{
    public partial class ZoneUIPanel : UserControl
    {
        public ZoneUIPanel()
        {
            InitializeComponent();
        }

        public void SetZones(List<Zone> zones)
        {
            lbZoneList.Items.Clear();
            lbZoneList.Items.Add("Automatic");
            for (int x = 0; x < zones.Count; x++)
            {
                lbZoneList.Items.Add(String.Format("Zone {0}", x));
            }
        }
    }
}
