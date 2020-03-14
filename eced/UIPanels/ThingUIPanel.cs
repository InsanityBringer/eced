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
    public partial class ThingUIPanel : UserControl
    {
        private List<ThingDefinition> palette;
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
                names[i] = things[i].name;
            }
            comboBox1.Items.AddRange(names);
        }
    }
}
