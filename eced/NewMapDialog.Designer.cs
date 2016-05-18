namespace eced
{
    partial class NewMapDialog
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
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudXSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudYSize = new System.Windows.Forms.NumericUpDown();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nudLayerCount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudXSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayerCount)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(80, 6);
            this.txtMapName.MaxLength = 8;
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(62, 20);
            this.txtMapName.TabIndex = 0;
            this.txtMapName.Text = "MAP01";
            this.txtMapName.TextChanged += new System.EventHandler(this.txtMapName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Map Name:";
            // 
            // nudXSize
            // 
            this.nudXSize.Location = new System.Drawing.Point(151, 25);
            this.nudXSize.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nudXSize.Name = "nudXSize";
            this.nudXSize.Size = new System.Drawing.Size(62, 20);
            this.nudXSize.TabIndex = 2;
            this.nudXSize.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nudXSize.ValueChanged += new System.EventHandler(this.nudXSize_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Dimensions:";
            // 
            // nudYSize
            // 
            this.nudYSize.Location = new System.Drawing.Point(219, 25);
            this.nudYSize.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nudYSize.Name = "nudYSize";
            this.nudYSize.Size = new System.Drawing.Size(62, 20);
            this.nudYSize.TabIndex = 4;
            this.nudYSize.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nudYSize.ValueChanged += new System.EventHandler(this.nudYSize_ValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 106);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(266, 147);
            this.listBox1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Resource List";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(96, 259);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Remove";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(177, 259);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Up";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(228, 259);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(53, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "Down";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(212, 305);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 11;
            this.button5.Text = "Cancel";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(131, 305);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 12;
            this.button6.Text = "OK";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Stating Layers:";
            // 
            // nudLayerCount
            // 
            this.nudLayerCount.Location = new System.Drawing.Point(95, 56);
            this.nudLayerCount.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nudLayerCount.Name = "nudLayerCount";
            this.nudLayerCount.Size = new System.Drawing.Size(62, 20);
            this.nudLayerCount.TabIndex = 13;
            this.nudLayerCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLayerCount.ValueChanged += new System.EventHandler(this.nudLayerCount_ValueChanged);
            // 
            // NewMapDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 341);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudLayerCount);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.nudYSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudXSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMapName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewMapDialog";
            this.ShowInTaskbar = false;
            this.Text = "Create New Map";
            ((System.ComponentModel.ISupportInitialize)(this.nudXSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayerCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudXSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudYSize;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudLayerCount;
    }
}