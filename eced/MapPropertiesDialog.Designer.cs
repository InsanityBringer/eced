namespace eced
{
    partial class MapPropertiesDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.MapNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TileSizeSpinner = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExperimentalCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DefaultLightSpinner = new System.Windows.Forms.NumericUpDown();
            this.VisibilitySpinner = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.CloseDialogButton = new System.Windows.Forms.Button();
            this.AcceptDialogButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TileSizeSpinner)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultLightSpinner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VisibilitySpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // MapNameTextBox
            // 
            this.MapNameTextBox.Location = new System.Drawing.Point(15, 25);
            this.MapNameTextBox.Name = "MapNameTextBox";
            this.MapNameTextBox.Size = new System.Drawing.Size(300, 20);
            this.MapNameTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tile Size:";
            // 
            // TileSizeSpinner
            // 
            this.TileSizeSpinner.Location = new System.Drawing.Point(71, 51);
            this.TileSizeSpinner.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.TileSizeSpinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TileSizeSpinner.Name = "TileSizeSpinner";
            this.TileSizeSpinner.Size = new System.Drawing.Size(55, 20);
            this.TileSizeSpinner.TabIndex = 3;
            this.TileSizeSpinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.VisibilitySpinner);
            this.groupBox1.Controls.Add(this.DefaultLightSpinner);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ExperimentalCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(18, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 98);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Experimental";
            // 
            // ExperimentalCheckBox
            // 
            this.ExperimentalCheckBox.AutoSize = true;
            this.ExperimentalCheckBox.Location = new System.Drawing.Point(6, 19);
            this.ExperimentalCheckBox.Name = "ExperimentalCheckBox";
            this.ExperimentalCheckBox.Size = new System.Drawing.Size(162, 17);
            this.ExperimentalCheckBox.TabIndex = 0;
            this.ExperimentalCheckBox.Text = "Enable experimental features";
            this.ExperimentalCheckBox.UseVisualStyleBackColor = true;
            this.ExperimentalCheckBox.CheckedChanged += new System.EventHandler(this.ExperimentalCheckBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Default tile brightness:";
            // 
            // DefaultLightSpinner
            // 
            this.DefaultLightSpinner.Location = new System.Drawing.Point(123, 42);
            this.DefaultLightSpinner.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.DefaultLightSpinner.Name = "DefaultLightSpinner";
            this.DefaultLightSpinner.Size = new System.Drawing.Size(69, 20);
            this.DefaultLightSpinner.TabIndex = 5;
            // 
            // VisibilitySpinner
            // 
            this.VisibilitySpinner.DecimalPlaces = 2;
            this.VisibilitySpinner.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.VisibilitySpinner.Location = new System.Drawing.Point(123, 68);
            this.VisibilitySpinner.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.VisibilitySpinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.VisibilitySpinner.Name = "VisibilitySpinner";
            this.VisibilitySpinner.Size = new System.Drawing.Size(69, 20);
            this.VisibilitySpinner.TabIndex = 5;
            this.VisibilitySpinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Default visibility:";
            // 
            // CloseDialogButton
            // 
            this.CloseDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseDialogButton.Location = new System.Drawing.Point(240, 182);
            this.CloseDialogButton.Name = "CloseDialogButton";
            this.CloseDialogButton.Size = new System.Drawing.Size(75, 23);
            this.CloseDialogButton.TabIndex = 5;
            this.CloseDialogButton.Text = "Cancel";
            this.CloseDialogButton.UseVisualStyleBackColor = true;
            // 
            // AcceptDialogButton
            // 
            this.AcceptDialogButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AcceptDialogButton.Location = new System.Drawing.Point(159, 182);
            this.AcceptDialogButton.Name = "AcceptDialogButton";
            this.AcceptDialogButton.Size = new System.Drawing.Size(75, 23);
            this.AcceptDialogButton.TabIndex = 6;
            this.AcceptDialogButton.Text = "OK";
            this.AcceptDialogButton.UseVisualStyleBackColor = true;
            // 
            // MapPropertiesDialog
            // 
            this.AcceptButton = this.AcceptDialogButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseDialogButton;
            this.ClientSize = new System.Drawing.Size(331, 217);
            this.ControlBox = false;
            this.Controls.Add(this.AcceptDialogButton);
            this.Controls.Add(this.CloseDialogButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TileSizeSpinner);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MapNameTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapPropertiesDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Global Map Properties";
            ((System.ComponentModel.ISupportInitialize)(this.TileSizeSpinner)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultLightSpinner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VisibilitySpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MapNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown TileSizeSpinner;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown VisibilitySpinner;
        private System.Windows.Forms.NumericUpDown DefaultLightSpinner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ExperimentalCheckBox;
        private System.Windows.Forms.Button CloseDialogButton;
        private System.Windows.Forms.Button AcceptDialogButton;
    }
}