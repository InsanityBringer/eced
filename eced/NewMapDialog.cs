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
    public partial class NewMapDialog : Form
    {
        //private MapInformation currentMapInfo = new MapInformation();
        public MapInformation CurrentMap { get; } = new MapInformation();
        public NewMapDialog()
        {
            InitializeComponent();
        }

        private void txtMapName_TextChanged(object sender, EventArgs e)
        {
            CurrentMap.lumpname = ((TextBox)sender).Text;
        }

        private void nudXSize_ValueChanged(object sender, EventArgs e)
        {
            CurrentMap.sizex = (int)((NumericUpDown)sender).Value;
        }

        private void nudYSize_ValueChanged(object sender, EventArgs e)
        {
            CurrentMap.sizey = (int)((NumericUpDown)sender).Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                CurrentMap.files.Add((ResourceFiles.ResourceArchiveHeader)listBox1.Items[i]);
            }
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void nudLayerCount_ValueChanged(object sender, EventArgs e)
        {
            CurrentMap.layers = (int)((NumericUpDown)sender).Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddResourceDialog resourceDialog = new AddResourceDialog();
            if (resourceDialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(resourceDialog.getResource());
            }

            resourceDialog.Dispose();
        }
    }

    public class MapInformation
    {
        public int sizex = 64;
        public int sizey = 64;
        public int layers = 1;
        public string lumpname = "MAP01";

        public List<ResourceFiles.ResourceArchiveHeader> files = new List<ResourceFiles.ResourceArchiveHeader>();

        /// <summary>
        /// Finds if the specified resource name exists in this map's resources
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsResource(string name)
        {
            foreach (ResourceFiles.ResourceArchiveHeader file in files)
            {
                if (file.filename.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
