namespace eced
{
    partial class TriggerEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbTriggerList = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ndParam5 = new System.Windows.Forms.NumericUpDown();
            this.ndParam4 = new System.Windows.Forms.NumericUpDown();
            this.ndParam3 = new System.Windows.Forms.NumericUpDown();
            this.ndParam2 = new System.Windows.Forms.NumericUpDown();
            this.ndParam1 = new System.Windows.Forms.NumericUpDown();
            this.Arg5Label = new System.Windows.Forms.Label();
            this.Arg4Label = new System.Windows.Forms.Label();
            this.Arg3Label = new System.Windows.Forms.Label();
            this.Arg2Label = new System.Windows.Forms.Label();
            this.Arg1Label = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSecret = new System.Windows.Forms.CheckBox();
            this.cbRepeat = new System.Windows.Forms.CheckBox();
            this.cbCross = new System.Windows.Forms.CheckBox();
            this.cbUseMonst = new System.Windows.Forms.CheckBox();
            this.cbUse = new System.Windows.Forms.CheckBox();
            this.cbTrigWest = new System.Windows.Forms.CheckBox();
            this.cbTrigSouth = new System.Windows.Forms.CheckBox();
            this.cbTrigEast = new System.Windows.Forms.CheckBox();
            this.cbTrigNorth = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TriggerComboBox = new System.Windows.Forms.ComboBox();
            this.CloseDialogButton = new System.Windows.Forms.Button();
            this.AcceptDialogButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Triggers on this cell:";
            // 
            // cbTriggerList
            // 
            this.cbTriggerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTriggerList.FormattingEnabled = true;
            this.cbTriggerList.Location = new System.Drawing.Point(12, 25);
            this.cbTriggerList.Name = "cbTriggerList";
            this.cbTriggerList.Size = new System.Drawing.Size(113, 21);
            this.cbTriggerList.TabIndex = 1;
            this.cbTriggerList.SelectedIndexChanged += new System.EventHandler(this.cbTriggerList_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 52);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(50, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(68, 52);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(57, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ndParam5);
            this.groupBox1.Controls.Add(this.ndParam4);
            this.groupBox1.Controls.Add(this.ndParam3);
            this.groupBox1.Controls.Add(this.ndParam2);
            this.groupBox1.Controls.Add(this.ndParam1);
            this.groupBox1.Controls.Add(this.Arg5Label);
            this.groupBox1.Controls.Add(this.Arg4Label);
            this.groupBox1.Controls.Add(this.Arg3Label);
            this.groupBox1.Controls.Add(this.Arg2Label);
            this.groupBox1.Controls.Add(this.Arg1Label);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbSecret);
            this.groupBox1.Controls.Add(this.cbRepeat);
            this.groupBox1.Controls.Add(this.cbCross);
            this.groupBox1.Controls.Add(this.cbUseMonst);
            this.groupBox1.Controls.Add(this.cbUse);
            this.groupBox1.Controls.Add(this.cbTrigWest);
            this.groupBox1.Controls.Add(this.cbTrigSouth);
            this.groupBox1.Controls.Add(this.cbTrigEast);
            this.groupBox1.Controls.Add(this.cbTrigNorth);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.TriggerComboBox);
            this.groupBox1.Location = new System.Drawing.Point(131, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 410);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Trigger";
            // 
            // ndParam5
            // 
            this.ndParam5.Location = new System.Drawing.Point(9, 372);
            this.ndParam5.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ndParam5.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ndParam5.Name = "ndParam5";
            this.ndParam5.Size = new System.Drawing.Size(61, 20);
            this.ndParam5.TabIndex = 59;
            this.ndParam5.ValueChanged += new System.EventHandler(this.triggerParams);
            // 
            // ndParam4
            // 
            this.ndParam4.Location = new System.Drawing.Point(9, 333);
            this.ndParam4.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ndParam4.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ndParam4.Name = "ndParam4";
            this.ndParam4.Size = new System.Drawing.Size(61, 20);
            this.ndParam4.TabIndex = 58;
            this.ndParam4.ValueChanged += new System.EventHandler(this.triggerParams);
            // 
            // ndParam3
            // 
            this.ndParam3.Location = new System.Drawing.Point(9, 294);
            this.ndParam3.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ndParam3.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ndParam3.Name = "ndParam3";
            this.ndParam3.Size = new System.Drawing.Size(61, 20);
            this.ndParam3.TabIndex = 57;
            this.ndParam3.ValueChanged += new System.EventHandler(this.triggerParams);
            // 
            // ndParam2
            // 
            this.ndParam2.Location = new System.Drawing.Point(9, 255);
            this.ndParam2.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ndParam2.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ndParam2.Name = "ndParam2";
            this.ndParam2.Size = new System.Drawing.Size(61, 20);
            this.ndParam2.TabIndex = 56;
            this.ndParam2.ValueChanged += new System.EventHandler(this.triggerParams);
            // 
            // ndParam1
            // 
            this.ndParam1.Location = new System.Drawing.Point(9, 216);
            this.ndParam1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ndParam1.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.ndParam1.Name = "ndParam1";
            this.ndParam1.Size = new System.Drawing.Size(61, 20);
            this.ndParam1.TabIndex = 42;
            this.ndParam1.ValueChanged += new System.EventHandler(this.triggerParams);
            // 
            // Arg5Label
            // 
            this.Arg5Label.AutoSize = true;
            this.Arg5Label.Location = new System.Drawing.Point(6, 356);
            this.Arg5Label.Name = "Arg5Label";
            this.Arg5Label.Size = new System.Drawing.Size(32, 13);
            this.Arg5Label.TabIndex = 55;
            this.Arg5Label.Text = "Arg 5";
            // 
            // Arg4Label
            // 
            this.Arg4Label.AutoSize = true;
            this.Arg4Label.Location = new System.Drawing.Point(6, 317);
            this.Arg4Label.Name = "Arg4Label";
            this.Arg4Label.Size = new System.Drawing.Size(32, 13);
            this.Arg4Label.TabIndex = 54;
            this.Arg4Label.Text = "Arg 4";
            // 
            // Arg3Label
            // 
            this.Arg3Label.AutoSize = true;
            this.Arg3Label.Location = new System.Drawing.Point(6, 278);
            this.Arg3Label.Name = "Arg3Label";
            this.Arg3Label.Size = new System.Drawing.Size(32, 13);
            this.Arg3Label.TabIndex = 53;
            this.Arg3Label.Text = "Arg 3";
            // 
            // Arg2Label
            // 
            this.Arg2Label.AutoSize = true;
            this.Arg2Label.Location = new System.Drawing.Point(6, 239);
            this.Arg2Label.Name = "Arg2Label";
            this.Arg2Label.Size = new System.Drawing.Size(32, 13);
            this.Arg2Label.TabIndex = 52;
            this.Arg2Label.Text = "Arg 2";
            // 
            // Arg1Label
            // 
            this.Arg1Label.AutoSize = true;
            this.Arg1Label.Location = new System.Drawing.Point(6, 197);
            this.Arg1Label.Name = "Arg1Label";
            this.Arg1Label.Size = new System.Drawing.Size(32, 13);
            this.Arg1Label.TabIndex = 51;
            this.Arg1Label.Text = "Arg 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 50;
            this.label4.Text = "Arguments";
            // 
            // cbSecret
            // 
            this.cbSecret.AutoSize = true;
            this.cbSecret.Location = new System.Drawing.Point(207, 103);
            this.cbSecret.Name = "cbSecret";
            this.cbSecret.Size = new System.Drawing.Size(57, 17);
            this.cbSecret.TabIndex = 49;
            this.cbSecret.Text = "Secret";
            this.cbSecret.UseVisualStyleBackColor = true;
            this.cbSecret.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbRepeat
            // 
            this.cbRepeat.AutoSize = true;
            this.cbRepeat.Location = new System.Drawing.Point(208, 80);
            this.cbRepeat.Name = "cbRepeat";
            this.cbRepeat.Size = new System.Drawing.Size(61, 17);
            this.cbRepeat.TabIndex = 48;
            this.cbRepeat.Text = "Repeat";
            this.cbRepeat.UseVisualStyleBackColor = true;
            this.cbRepeat.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbCross
            // 
            this.cbCross.AutoSize = true;
            this.cbCross.Location = new System.Drawing.Point(116, 126);
            this.cbCross.Name = "cbCross";
            this.cbCross.Size = new System.Drawing.Size(84, 17);
            this.cbCross.TabIndex = 47;
            this.cbCross.Text = "Player Cross";
            this.cbCross.UseVisualStyleBackColor = true;
            this.cbCross.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbUseMonst
            // 
            this.cbUseMonst.AutoSize = true;
            this.cbUseMonst.Location = new System.Drawing.Point(116, 80);
            this.cbUseMonst.Name = "cbUseMonst";
            this.cbUseMonst.Size = new System.Drawing.Size(86, 17);
            this.cbUseMonst.TabIndex = 46;
            this.cbUseMonst.Text = "Monster Use";
            this.cbUseMonst.UseVisualStyleBackColor = true;
            this.cbUseMonst.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbUse
            // 
            this.cbUse.AutoSize = true;
            this.cbUse.Location = new System.Drawing.Point(116, 103);
            this.cbUse.Name = "cbUse";
            this.cbUse.Size = new System.Drawing.Size(77, 17);
            this.cbUse.TabIndex = 45;
            this.cbUse.Text = "Player Use";
            this.cbUse.UseVisualStyleBackColor = true;
            this.cbUse.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbTrigWest
            // 
            this.cbTrigWest.AutoSize = true;
            this.cbTrigWest.Location = new System.Drawing.Point(6, 103);
            this.cbTrigWest.Name = "cbTrigWest";
            this.cbTrigWest.Size = new System.Drawing.Size(51, 17);
            this.cbTrigWest.TabIndex = 44;
            this.cbTrigWest.Text = "West";
            this.cbTrigWest.UseVisualStyleBackColor = true;
            this.cbTrigWest.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbTrigSouth
            // 
            this.cbTrigSouth.AutoSize = true;
            this.cbTrigSouth.Location = new System.Drawing.Point(34, 126);
            this.cbTrigSouth.Name = "cbTrigSouth";
            this.cbTrigSouth.Size = new System.Drawing.Size(54, 17);
            this.cbTrigSouth.TabIndex = 43;
            this.cbTrigSouth.Text = "South";
            this.cbTrigSouth.UseVisualStyleBackColor = true;
            this.cbTrigSouth.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbTrigEast
            // 
            this.cbTrigEast.AutoSize = true;
            this.cbTrigEast.Location = new System.Drawing.Point(63, 103);
            this.cbTrigEast.Name = "cbTrigEast";
            this.cbTrigEast.Size = new System.Drawing.Size(47, 17);
            this.cbTrigEast.TabIndex = 41;
            this.cbTrigEast.Text = "East";
            this.cbTrigEast.UseVisualStyleBackColor = true;
            this.cbTrigEast.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // cbTrigNorth
            // 
            this.cbTrigNorth.AutoSize = true;
            this.cbTrigNorth.Location = new System.Drawing.Point(36, 80);
            this.cbTrigNorth.Name = "cbTrigNorth";
            this.cbTrigNorth.Size = new System.Drawing.Size(52, 17);
            this.cbTrigNorth.TabIndex = 39;
            this.cbTrigNorth.Text = "North";
            this.cbTrigNorth.UseVisualStyleBackColor = true;
            this.cbTrigNorth.CheckedChanged += new System.EventHandler(this.triggerThingFlags);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Activation";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Trigger Type";
            // 
            // TriggerComboBox
            // 
            this.TriggerComboBox.FormattingEnabled = true;
            this.TriggerComboBox.Location = new System.Drawing.Point(6, 40);
            this.TriggerComboBox.Name = "TriggerComboBox";
            this.TriggerComboBox.Size = new System.Drawing.Size(121, 21);
            this.TriggerComboBox.TabIndex = 37;
            this.TriggerComboBox.SelectedIndexChanged += new System.EventHandler(this.cbTriggerType_SelectedIndexChanged);
            this.TriggerComboBox.TextChanged += new System.EventHandler(this.TriggerComboBox_TextChanged);
            // 
            // CloseDialogButton
            // 
            this.CloseDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseDialogButton.Location = new System.Drawing.Point(338, 425);
            this.CloseDialogButton.Name = "CloseDialogButton";
            this.CloseDialogButton.Size = new System.Drawing.Size(75, 23);
            this.CloseDialogButton.TabIndex = 4;
            this.CloseDialogButton.Text = "Cancel";
            this.CloseDialogButton.UseVisualStyleBackColor = true;
            // 
            // AcceptDialogButton
            // 
            this.AcceptDialogButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AcceptDialogButton.Location = new System.Drawing.Point(258, 425);
            this.AcceptDialogButton.Name = "AcceptDialogButton";
            this.AcceptDialogButton.Size = new System.Drawing.Size(75, 23);
            this.AcceptDialogButton.TabIndex = 5;
            this.AcceptDialogButton.Text = "OK";
            this.AcceptDialogButton.UseVisualStyleBackColor = true;
            // 
            // TriggerEditor
            // 
            this.AcceptButton = this.AcceptDialogButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseDialogButton;
            this.ClientSize = new System.Drawing.Size(425, 459);
            this.Controls.Add(this.AcceptDialogButton);
            this.Controls.Add(this.CloseDialogButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbTriggerList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TriggerEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editing Triggers";
            this.Load += new System.EventHandler(this.TriggerEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndParam1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTriggerList;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown ndParam5;
        private System.Windows.Forms.NumericUpDown ndParam4;
        private System.Windows.Forms.NumericUpDown ndParam3;
        private System.Windows.Forms.NumericUpDown ndParam2;
        private System.Windows.Forms.NumericUpDown ndParam1;
        private System.Windows.Forms.Label Arg5Label;
        private System.Windows.Forms.Label Arg4Label;
        private System.Windows.Forms.Label Arg3Label;
        private System.Windows.Forms.Label Arg2Label;
        private System.Windows.Forms.Label Arg1Label;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbSecret;
        private System.Windows.Forms.CheckBox cbRepeat;
        private System.Windows.Forms.CheckBox cbCross;
        private System.Windows.Forms.CheckBox cbUseMonst;
        private System.Windows.Forms.CheckBox cbUse;
        private System.Windows.Forms.CheckBox cbTrigWest;
        private System.Windows.Forms.CheckBox cbTrigSouth;
        private System.Windows.Forms.CheckBox cbTrigEast;
        private System.Windows.Forms.CheckBox cbTrigNorth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox TriggerComboBox;
        private System.Windows.Forms.Button CloseDialogButton;
        private System.Windows.Forms.Button AcceptDialogButton;
    }
}