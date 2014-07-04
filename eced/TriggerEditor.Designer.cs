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
            this.lbArg5 = new System.Windows.Forms.Label();
            this.lbArg4 = new System.Windows.Forms.Label();
            this.lbArg3 = new System.Windows.Forms.Label();
            this.lbArg2 = new System.Windows.Forms.Label();
            this.lbArg1 = new System.Windows.Forms.Label();
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
            this.cbTriggerType = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
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
            this.groupBox1.Controls.Add(this.lbArg5);
            this.groupBox1.Controls.Add(this.lbArg4);
            this.groupBox1.Controls.Add(this.lbArg3);
            this.groupBox1.Controls.Add(this.lbArg2);
            this.groupBox1.Controls.Add(this.lbArg1);
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
            this.groupBox1.Controls.Add(this.cbTriggerType);
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
            // lbArg5
            // 
            this.lbArg5.AutoSize = true;
            this.lbArg5.Location = new System.Drawing.Point(6, 356);
            this.lbArg5.Name = "lbArg5";
            this.lbArg5.Size = new System.Drawing.Size(32, 13);
            this.lbArg5.TabIndex = 55;
            this.lbArg5.Text = "Arg 5";
            // 
            // lbArg4
            // 
            this.lbArg4.AutoSize = true;
            this.lbArg4.Location = new System.Drawing.Point(6, 317);
            this.lbArg4.Name = "lbArg4";
            this.lbArg4.Size = new System.Drawing.Size(32, 13);
            this.lbArg4.TabIndex = 54;
            this.lbArg4.Text = "Arg 4";
            // 
            // lbArg3
            // 
            this.lbArg3.AutoSize = true;
            this.lbArg3.Location = new System.Drawing.Point(6, 278);
            this.lbArg3.Name = "lbArg3";
            this.lbArg3.Size = new System.Drawing.Size(32, 13);
            this.lbArg3.TabIndex = 53;
            this.lbArg3.Text = "Arg 3";
            // 
            // lbArg2
            // 
            this.lbArg2.AutoSize = true;
            this.lbArg2.Location = new System.Drawing.Point(6, 239);
            this.lbArg2.Name = "lbArg2";
            this.lbArg2.Size = new System.Drawing.Size(32, 13);
            this.lbArg2.TabIndex = 52;
            this.lbArg2.Text = "Arg 2";
            // 
            // lbArg1
            // 
            this.lbArg1.AutoSize = true;
            this.lbArg1.Location = new System.Drawing.Point(6, 197);
            this.lbArg1.Name = "lbArg1";
            this.lbArg1.Size = new System.Drawing.Size(32, 13);
            this.lbArg1.TabIndex = 51;
            this.lbArg1.Text = "Arg 1";
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
            this.cbSecret.Location = new System.Drawing.Point(73, 153);
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
            this.cbRepeat.Location = new System.Drawing.Point(6, 153);
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
            this.cbCross.Location = new System.Drawing.Point(181, 130);
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
            this.cbUseMonst.Location = new System.Drawing.Point(6, 130);
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
            this.cbUse.Location = new System.Drawing.Point(98, 130);
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
            this.cbTrigWest.Location = new System.Drawing.Point(64, 107);
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
            this.cbTrigSouth.Location = new System.Drawing.Point(6, 107);
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
            this.cbTrigEast.Location = new System.Drawing.Point(64, 84);
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
            this.cbTrigNorth.Location = new System.Drawing.Point(6, 84);
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
            // cbTriggerType
            // 
            this.cbTriggerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTriggerType.FormattingEnabled = true;
            this.cbTriggerType.Items.AddRange(new object[] {
            "1: Door Open",
            "2: Pushwall Move",
            "3: Exit Normal",
            "4: Exit Secret",
            "5: Teleport_NewMap",
            "6: Exit_VictorySpin",
            "7: Exit_Victory",
            "8: Trigger_Execute",
            "9: StartConversation",
            "10: Door_Elevator",
            "11: Elevator_SwitchFloor"});
            this.cbTriggerType.Location = new System.Drawing.Point(6, 40);
            this.cbTriggerType.Name = "cbTriggerType";
            this.cbTriggerType.Size = new System.Drawing.Size(121, 21);
            this.cbTriggerType.TabIndex = 37;
            this.cbTriggerType.SelectedIndexChanged += new System.EventHandler(this.cbTriggerType_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 425);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 459);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbTriggerList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TriggerEditor";
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
        private System.Windows.Forms.Label lbArg5;
        private System.Windows.Forms.Label lbArg4;
        private System.Windows.Forms.Label lbArg3;
        private System.Windows.Forms.Label lbArg2;
        private System.Windows.Forms.Label lbArg1;
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
        private System.Windows.Forms.ComboBox cbTriggerType;
        private System.Windows.Forms.Button button1;
    }
}