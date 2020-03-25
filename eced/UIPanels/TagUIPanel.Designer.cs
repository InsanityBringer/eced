namespace eced.UIPanels
{
    partial class TagUIPanel
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
            this.TagSpinner = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TagSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // TagSpinner
            // 
            this.TagSpinner.Location = new System.Drawing.Point(6, 16);
            this.TagSpinner.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.TagSpinner.Name = "TagSpinner";
            this.TagSpinner.Size = new System.Drawing.Size(99, 20);
            this.TagSpinner.TabIndex = 1;
            this.TagSpinner.ValueChanged += new System.EventHandler(this.TagSpinner_ValueChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(51, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "New Tag";
            // 
            // TagUIPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TagSpinner);
            this.Controls.Add(this.label24);
            this.Name = "TagUIPanel";
            this.Size = new System.Drawing.Size(113, 55);
            ((System.ComponentModel.ISupportInitialize)(this.TagSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown TagSpinner;
        private System.Windows.Forms.Label label24;
    }
}
