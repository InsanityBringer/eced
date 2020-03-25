using System;
using System.Windows.Forms;

using eced.Brushes;

namespace eced.UIPanels
{
    public partial class TagUIPanel : UserControl
    {
        private TagTool pairedBrush;
        public TagUIPanel()
        {
            InitializeComponent();
        }

        public void SetPairedBrush(TagTool brush)
        {
            pairedBrush = brush;
        }

        private void TagSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (pairedBrush != null)
            {
                pairedBrush.tag = (int)TagSpinner.Value;
            }
        }
    }
}
