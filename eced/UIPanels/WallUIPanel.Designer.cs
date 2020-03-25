namespace eced.UIPanels
{
    partial class WallUIPanel
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.TilePaletteComboBox = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.SndSeqTextBox = new System.Windows.Forms.TextBox();
            this.WestBlockCheckBox = new System.Windows.Forms.CheckBox();
            this.SouthBlockCheckBox = new System.Windows.Forms.CheckBox();
            this.EastBlockCheckBox = new System.Windows.Forms.CheckBox();
            this.InsetVerticalCheckBox = new System.Windows.Forms.CheckBox();
            this.InsetHorizontalCheckBox = new System.Windows.Forms.CheckBox();
            this.cbDontMap = new System.Windows.Forms.CheckBox();
            this.NorthBlockCheckBox = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.MapTexTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.WestTexTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.EastTexTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SouthTexTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.NorthTexTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 23);
            this.button2.TabIndex = 29;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(57, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 28;
            this.button1.Text = "Manage...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TilePaletteComboBox
            // 
            this.TilePaletteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TilePaletteComboBox.FormattingEnabled = true;
            this.TilePaletteComboBox.Location = new System.Drawing.Point(6, 16);
            this.TilePaletteComboBox.Name = "TilePaletteComboBox";
            this.TilePaletteComboBox.Size = new System.Drawing.Size(126, 21);
            this.TilePaletteComboBox.TabIndex = 27;
            this.TilePaletteComboBox.SelectedIndexChanged += new System.EventHandler(this.TilePaletteComboBox_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(6, 413);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Palette";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 315);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(32, 13);
            this.label19.TabIndex = 24;
            this.label19.Text = "Flags";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 397);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(78, 13);
            this.label20.TabIndex = 23;
            this.label20.Text = "Automap Level";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(3, 436);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(150, 13);
            this.label18.TabIndex = 23;
            this.label18.Text = "Event Sound Sequence name";
            // 
            // SndSeqTextBox
            // 
            this.SndSeqTextBox.Location = new System.Drawing.Point(6, 452);
            this.SndSeqTextBox.Name = "SndSeqTextBox";
            this.SndSeqTextBox.Size = new System.Drawing.Size(134, 20);
            this.SndSeqTextBox.TabIndex = 22;
            this.SndSeqTextBox.TextChanged += new System.EventHandler(this.SndSeqTextBox_TextChanged);
            // 
            // WestBlockCheckBox
            // 
            this.WestBlockCheckBox.AutoSize = true;
            this.WestBlockCheckBox.Location = new System.Drawing.Point(103, 295);
            this.WestBlockCheckBox.Name = "WestBlockCheckBox";
            this.WestBlockCheckBox.Size = new System.Drawing.Size(37, 17);
            this.WestBlockCheckBox.TabIndex = 21;
            this.WestBlockCheckBox.Text = "W";
            this.WestBlockCheckBox.UseVisualStyleBackColor = true;
            this.WestBlockCheckBox.CheckedChanged += new System.EventHandler(this.TileBlocking_CheckedChanged);
            // 
            // SouthBlockCheckBox
            // 
            this.SouthBlockCheckBox.AutoSize = true;
            this.SouthBlockCheckBox.Location = new System.Drawing.Point(72, 295);
            this.SouthBlockCheckBox.Name = "SouthBlockCheckBox";
            this.SouthBlockCheckBox.Size = new System.Drawing.Size(33, 17);
            this.SouthBlockCheckBox.TabIndex = 21;
            this.SouthBlockCheckBox.Text = "S";
            this.SouthBlockCheckBox.UseVisualStyleBackColor = true;
            this.SouthBlockCheckBox.CheckedChanged += new System.EventHandler(this.TileBlocking_CheckedChanged);
            // 
            // EastBlockCheckBox
            // 
            this.EastBlockCheckBox.AutoSize = true;
            this.EastBlockCheckBox.Location = new System.Drawing.Point(39, 295);
            this.EastBlockCheckBox.Name = "EastBlockCheckBox";
            this.EastBlockCheckBox.Size = new System.Drawing.Size(33, 17);
            this.EastBlockCheckBox.TabIndex = 21;
            this.EastBlockCheckBox.Text = "E";
            this.EastBlockCheckBox.UseVisualStyleBackColor = true;
            this.EastBlockCheckBox.CheckedChanged += new System.EventHandler(this.TileBlocking_CheckedChanged);
            // 
            // InsetVerticalCheckBox
            // 
            this.InsetVerticalCheckBox.AutoSize = true;
            this.InsetVerticalCheckBox.Location = new System.Drawing.Point(6, 377);
            this.InsetVerticalCheckBox.Name = "InsetVerticalCheckBox";
            this.InsetVerticalCheckBox.Size = new System.Drawing.Size(103, 17);
            this.InsetVerticalCheckBox.TabIndex = 21;
            this.InsetVerticalCheckBox.Text = "Inset East/West";
            this.InsetVerticalCheckBox.UseVisualStyleBackColor = true;
            this.InsetVerticalCheckBox.CheckedChanged += new System.EventHandler(this.InsetHorizontalCheckBox_CheckedChanged);
            // 
            // InsetHorizontalCheckBox
            // 
            this.InsetHorizontalCheckBox.AutoSize = true;
            this.InsetHorizontalCheckBox.Location = new System.Drawing.Point(6, 354);
            this.InsetHorizontalCheckBox.Name = "InsetHorizontalCheckBox";
            this.InsetHorizontalCheckBox.Size = new System.Drawing.Size(111, 17);
            this.InsetHorizontalCheckBox.TabIndex = 21;
            this.InsetHorizontalCheckBox.Text = "Inset North/South";
            this.InsetHorizontalCheckBox.UseVisualStyleBackColor = true;
            this.InsetHorizontalCheckBox.CheckedChanged += new System.EventHandler(this.InsetHorizontalCheckBox_CheckedChanged);
            // 
            // cbDontMap
            // 
            this.cbDontMap.AutoSize = true;
            this.cbDontMap.Location = new System.Drawing.Point(6, 331);
            this.cbDontMap.Name = "cbDontMap";
            this.cbDontMap.Size = new System.Drawing.Size(126, 17);
            this.cbDontMap.TabIndex = 21;
            this.cbDontMap.Text = "Don\'t map on overlay";
            this.cbDontMap.UseVisualStyleBackColor = true;
            // 
            // NorthBlockCheckBox
            // 
            this.NorthBlockCheckBox.AutoSize = true;
            this.NorthBlockCheckBox.Location = new System.Drawing.Point(6, 295);
            this.NorthBlockCheckBox.Name = "NorthBlockCheckBox";
            this.NorthBlockCheckBox.Size = new System.Drawing.Size(34, 17);
            this.NorthBlockCheckBox.TabIndex = 21;
            this.NorthBlockCheckBox.Text = "N";
            this.NorthBlockCheckBox.UseVisualStyleBackColor = true;
            this.NorthBlockCheckBox.CheckedChanged += new System.EventHandler(this.TileBlocking_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 279);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 13);
            this.label16.TabIndex = 20;
            this.label16.Text = "Block:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 240);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Map Texture";
            // 
            // MapTexTextBox
            // 
            this.MapTexTextBox.Location = new System.Drawing.Point(6, 256);
            this.MapTexTextBox.Name = "MapTexTextBox";
            this.MapTexTextBox.Size = new System.Drawing.Size(97, 20);
            this.MapTexTextBox.TabIndex = 18;
            this.MapTexTextBox.TextChanged += new System.EventHandler(this.TileTexture_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 201);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "West Texture";
            // 
            // WestTexTextBox
            // 
            this.WestTexTextBox.Location = new System.Drawing.Point(6, 217);
            this.WestTexTextBox.Name = "WestTexTextBox";
            this.WestTexTextBox.Size = new System.Drawing.Size(97, 20);
            this.WestTexTextBox.TabIndex = 18;
            this.WestTexTextBox.TextChanged += new System.EventHandler(this.TileTexture_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 161);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "East Texture";
            // 
            // EastTexTextBox
            // 
            this.EastTexTextBox.Location = new System.Drawing.Point(6, 177);
            this.EastTexTextBox.Name = "EastTexTextBox";
            this.EastTexTextBox.Size = new System.Drawing.Size(97, 20);
            this.EastTexTextBox.TabIndex = 16;
            this.EastTexTextBox.TextChanged += new System.EventHandler(this.TileTexture_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 121);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "South Texture";
            // 
            // SouthTexTextBox
            // 
            this.SouthTexTextBox.Location = new System.Drawing.Point(6, 137);
            this.SouthTexTextBox.Name = "SouthTexTextBox";
            this.SouthTexTextBox.Size = new System.Drawing.Size(97, 20);
            this.SouthTexTextBox.TabIndex = 14;
            this.SouthTexTextBox.TextChanged += new System.EventHandler(this.TileTexture_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 81);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "North Texture";
            // 
            // NorthTexTextBox
            // 
            this.NorthTexTextBox.Location = new System.Drawing.Point(6, 97);
            this.NorthTexTextBox.Name = "NorthTexTextBox";
            this.NorthTexTextBox.Size = new System.Drawing.Size(97, 20);
            this.NorthTexTextBox.TabIndex = 12;
            this.NorthTexTextBox.TextChanged += new System.EventHandler(this.TileTexture_TextChanged);
            // 
            // WallUIPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TilePaletteComboBox);
            this.Controls.Add(this.NorthTexTextBox);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.SouthTexTextBox);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.EastTexTextBox);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.SndSeqTextBox);
            this.Controls.Add(this.WestTexTextBox);
            this.Controls.Add(this.WestBlockCheckBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.SouthBlockCheckBox);
            this.Controls.Add(this.MapTexTextBox);
            this.Controls.Add(this.EastBlockCheckBox);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.InsetVerticalCheckBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.InsetHorizontalCheckBox);
            this.Controls.Add(this.NorthBlockCheckBox);
            this.Controls.Add(this.cbDontMap);
            this.Name = "WallUIPanel";
            this.Size = new System.Drawing.Size(170, 500);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox SndSeqTextBox;
        private System.Windows.Forms.CheckBox WestBlockCheckBox;
        private System.Windows.Forms.CheckBox SouthBlockCheckBox;
        private System.Windows.Forms.CheckBox EastBlockCheckBox;
        private System.Windows.Forms.CheckBox InsetVerticalCheckBox;
        private System.Windows.Forms.CheckBox InsetHorizontalCheckBox;
        private System.Windows.Forms.CheckBox cbDontMap;
        private System.Windows.Forms.CheckBox NorthBlockCheckBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox MapTexTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox WestTexTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox EastTexTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox SouthTexTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox NorthTexTextBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox TilePaletteComboBox;
    }
}
