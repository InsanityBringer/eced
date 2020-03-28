namespace eced.UIPanels
{
    partial class ZoneUIPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.ZoneListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.HighlightedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zone list:";
            // 
            // ZoneListView
            // 
            this.ZoneListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ZoneListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ZoneListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ZoneListView.HideSelection = false;
            this.ZoneListView.Location = new System.Drawing.Point(3, 50);
            this.ZoneListView.MultiSelect = false;
            this.ZoneListView.Name = "ZoneListView";
            this.ZoneListView.Size = new System.Drawing.Size(157, 259);
            this.ZoneListView.TabIndex = 2;
            this.ZoneListView.UseCompatibleStateImageBehavior = false;
            this.ZoneListView.View = System.Windows.Forms.View.Details;
            this.ZoneListView.SelectedIndexChanged += new System.EventHandler(this.ZoneListBox_SelectedIndexChanged);
            this.ZoneListView.Resize += new System.EventHandler(this.ZoneListView_Resize);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 118;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Currently highlighted";
            // 
            // HighlightedLabel
            // 
            this.HighlightedLabel.Location = new System.Drawing.Point(3, 17);
            this.HighlightedLabel.Name = "HighlightedLabel";
            this.HighlightedLabel.Size = new System.Drawing.Size(157, 17);
            this.HighlightedLabel.TabIndex = 4;
            this.HighlightedLabel.Text = "label3";
            // 
            // ZoneUIPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HighlightedLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ZoneListView);
            this.Controls.Add(this.label1);
            this.Name = "ZoneUIPanel";
            this.Size = new System.Drawing.Size(163, 312);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView ZoneListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label HighlightedLabel;
    }
}
