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
            this.Skill4CheckBox = new System.Windows.Forms.CheckBox();
            this.Skill3CheckBox = new System.Windows.Forms.CheckBox();
            this.Skill2CheckBox = new System.Windows.Forms.CheckBox();
            this.Skill1CheckBox = new System.Windows.Forms.CheckBox();
            this.PatrolCheckBox = new System.Windows.Forms.CheckBox();
            this.AmbushCheckBox = new System.Windows.Forms.CheckBox();
            this.ThingAngleSpinner = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DialogCancelButton = new System.Windows.Forms.Button();
            this.DialogOKButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.XPosTextBox = new System.Windows.Forms.TextBox();
            this.YPosTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ZPosTextBox = new System.Windows.Forms.TextBox();
            this.ThingTypeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ThingAngleSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // rbThingNW
            // 
            this.rbThingNW.AutoSize = true;
            this.rbThingNW.Location = new System.Drawing.Point(85, 105);
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
            this.rbThingNE.Location = new System.Drawing.Point(195, 105);
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
            this.rbThingSW.Location = new System.Drawing.Point(85, 153);
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
            this.rbThingSE.Location = new System.Drawing.Point(195, 154);
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
            this.rbThingSouth.Location = new System.Drawing.Point(136, 154);
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
            this.rbThingNorth.Location = new System.Drawing.Point(136, 105);
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
            this.rbThingWest.Location = new System.Drawing.Point(85, 128);
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
            this.rbThingEast.Location = new System.Drawing.Point(195, 128);
            this.rbThingEast.Name = "rbThingEast";
            this.rbThingEast.Size = new System.Drawing.Size(46, 17);
            this.rbThingEast.TabIndex = 30;
            this.rbThingEast.TabStop = true;
            this.rbThingEast.Text = "East";
            this.rbThingEast.UseVisualStyleBackColor = true;
            this.rbThingEast.CheckedChanged += new System.EventHandler(this.rbThingEast_CheckedChanged);
            // 
            // Skill4CheckBox
            // 
            this.Skill4CheckBox.AutoSize = true;
            this.Skill4CheckBox.Checked = true;
            this.Skill4CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Skill4CheckBox.Location = new System.Drawing.Point(15, 172);
            this.Skill4CheckBox.Name = "Skill4CheckBox";
            this.Skill4CheckBox.Size = new System.Drawing.Size(54, 17);
            this.Skill4CheckBox.TabIndex = 37;
            this.Skill4CheckBox.Text = "Skill 4";
            this.Skill4CheckBox.UseVisualStyleBackColor = true;
            this.Skill4CheckBox.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // Skill3CheckBox
            // 
            this.Skill3CheckBox.AutoSize = true;
            this.Skill3CheckBox.Checked = true;
            this.Skill3CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Skill3CheckBox.Location = new System.Drawing.Point(15, 150);
            this.Skill3CheckBox.Name = "Skill3CheckBox";
            this.Skill3CheckBox.Size = new System.Drawing.Size(54, 17);
            this.Skill3CheckBox.TabIndex = 36;
            this.Skill3CheckBox.Text = "Skill 3";
            this.Skill3CheckBox.UseVisualStyleBackColor = true;
            this.Skill3CheckBox.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // Skill2CheckBox
            // 
            this.Skill2CheckBox.AutoSize = true;
            this.Skill2CheckBox.Checked = true;
            this.Skill2CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Skill2CheckBox.Location = new System.Drawing.Point(15, 127);
            this.Skill2CheckBox.Name = "Skill2CheckBox";
            this.Skill2CheckBox.Size = new System.Drawing.Size(54, 17);
            this.Skill2CheckBox.TabIndex = 35;
            this.Skill2CheckBox.Text = "Skill 2";
            this.Skill2CheckBox.UseVisualStyleBackColor = true;
            this.Skill2CheckBox.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // Skill1CheckBox
            // 
            this.Skill1CheckBox.AutoSize = true;
            this.Skill1CheckBox.Checked = true;
            this.Skill1CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Skill1CheckBox.Location = new System.Drawing.Point(15, 104);
            this.Skill1CheckBox.Name = "Skill1CheckBox";
            this.Skill1CheckBox.Size = new System.Drawing.Size(54, 17);
            this.Skill1CheckBox.TabIndex = 34;
            this.Skill1CheckBox.Text = "Skill 1";
            this.Skill1CheckBox.UseVisualStyleBackColor = true;
            this.Skill1CheckBox.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // PatrolCheckBox
            // 
            this.PatrolCheckBox.AutoSize = true;
            this.PatrolCheckBox.Checked = true;
            this.PatrolCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PatrolCheckBox.Location = new System.Drawing.Point(15, 82);
            this.PatrolCheckBox.Name = "PatrolCheckBox";
            this.PatrolCheckBox.Size = new System.Drawing.Size(53, 17);
            this.PatrolCheckBox.TabIndex = 33;
            this.PatrolCheckBox.Text = "Patrol";
            this.PatrolCheckBox.UseVisualStyleBackColor = true;
            this.PatrolCheckBox.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // AmbushCheckBox
            // 
            this.AmbushCheckBox.AutoSize = true;
            this.AmbushCheckBox.Checked = true;
            this.AmbushCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AmbushCheckBox.Location = new System.Drawing.Point(15, 59);
            this.AmbushCheckBox.Name = "AmbushCheckBox";
            this.AmbushCheckBox.Size = new System.Drawing.Size(64, 17);
            this.AmbushCheckBox.TabIndex = 31;
            this.AmbushCheckBox.Text = "Ambush";
            this.AmbushCheckBox.UseVisualStyleBackColor = true;
            this.AmbushCheckBox.CheckedChanged += new System.EventHandler(this.thingBitChange);
            // 
            // ThingAngleSpinner
            // 
            this.ThingAngleSpinner.Location = new System.Drawing.Point(141, 128);
            this.ThingAngleSpinner.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.ThingAngleSpinner.Name = "ThingAngleSpinner";
            this.ThingAngleSpinner.Size = new System.Drawing.Size(39, 20);
            this.ThingAngleSpinner.TabIndex = 32;
            this.ThingAngleSpinner.ValueChanged += new System.EventHandler(this.ndThingAngle_ValueChanged);
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
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Flags";
            // 
            // DialogCancelButton
            // 
            this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DialogCancelButton.Location = new System.Drawing.Point(164, 199);
            this.DialogCancelButton.Name = "DialogCancelButton";
            this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DialogCancelButton.TabIndex = 47;
            this.DialogCancelButton.Text = "Cancel";
            this.DialogCancelButton.UseVisualStyleBackColor = true;
            // 
            // DialogOKButton
            // 
            this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DialogOKButton.Location = new System.Drawing.Point(83, 199);
            this.DialogOKButton.Name = "DialogOKButton";
            this.DialogOKButton.Size = new System.Drawing.Size(75, 23);
            this.DialogOKButton.TabIndex = 48;
            this.DialogOKButton.Text = "OK";
            this.DialogOKButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(85, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "Position (map units):";
            // 
            // XPosTextBox
            // 
            this.XPosTextBox.Location = new System.Drawing.Point(85, 79);
            this.XPosTextBox.Name = "XPosTextBox";
            this.XPosTextBox.Size = new System.Drawing.Size(43, 20);
            this.XPosTextBox.TabIndex = 50;
            // 
            // YPosTextBox
            // 
            this.YPosTextBox.Location = new System.Drawing.Point(134, 79);
            this.YPosTextBox.Name = "YPosTextBox";
            this.YPosTextBox.Size = new System.Drawing.Size(43, 20);
            this.YPosTextBox.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(133, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 54;
            this.label5.Text = "Y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(180, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Z";
            // 
            // ZPosTextBox
            // 
            this.ZPosTextBox.Location = new System.Drawing.Point(183, 79);
            this.ZPosTextBox.Name = "ZPosTextBox";
            this.ZPosTextBox.Size = new System.Drawing.Size(43, 20);
            this.ZPosTextBox.TabIndex = 52;
            // 
            // ThingTypeComboBox
            // 
            this.ThingTypeComboBox.FormattingEnabled = true;
            this.ThingTypeComboBox.Location = new System.Drawing.Point(52, 6);
            this.ThingTypeComboBox.Name = "ThingTypeComboBox";
            this.ThingTypeComboBox.Size = new System.Drawing.Size(127, 21);
            this.ThingTypeComboBox.TabIndex = 56;
            // 
            // ThingEditor
            // 
            this.AcceptButton = this.DialogOKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DialogCancelButton;
            this.ClientSize = new System.Drawing.Size(251, 234);
            this.Controls.Add(this.ThingTypeComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ZPosTextBox);
            this.Controls.Add(this.YPosTextBox);
            this.Controls.Add(this.XPosTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DialogOKButton);
            this.Controls.Add(this.DialogCancelButton);
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
            this.Controls.Add(this.Skill4CheckBox);
            this.Controls.Add(this.Skill3CheckBox);
            this.Controls.Add(this.Skill2CheckBox);
            this.Controls.Add(this.Skill1CheckBox);
            this.Controls.Add(this.PatrolCheckBox);
            this.Controls.Add(this.AmbushCheckBox);
            this.Controls.Add(this.ThingAngleSpinner);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThingEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editing Thing";
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
        private System.Windows.Forms.CheckBox Skill4CheckBox;
        private System.Windows.Forms.CheckBox Skill3CheckBox;
        private System.Windows.Forms.CheckBox Skill2CheckBox;
        private System.Windows.Forms.CheckBox Skill1CheckBox;
        private System.Windows.Forms.CheckBox PatrolCheckBox;
        private System.Windows.Forms.CheckBox AmbushCheckBox;
        private System.Windows.Forms.NumericUpDown ThingAngleSpinner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DialogCancelButton;
        private System.Windows.Forms.Button DialogOKButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox XPosTextBox;
        private System.Windows.Forms.TextBox YPosTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ZPosTextBox;
        private System.Windows.Forms.ComboBox ThingTypeComboBox;
    }
}