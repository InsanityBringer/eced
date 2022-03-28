namespace eced
{
    partial class AddResourceDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.txtWadPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button6 = new System.Windows.Forms.Button();
            this.txtPK3Path = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.VSwapPathTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ArchiveOKButton = new System.Windows.Forms.Button();
            this.ArchiveCancelButton = new System.Windows.Forms.Button();
            this.ArchiveOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(284, 85);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.txtWadPath);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(276, 59);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "WAD";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(231, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 20);
            this.button3.TabIndex = 2;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtWadPath
            // 
            this.txtWadPath.Location = new System.Drawing.Point(8, 20);
            this.txtWadPath.Name = "txtWadPath";
            this.txtWadPath.Size = new System.Drawing.Size(217, 20);
            this.txtWadPath.TabIndex = 1;
            this.txtWadPath.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WAD Path";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.txtPK3Path);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(276, 59);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ZIP/PK3";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(229, 19);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(36, 20);
            this.button6.TabIndex = 7;
            this.button6.Text = "...";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txtPK3Path
            // 
            this.txtPK3Path.Location = new System.Drawing.Point(6, 19);
            this.txtPK3Path.Name = "txtPK3Path";
            this.txtPK3Path.Size = new System.Drawing.Size(217, 20);
            this.txtPK3Path.TabIndex = 6;
            this.txtPK3Path.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "ZIP/PK3 Path";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button7);
            this.tabPage3.Controls.Add(this.VSwapPathTextBox);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(276, 59);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "VSWAP";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(231, 20);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(36, 20);
            this.button7.TabIndex = 5;
            this.button7.Text = "...";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // VSwapPathTextBox
            // 
            this.VSwapPathTextBox.Location = new System.Drawing.Point(8, 20);
            this.VSwapPathTextBox.Name = "VSwapPathTextBox";
            this.VSwapPathTextBox.Size = new System.Drawing.Size(217, 20);
            this.VSwapPathTextBox.TabIndex = 4;
            this.VSwapPathTextBox.TextChanged += new System.EventHandler(this.VSwapPathTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "VSwap Path";
            // 
            // ArchiveOKButton
            // 
            this.ArchiveOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ArchiveOKButton.Location = new System.Drawing.Point(116, 91);
            this.ArchiveOKButton.Name = "ArchiveOKButton";
            this.ArchiveOKButton.Size = new System.Drawing.Size(75, 23);
            this.ArchiveOKButton.TabIndex = 2;
            this.ArchiveOKButton.Text = "OK";
            this.ArchiveOKButton.UseVisualStyleBackColor = true;
            // 
            // ArchiveCancelButton
            // 
            this.ArchiveCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ArchiveCancelButton.Location = new System.Drawing.Point(197, 91);
            this.ArchiveCancelButton.Name = "ArchiveCancelButton";
            this.ArchiveCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ArchiveCancelButton.TabIndex = 1;
            this.ArchiveCancelButton.Text = "Cancel";
            this.ArchiveCancelButton.UseVisualStyleBackColor = true;
            // 
            // ArchiveOpenFileDialog
            // 
            this.ArchiveOpenFileDialog.FileName = "openFileDialog1";
            // 
            // AddResourceDialog
            // 
            this.AcceptButton = this.ArchiveOKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ArchiveCancelButton;
            this.ClientSize = new System.Drawing.Size(284, 126);
            this.Controls.Add(this.ArchiveOKButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ArchiveCancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddResourceDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddResourceDialog";
            this.Load += new System.EventHandler(this.AddResourceDialog_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button ArchiveCancelButton;
        private System.Windows.Forms.Button ArchiveOKButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtWadPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtPK3Path;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog ArchiveOpenFileDialog;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox VSwapPathTextBox;
        private System.Windows.Forms.Label label3;
    }
}