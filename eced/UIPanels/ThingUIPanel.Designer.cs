namespace eced.UIPanels
{
    partial class ThingUIPanel
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
            this.label10 = new System.Windows.Forms.Label();
            this.ThingAngleSpinner = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.ThingTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ThingAngleSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // rbThingNW
            // 
            this.rbThingNW.AutoSize = true;
            this.rbThingNW.Location = new System.Drawing.Point(6, 56);
            this.rbThingNW.Name = "rbThingNW";
            this.rbThingNW.Size = new System.Drawing.Size(50, 17);
            this.rbThingNW.TabIndex = 28;
            this.rbThingNW.TabStop = true;
            this.rbThingNW.Tag = "135";
            this.rbThingNW.Text = "N.W.";
            this.rbThingNW.UseVisualStyleBackColor = true;
            this.rbThingNW.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingNE
            // 
            this.rbThingNE.AutoSize = true;
            this.rbThingNE.Location = new System.Drawing.Point(116, 56);
            this.rbThingNE.Name = "rbThingNE";
            this.rbThingNE.Size = new System.Drawing.Size(49, 17);
            this.rbThingNE.TabIndex = 27;
            this.rbThingNE.TabStop = true;
            this.rbThingNE.Tag = "45";
            this.rbThingNE.Text = "N. E.";
            this.rbThingNE.UseVisualStyleBackColor = true;
            this.rbThingNE.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingSW
            // 
            this.rbThingSW.AutoSize = true;
            this.rbThingSW.Location = new System.Drawing.Point(6, 104);
            this.rbThingSW.Name = "rbThingSW";
            this.rbThingSW.Size = new System.Drawing.Size(49, 17);
            this.rbThingSW.TabIndex = 26;
            this.rbThingSW.TabStop = true;
            this.rbThingSW.Tag = "225";
            this.rbThingSW.Text = "S.W.";
            this.rbThingSW.UseVisualStyleBackColor = true;
            this.rbThingSW.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingSE
            // 
            this.rbThingSE.AutoSize = true;
            this.rbThingSE.Location = new System.Drawing.Point(116, 105);
            this.rbThingSE.Name = "rbThingSE";
            this.rbThingSE.Size = new System.Drawing.Size(48, 17);
            this.rbThingSE.TabIndex = 25;
            this.rbThingSE.TabStop = true;
            this.rbThingSE.Tag = "315";
            this.rbThingSE.Text = "S. E.";
            this.rbThingSE.UseVisualStyleBackColor = true;
            this.rbThingSE.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingSouth
            // 
            this.rbThingSouth.AutoSize = true;
            this.rbThingSouth.Location = new System.Drawing.Point(57, 105);
            this.rbThingSouth.Name = "rbThingSouth";
            this.rbThingSouth.Size = new System.Drawing.Size(53, 17);
            this.rbThingSouth.TabIndex = 24;
            this.rbThingSouth.TabStop = true;
            this.rbThingSouth.Tag = "270";
            this.rbThingSouth.Text = "South";
            this.rbThingSouth.UseVisualStyleBackColor = true;
            this.rbThingSouth.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingNorth
            // 
            this.rbThingNorth.AutoSize = true;
            this.rbThingNorth.Location = new System.Drawing.Point(57, 56);
            this.rbThingNorth.Name = "rbThingNorth";
            this.rbThingNorth.Size = new System.Drawing.Size(51, 17);
            this.rbThingNorth.TabIndex = 23;
            this.rbThingNorth.TabStop = true;
            this.rbThingNorth.Tag = "90";
            this.rbThingNorth.Text = "North";
            this.rbThingNorth.UseVisualStyleBackColor = true;
            this.rbThingNorth.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingWest
            // 
            this.rbThingWest.AutoSize = true;
            this.rbThingWest.Location = new System.Drawing.Point(6, 79);
            this.rbThingWest.Name = "rbThingWest";
            this.rbThingWest.Size = new System.Drawing.Size(50, 17);
            this.rbThingWest.TabIndex = 22;
            this.rbThingWest.TabStop = true;
            this.rbThingWest.Tag = "180";
            this.rbThingWest.Text = "West";
            this.rbThingWest.UseVisualStyleBackColor = true;
            this.rbThingWest.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // rbThingEast
            // 
            this.rbThingEast.AutoSize = true;
            this.rbThingEast.Location = new System.Drawing.Point(116, 79);
            this.rbThingEast.Name = "rbThingEast";
            this.rbThingEast.Size = new System.Drawing.Size(46, 17);
            this.rbThingEast.TabIndex = 16;
            this.rbThingEast.TabStop = true;
            this.rbThingEast.Tag = "0";
            this.rbThingEast.Text = "East";
            this.rbThingEast.UseVisualStyleBackColor = true;
            this.rbThingEast.CheckedChanged += new System.EventHandler(this.AngleRadioBox_CheckedChanged);
            // 
            // cbThingSkill4
            // 
            this.cbThingSkill4.AutoSize = true;
            this.cbThingSkill4.Checked = true;
            this.cbThingSkill4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill4.Location = new System.Drawing.Point(6, 254);
            this.cbThingSkill4.Name = "cbThingSkill4";
            this.cbThingSkill4.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill4.TabIndex = 21;
            this.cbThingSkill4.Tag = "skill4";
            this.cbThingSkill4.Text = "Skill 4";
            this.cbThingSkill4.UseVisualStyleBackColor = true;
            this.cbThingSkill4.CheckedChanged += new System.EventHandler(this.cbThingSkill4_CheckedChanged);
            // 
            // cbThingSkill3
            // 
            this.cbThingSkill3.AutoSize = true;
            this.cbThingSkill3.Checked = true;
            this.cbThingSkill3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill3.Location = new System.Drawing.Point(6, 231);
            this.cbThingSkill3.Name = "cbThingSkill3";
            this.cbThingSkill3.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill3.TabIndex = 20;
            this.cbThingSkill3.Tag = "skill3";
            this.cbThingSkill3.Text = "Skill 3";
            this.cbThingSkill3.UseVisualStyleBackColor = true;
            this.cbThingSkill3.CheckedChanged += new System.EventHandler(this.cbThingSkill3_CheckedChanged);
            // 
            // cbThingSkill2
            // 
            this.cbThingSkill2.AutoSize = true;
            this.cbThingSkill2.Checked = true;
            this.cbThingSkill2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill2.Location = new System.Drawing.Point(6, 208);
            this.cbThingSkill2.Name = "cbThingSkill2";
            this.cbThingSkill2.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill2.TabIndex = 19;
            this.cbThingSkill2.Tag = "skill2";
            this.cbThingSkill2.Text = "Skill 2";
            this.cbThingSkill2.UseVisualStyleBackColor = true;
            this.cbThingSkill2.CheckedChanged += new System.EventHandler(this.cbThingSkill2_CheckedChanged);
            // 
            // cbThingSkill1
            // 
            this.cbThingSkill1.AutoSize = true;
            this.cbThingSkill1.Checked = true;
            this.cbThingSkill1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThingSkill1.Location = new System.Drawing.Point(6, 185);
            this.cbThingSkill1.Name = "cbThingSkill1";
            this.cbThingSkill1.Size = new System.Drawing.Size(54, 17);
            this.cbThingSkill1.TabIndex = 18;
            this.cbThingSkill1.Tag = "skill1";
            this.cbThingSkill1.Text = "Skill 1";
            this.cbThingSkill1.UseVisualStyleBackColor = true;
            this.cbThingSkill1.CheckedChanged += new System.EventHandler(this.cbThingSkill1_CheckedChanged);
            // 
            // cbThingPatrol
            // 
            this.cbThingPatrol.AutoSize = true;
            this.cbThingPatrol.Location = new System.Drawing.Point(6, 163);
            this.cbThingPatrol.Name = "cbThingPatrol";
            this.cbThingPatrol.Size = new System.Drawing.Size(53, 17);
            this.cbThingPatrol.TabIndex = 17;
            this.cbThingPatrol.Tag = "patrol";
            this.cbThingPatrol.Text = "Patrol";
            this.cbThingPatrol.UseVisualStyleBackColor = true;
            this.cbThingPatrol.CheckedChanged += new System.EventHandler(this.cbThingPatrol_CheckedChanged);
            // 
            // cbThingAmbush
            // 
            this.cbThingAmbush.AutoSize = true;
            this.cbThingAmbush.Location = new System.Drawing.Point(6, 140);
            this.cbThingAmbush.Name = "cbThingAmbush";
            this.cbThingAmbush.Size = new System.Drawing.Size(64, 17);
            this.cbThingAmbush.TabIndex = 16;
            this.cbThingAmbush.Tag = "ambush";
            this.cbThingAmbush.Text = "Ambush";
            this.cbThingAmbush.UseVisualStyleBackColor = true;
            this.cbThingAmbush.CheckedChanged += new System.EventHandler(this.cbThingAmbush_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Bits:";
            // 
            // ThingAngleSpinner
            // 
            this.ThingAngleSpinner.Location = new System.Drawing.Point(62, 79);
            this.ThingAngleSpinner.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.ThingAngleSpinner.Name = "ThingAngleSpinner";
            this.ThingAngleSpinner.Size = new System.Drawing.Size(39, 20);
            this.ThingAngleSpinner.TabIndex = 16;
            this.ThingAngleSpinner.ValueChanged += new System.EventHandler(this.ThingAngleSpinner_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Angle";
            // 
            // ThingTypeComboBox
            // 
            this.ThingTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ThingTypeComboBox.FormattingEnabled = true;
            this.ThingTypeComboBox.Location = new System.Drawing.Point(3, 16);
            this.ThingTypeComboBox.Name = "ThingTypeComboBox";
            this.ThingTypeComboBox.Size = new System.Drawing.Size(159, 21);
            this.ThingTypeComboBox.TabIndex = 29;
            this.ThingTypeComboBox.TextChanged += new System.EventHandler(this.ThingTypeComboBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Thing Type";
            // 
            // ThingUIPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ThingTypeComboBox);
            this.Controls.Add(this.cbThingSkill4);
            this.Controls.Add(this.rbThingNW);
            this.Controls.Add(this.rbThingNE);
            this.Controls.Add(this.rbThingSW);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.rbThingSE);
            this.Controls.Add(this.ThingAngleSpinner);
            this.Controls.Add(this.rbThingSouth);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.rbThingNorth);
            this.Controls.Add(this.cbThingAmbush);
            this.Controls.Add(this.rbThingWest);
            this.Controls.Add(this.cbThingPatrol);
            this.Controls.Add(this.rbThingEast);
            this.Controls.Add(this.cbThingSkill1);
            this.Controls.Add(this.cbThingSkill2);
            this.Controls.Add(this.cbThingSkill3);
            this.Name = "ThingUIPanel";
            this.Size = new System.Drawing.Size(168, 275);
            ((System.ComponentModel.ISupportInitialize)(this.ThingAngleSpinner)).EndInit();
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown ThingAngleSpinner;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ThingTypeComboBox;
        private System.Windows.Forms.Label label1;
    }
}
