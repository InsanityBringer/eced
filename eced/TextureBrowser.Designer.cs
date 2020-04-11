namespace eced
{
    partial class TextureBrowser
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(647, 466);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // SelectButton
            // 
            this.SelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SelectButton.Location = new System.Drawing.Point(566, 466);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(75, 23);
            this.SelectButton.TabIndex = 1;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 471);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of textures:";
            // 
            // SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything
            // 
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Location = new System.Drawing.Point(0, 0);
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Name = "SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrA" +
    "nything";
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Size = new System.Drawing.Size(734, 460);
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.TabIndex = 3;
            this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything.Visible = false;
            // 
            // TextureBrowser
            // 
            this.AcceptButton = this.SelectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(734, 501);
            this.Controls.Add(this.SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.CancelButton);
            this.Name = "TextureBrowser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Browsing Textures";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel SizeReferencePanelBecauseTheWindowsFormDesignerIsABeautifulThingThatIsntBrokenOrAnything;
    }
}