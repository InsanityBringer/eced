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
        public ArchiveHeader CurrentArchive { get; } = new ResourceFiles.ArchiveHeader();
        public AddResourceDialog()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArchiveOpenFileDialog.Filter = ".WAD files|*.wad";

            DialogResult res = ArchiveOpenFileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                if (ArchiveOpenFileDialog.FileName != "")
                {
                    txtWadPath.Text = ArchiveOpenFileDialog.FileName;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ArchiveOpenFileDialog.Filter = ".PK3/.ZIP files|*.PK3;*.ZIP;*.PKZ;*.PKW";

            DialogResult res = ArchiveOpenFileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                if (ArchiveOpenFileDialog.FileName != "")
                {
                    txtPK3Path.Text = ArchiveOpenFileDialog.FileName;
                }
            }
        }

        //what a horrible hack
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CurrentArchive.filename = txtWadPath.Text;
            CurrentArchive.format = ResourceFiles.ResourceFormat.Wad;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CurrentArchive.filename = txtPK3Path.Text;
            CurrentArchive.format = ResourceFiles.ResourceFormat.Zip;        
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ArchiveOpenFileDialog.Filter = "VSWAP Files|VSWAP.WL6;VSWAP.SOD";

            DialogResult res = ArchiveOpenFileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                if (ArchiveOpenFileDialog.FileName != "")
                {
                    VSwapPathTextBox.Text = ArchiveOpenFileDialog.FileName;
                }
            }
        }

        private void VSwapPathTextBox_TextChanged(object sender, EventArgs e)
        {
            CurrentArchive.filename = VSwapPathTextBox.Text;
            CurrentArchive.format = ResourceFormat.VSwap;
        }
    }
}
