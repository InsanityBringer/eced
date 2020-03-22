namespace eced.UIPanels
{
    partial class SectorUIPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nudSectorLight = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.CeilTexTextBox = new System.Windows.Forms.TextBox();
            this.FloorTexTextBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSectorLight)).BeginInit();
            this.SuspendLayout();
            // 
            // nudSectorLight
            // 
            this.nudSectorLight.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudSectorLight.Location = new System.Drawing.Point(6, 103);
            this.nudSectorLight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudSectorLight.Name = "nudSectorLight";
            this.nudSectorLight.Size = new System.Drawing.Size(99, 20);
            this.nudSectorLight.TabIndex = 3;
            this.nudSectorLight.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudSectorLight.ValueChanged += new System.EventHandler(this.nudSectorLight_ValueChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(2, 84);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 13);
            this.label23.TabIndex = 2;
            this.label23.Text = "Light Level";
            // 
            // CeilTexTextBox
            // 
            this.CeilTexTextBox.Location = new System.Drawing.Point(5, 57);
            this.CeilTexTextBox.Name = "CeilTexTextBox";
            this.CeilTexTextBox.Size = new System.Drawing.Size(100, 20);
            this.CeilTexTextBox.TabIndex = 1;
            this.CeilTexTextBox.Text = "#383838";
            this.CeilTexTextBox.TextChanged += new System.EventHandler(this.FloorTexTextBox_TextChanged);
            // 
            // FloorTexTextBox
            // 
            this.FloorTexTextBox.Location = new System.Drawing.Point(5, 17);
            this.FloorTexTextBox.Name = "FloorTexTextBox";
            this.FloorTexTextBox.Size = new System.Drawing.Size(100, 20);
            this.FloorTexTextBox.TabIndex = 1;
            this.FloorTexTextBox.Text = "#717171";
            this.FloorTexTextBox.TextChanged += new System.EventHandler(this.FloorTexTextBox_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 40);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 13);
            this.label22.TabIndex = 0;
            this.label22.Text = "Ceiling Texture";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(3, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(69, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Floor Texture";
            // 
            // SectorUIPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nudSectorLight);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.CeilTexTextBox);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.FloorTexTextBox);
            this.Name = "SectorUIPanel";
            this.Size = new System.Drawing.Size(118, 133);
            ((System.ComponentModel.ISupportInitialize)(this.nudSectorLight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudSectorLight;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox CeilTexTextBox;
        private System.Windows.Forms.TextBox FloorTexTextBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
    }
}
