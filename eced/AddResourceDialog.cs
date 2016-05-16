using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eced.ResourceFiles;

namespace eced
{
    public partial class AddResourceDialog : Form
    {
        ResourceFiles.ResourceArchiveHeader resfile = new ResourceFiles.ResourceArchiveHeader();
        public AddResourceDialog()
        {
            InitializeComponent();
        }

        public ResourceArchiveHeader getResource()
        {
            return resfile;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ofdResource.Filter = ".WAD files|*.wad";

            DialogResult res = ofdResource.ShowDialog();

            if (res == DialogResult.OK)
            {
                if (ofdResource.FileName != "")
                {
                    txtWadPath.Text = ofdResource.FileName;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ofdResource.Filter = ".PK3/.ZIP files|*.PK3;*.ZIP;*.PKZ;*.PKW";

            DialogResult res = ofdResource.ShowDialog();

            if (res == DialogResult.OK)
            {
                if (ofdResource.FileName != "")
                {
                    txtPK3Path.Text = ofdResource.FileName;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            resfile.filename = txtWadPath.Text;
            resfile.format = ResourceFiles.ResourceFormat.FORMAT_WAD;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            resfile.filename = txtPK3Path.Text;
            resfile.format = ResourceFiles.ResourceFormat.FORMAT_ZIP;        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (resfile.filename == "")
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }
    }
}
