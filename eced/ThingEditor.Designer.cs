namespace eced
{
    partial class ThingEditor
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
            this.rbThingNW = new System.Windows.Forms.RadioButton();
            this.rbThingNE = new System.Windows.Forms.RadioButton();
            this.rbThingSW = new System.Windows.Forms.RadioButton();
            this.rbThingSE = new System.Windows.Forms.RadioButton();
            this.rbThingSouth = new System.Windows.Forms.RadioButton();
            this.rbThingNorth = new System.Windows.Forms.RadioButton();
            this.rbThingWest = new System.Windows.Forms.RadioButton();
            this.rbThingEast = new System.Windows.Forms.RadioButton();
            this.cbThingSkill4 = new System.Windows.Forms.CheckBox();
            this.cbThingSkill3 = new System.Windows.Forms.CheckBox();
            this.cbThingSkill2 = new System.Windows.Forms.CheckBox();
            this.cbThingSkill1 = new System.Windows.Forms.CheckBox();
            this.cbThingPatrol = new System.Windows.Forms.CheckBox();
            this.cbThingAmbush = new System.Windows.Forms.CheckBox();
            this.ndThingAngle = new System.Windows.Forms.NumericUpDown();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ndThingAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // rbThingNW
            // 
            this.rbThingNW.AutoSize = true;
            this.rbThingNW.Location = new System.Drawing.Point(12, 295);
            this.rbThingNW.Name = "rbThingNW";
            this.rbThingNW.Size = new System.Drawing.Size(50, 17);
            this.rbThingNW.TabIndex = 44;
            this.rbThingNW.TabStop = true;
            this.rbThingNW.Text = "N.W.";
            this.rbThingNW.UseVisualStyleBackColor = true;
            this.rbThingNW.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingNE
            // 
            this.rbThingNE.AutoSize = true;
            this.rbThingNE.Location = new System.Drawing.Point(122, 295);
            this.rbThingNE.Name = "rbThingNE";
            this.rbThingNE.Size = new System.Drawing.Size(49, 17);
            this.rbThingNE.TabIndex = 43;
            this.rbThingNE.TabStop = true;
            this.rbThingNE.Text = "N. E.";
            this.rbThingNE.UseVisualStyleBackColor = true;
            this.rbThingNE.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingSW
            // 
            this.rbThingSW.AutoSize = true;
            this.rbThingSW.Location = new System.Drawing.Point(12, 343);
            this.rbThingSW.Name = "rbThingSW";
            this.rbThingSW.Size = new System.Drawing.Size(49, 17);
            this.rbThingSW.TabIndex = 42;
            this.rbThingSW.TabStop = true;
            this.rbThingSW.Text = "S.W.";
            this.rbThingSW.UseVisualStyleBackColor = true;
            this.rbThingSW.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingSE
            // 
            this.rbThingSE.AutoSize = true;
            this.rbThingSE.Location = new System.Drawing.Point(122, 344);
            this.rbThingSE.Name = "rbThingSE";
            this.rbThingSE.Size = new System.Drawing.Size(48, 17);
            this.rbThingSE.TabIndex = 41;
            this.rbThingSE.TabStop = true;
            this.rbThingSE.Text = "S. E.";
            this.rbThingSE.UseVisualStyleBackColor = true;
            this.rbThingSE.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingSouth
            // 
            this.rbThingSouth.AutoSize = true;
            this.rbThingSouth.Location = new System.Drawing.Point(63, 344);
            this.rbThingSouth.Name = "rbThingSouth";
            this.rbThingSouth.Size = new System.Drawing.Size(53, 17);
            this.rbThingSouth.TabIndex = 40;
            this.rbThingSouth.TabStop = true;
            this.rbThingSouth.Text = "South";
            this.rbThingSouth.UseVisualStyleBackColor = true;
            this.rbThingSouth.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingNorth
            // 
            this.rbThingNorth.AutoSize = true;
            this.rbThingNorth.Location = new System.Drawing.Point(63, 295);
            this.rbThingNorth.Name = "rbThingNorth";
            this.rbThingNorth.Size = new System.Drawing.Size(51, 17);
            this.rbThingNorth.TabIndex = 39;
            this.rbThingNorth.TabStop = true;
            this.rbThingNorth.Text = "North";
            this.rbThingNorth.UseVisualStyleBackColor = true;
            this.rbThingNorth.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingWest
            // 
            this.rbThingWest.AutoSize = true;
            this.rbThingWest.Location = new System.Drawing.Point(12, 318);
            this.rbThingWest.Name = "rbThingWest";
            this.rbThingWest.Size = new System.Drawing.Size(50, 17);
            this.rbThingWest.TabIndex = 38;
            this.rbThingWest.TabStop = true;
            this.rbThingWest.Text = "West";
            this.rbThingWest.UseVisualStyleBackColor = true;
            this.rbThingWest.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // rbThingEast
            // 
            this.rbThingEast.AutoSize = true;
            this.rbThingEast.Checked = true;
            this.rbThingEast.Location = new System.Drawing.Point(122, 318);
            this.rbThingEast.Name = "rbThingEast";
            this.rbThingEast.Size = new System.Drawing.Size(46, 17);
            this.rbThingEast.TabIndex = 30;
            this.rbThingEast.TabStop = true;
            this.rbThingEast.Text = "East";
            this.rbThingEast.UseVisualStyleBackColor = true;
            this.rbThingEast.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // cbThingSkill4
            // 
            this.cbThingSkill4.AutoSize = true;
            this.cbThingSkill4.Checked = true;
            this.cbThingSkill4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill4.Location = new System.Drawing.Point(165, 138);
            this.cbThingSkill4.Name = "cbThingSkill4";
            this.cbThingSkill4.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill4.TabIndex = 37;
            this.cbThingSkill4.Text = "Skill 4";
            this.cbThingSkill4.UseVisualStyleBackColor = true;
            this.cbThingSkill4.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // cbThingSkill3
            // 
            this.cbThingSkill3.AutoSize = true;
            this.cbThingSkill3.Checked = true;
            this.cbThingSkill3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill3.Location = new System.Drawing.Point(165, 116);
            this.cbThingSkill3.Name = "cbThingSkill3";
            this.cbThingSkill3.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill3.TabIndex = 36;
            this.cbThingSkill3.Text = "Skill 3";
            this.cbThingSkill3.UseVisualStyleBackColor = true;
            this.cbThingSkill3.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // cbThingSkill2
            // 
            this.cbThingSkill2.AutoSize = true;
            this.cbThingSkill2.Checked = true;
            this.cbThingSkill2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill2.Location = new System.Drawing.Point(165, 93);
            this.cbThingSkill2.Name = "cbThingSkill2";
            this.cbThingSkill2.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill2.TabIndex = 35;
            this.cbThingSkill2.Text = "Skill 2";
            this.cbThingSkill2.UseVisualStyleBackColor = true;
            this.cbThingSkill2.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // cbThingSkill1
            // 
            this.cbThingSkill1.AutoSize = true;
            this.cbThingSkill1.Checked = true;
            this.cbThingSkill1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill1.Location = new System.Drawing.Point(165, 70);
            this.cbThingSkill1.Name = "cbThingSkill1";
            this.cbThingSkill1.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill1.TabIndex = 34;
            this.cbThingSkill1.Text = "Skill 1";
            this.cbThingSkill1.UseVisualStyleBackColor = true;
            this.cbThingSkill1.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // cbThingPatrol
            // 
            this.cbThingPatrol.AutoSize = true;
            this.cbThingPatrol.Location = new System.Drawing.Point(165, 48);
            this.cbThingPatrol.Name = "cbThingPatrol";
            this.cbThingPatrol.Size = new System.Drawing.Size(53, 17);
            this.cbThingPatrol.TabIndex = 33;
            this.cbThingPatrol.Text = "Patrol";
            this.cbThingPatrol.UseVisualStyleBackColor = true;
            this.cbThingPatrol.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // cbThingAmbush
            // 
            this.cbThingAmbush.AutoSize = true;
            this.cbThingAmbush.Location = new System.Drawing.Point(165, 25);
            this.cbThingAmbush.Name = "cbThingAmbush";
            this.cbThingAmbush.Size = new System.Drawing.Size(64, 17);
            this.cbThingAmbush.TabIndex = 31;
            this.cbThingAmbush.Text = "Ambush";
            this.cbThingAmbush.UseVisualStyleBackColor = true;
            this.cbThingAmbush.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // ndThingAngle
            // 
            this.ndThingAngle.Location = new System.Drawing.Point(68, 318);
            this.ndThingAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.ndThingAngle.Name = "ndThingAngle";
            this.ndThingAngle.Size = new System.Drawing.Size(39, 20);
            this.ndThingAngle.TabIndex = 32;
            this.ndThingAngle.ValueChanged += new System.EventHandler(this.ndThingAngle_ValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(147, 264);
            this.listBox1.TabIndex = 29;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Flags";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(315, 347);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 47;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(234, 347);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 48;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click);
            // 
            // ThingEditor
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(402, 382);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbThingNW);
            this.Controls.Add(this.rbThingNE);
            this.Controls.Add(this.rbThingSW);
            this.Controls.Add(this.rbThingSE);
            this.Controls.Add(this.rbThingSouth);
            this.Controls.Add(this.rbThingNorth);
            this.Controls.Add(this.rbThingWest);
            this.Controls.Add(this.rbThingEast);
            this.Controls.Add(this.cbThingSkill4);
            this.Controls.Add(this.cbThingSkill3);
            this.Controls.Add(this.cbThingSkill2);
            this.Controls.Add(this.cbThingSkill1);
            this.Controls.Add(this.cbThingPatrol);
            this.Controls.Add(this.cbThingAmbush);
            this.Controls.Add(this.ndThingAngle);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThingEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editing Thing";
            ((System.ComponentModel.ISupportInitialize)(this.ndThingAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbThingNW;
        private System.Windows.Forms.RadioButton rbThingNE;
        private System.Windows.Forms.RadioButton rbThingSW;
        private System.Windows.Forms.RadioButton rbThingSE;
        private System.Windows.Forms.RadioButton rbThingSouth;
        private System.Windows.Forms.RadioButton rbThingNorth;
        private System.Windows.Forms.RadioButton rbThingWest;
        private System.Windows.Forms.RadioButton rbThingEast;
        private System.Windows.Forms.CheckBox cbThingSkill4;
        private System.Windows.Forms.CheckBox cbThingSkill3;
        private System.Windows.Forms.CheckBox cbThingSkill2;
        private System.Windows.Forms.CheckBox cbThingSkill1;
        private System.Windows.Forms.CheckBox cbThingPatrol;
        private System.Windows.Forms.CheckBox cbThingAmbush;
        private System.Windows.Forms.NumericUpDown ndThingAngle;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}