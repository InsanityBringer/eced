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
            this.tbCeilingTex = new System.Windows.Forms.TextBox();
            this.tbFloorTex = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSectorLight)).BeginInit();
            this.SuspendLayout();
            // 
            // nudSectorLight
            // 
            this.nudSectorLight.Location = new System.Drawing.Point(6, 103);
            this.nudSectorLight.Name = "nudSectorLight";
            this.nudSectorLight.Size = new System.Drawing.Size(99, 20);
            this.nudSectorLight.TabIndex = 3;
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
            // tbCeilingTex
            // 
            this.tbCeilingTex.Location = new System.Drawing.Point(5, 57);
            this.tbCeilingTex.Name = "tbCeilingTex";
            this.tbCeilingTex.Size = new System.Drawing.Size(100, 20);
            this.tbCeilingTex.TabIndex = 1;
            this.tbCeilingTex.Text = "#383838";
            // 
            // tbFloorTex
            // 
            this.tbFloorTex.Location = new System.Drawing.Point(5, 17);
            this.tbFloorTex.Name = "tbFloorTex";
            this.tbFloorTex.Size = new System.Drawing.Size(100, 20);
            this.tbFloorTex.TabIndex = 1;
            this.tbFloorTex.Text = "#717171";
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
            this.Controls.Add(this.tbCeilingTex);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.tbFloorTex);
            this.Name = "SectorUIPanel";
            this.Size = new System.Drawing.Size(118, 133);
            ((System.ComponentModel.ISupportInitialize)(this.nudSectorLight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudSectorLight;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbCeilingTex;
        private System.Windows.Forms.TextBox tbFloorTex;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
    }
}
