namespace eced
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.NewMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.OpenMenuItem = new System.Windows.Forms.MenuItem();
            this.SaveMenuItem = new System.Windows.Forms.MenuItem();
            this.SaveAsMenuItem = new System.Windows.Forms.MenuItem();
            this.SaveIntoMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.ExitMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.UndoMenuItem = new System.Windows.Forms.MenuItem();
            this.RedoMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.ClearUnusedElementMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.MapPropertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.PreferencesMenuItem = new System.Windows.Forms.MenuItem();
            this.tbToolPanel = new System.Windows.Forms.ToolBar();
            this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton18 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton13 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton14 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton15 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton16 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton17 = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SizeTemplatePanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ManageLayerButton = new System.Windows.Forms.Button();
            this.LayerNumSpinner = new System.Windows.Forms.NumericUpDown();
            this.GLControlSizeTemplate = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LayerNumSpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem10});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.NewMenuItem,
            this.menuItem4,
            this.OpenMenuItem,
            this.SaveMenuItem,
            this.SaveAsMenuItem,
            this.SaveIntoMenuItem,
            this.menuItem8,
            this.ExitMenuItem});
            this.menuItem1.Text = "File";
            // 
            // NewMenuItem
            // 
            this.NewMenuItem.Index = 0;
            this.NewMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.NewMenuItem.Text = "New";
            this.NewMenuItem.Click += new System.EventHandler(this.NewMenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "-";
            // 
            // OpenMenuItem
            // 
            this.OpenMenuItem.Index = 2;
            this.OpenMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.OpenMenuItem.Text = "Open...";
            this.OpenMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Index = 3;
            this.SaveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.SaveMenuItem.Text = "Save...";
            this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Index = 4;
            this.SaveAsMenuItem.Text = "Save As...";
            this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // SaveIntoMenuItem
            // 
            this.SaveIntoMenuItem.Index = 5;
            this.SaveIntoMenuItem.Text = "Save Into...";
            this.SaveIntoMenuItem.Click += new System.EventHandler(this.SaveIntoMenuItem_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 6;
            this.menuItem8.Text = "-";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Index = 7;
            this.ExitMenuItem.Text = "Exit";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.UndoMenuItem,
            this.RedoMenuItem,
            this.menuItem6,
            this.ClearUnusedElementMenuItem,
            this.menuItem3,
            this.MapPropertiesMenuItem});
            this.menuItem2.Text = "Edit";
            // 
            // UndoMenuItem
            // 
            this.UndoMenuItem.Index = 0;
            this.UndoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.UndoMenuItem.Text = "Undo";
            // 
            // RedoMenuItem
            // 
            this.RedoMenuItem.Index = 1;
            this.RedoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.RedoMenuItem.Text = "Redo";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "-";
            // 
            // ClearUnusedElementMenuItem
            // 
            this.ClearUnusedElementMenuItem.Index = 3;
            this.ClearUnusedElementMenuItem.Text = "Clear Unused Items";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 4;
            this.menuItem3.Text = "-";
            // 
            // MapPropertiesMenuItem
            // 
            this.MapPropertiesMenuItem.Index = 5;
            this.MapPropertiesMenuItem.Text = "Map Properties...";
            this.MapPropertiesMenuItem.Click += new System.EventHandler(this.MapPropertiesMenuItem_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.PreferencesMenuItem});
            this.menuItem10.Text = "Config";
            // 
            // PreferencesMenuItem
            // 
            this.PreferencesMenuItem.Index = 0;
            this.PreferencesMenuItem.Text = "Preferences...";
            // 
            // tbToolPanel
            // 
            this.tbToolPanel.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbToolPanel.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton10,
            this.toolBarButton11,
            this.toolBarButton12,
            this.toolBarButton9,
            this.toolBarButton1,
            this.toolBarButton2,
            this.toolBarButton3,
            this.toolBarButton4,
            this.toolBarButton5,
            this.toolBarButton6,
            this.toolBarButton7,
            this.toolBarButton8,
            this.toolBarButton18,
            this.toolBarButton13,
            this.toolBarButton14,
            this.toolBarButton15,
            this.toolBarButton16,
            this.toolBarButton17});
            this.tbToolPanel.Divider = false;
            this.tbToolPanel.DropDownArrows = true;
            this.tbToolPanel.ImageList = this.imageList1;
            this.tbToolPanel.Location = new System.Drawing.Point(0, 0);
            this.tbToolPanel.Name = "tbToolPanel";
            this.tbToolPanel.ShowToolTips = true;
            this.tbToolPanel.Size = new System.Drawing.Size(853, 26);
            this.tbToolPanel.TabIndex = 5;
            this.tbToolPanel.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton10
            // 
            this.toolBarButton10.ImageIndex = 9;
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Tag = "20";
            // 
            // toolBarButton11
            // 
            this.toolBarButton11.ImageIndex = 10;
            this.toolBarButton11.Name = "toolBarButton11";
            this.toolBarButton11.Tag = "21";
            // 
            // toolBarButton12
            // 
            this.toolBarButton12.ImageIndex = 8;
            this.toolBarButton12.Name = "toolBarButton12";
            this.toolBarButton12.Tag = "22";
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Tag = "0";
            this.toolBarButton1.ToolTipText = "Pointer";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 1;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Tag = "1";
            this.toolBarButton2.ToolTipText = "Subtract Brush";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.ImageIndex = 4;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Tag = "2";
            this.toolBarButton3.ToolTipText = "Tile Brush";
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.ImageIndex = 2;
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Tag = "3";
            this.toolBarButton4.ToolTipText = "Texture Brush";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.ImageIndex = 3;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Tag = "4";
            this.toolBarButton5.ToolTipText = "Thing Tool";
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.ImageIndex = 5;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Tag = "5";
            this.toolBarButton6.ToolTipText = "Trigger Tool";
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 6;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Tag = "6";
            this.toolBarButton7.ToolTipText = "Sector Paint Tool";
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.ImageIndex = 7;
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Tag = "7";
            this.toolBarButton8.ToolTipText = "Floor Fill Tool";
            // 
            // toolBarButton18
            // 
            this.toolBarButton18.ImageIndex = 15;
            this.toolBarButton18.Name = "toolBarButton18";
            this.toolBarButton18.Tag = "8";
            // 
            // toolBarButton13
            // 
            this.toolBarButton13.Name = "toolBarButton13";
            this.toolBarButton13.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            this.toolBarButton13.Tag = "";
            // 
            // toolBarButton14
            // 
            this.toolBarButton14.ImageIndex = 12;
            this.toolBarButton14.Name = "toolBarButton14";
            this.toolBarButton14.Tag = "100";
            this.toolBarButton14.ToolTipText = "Numeric Floorcode display";
            // 
            // toolBarButton15
            // 
            this.toolBarButton15.ImageIndex = 11;
            this.toolBarButton15.Name = "toolBarButton15";
            this.toolBarButton15.Tag = "101";
            this.toolBarButton15.ToolTipText = "Color-coded Floorcode display";
            // 
            // toolBarButton16
            // 
            this.toolBarButton16.ImageIndex = 13;
            this.toolBarButton16.Name = "toolBarButton16";
            this.toolBarButton16.Tag = "102";
            this.toolBarButton16.ToolTipText = "Floor texture display";
            // 
            // toolBarButton17
            // 
            this.toolBarButton17.ImageIndex = 14;
            this.toolBarButton17.Name = "toolBarButton17";
            this.toolBarButton17.Tag = "103";
            this.toolBarButton17.ToolTipText = "Ceiling texture display";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "Pointer.bmp");
            this.imageList1.Images.SetKeyName(1, "SubtractBrush.bmp");
            this.imageList1.Images.SetKeyName(2, "TextureBrush.bmp");
            this.imageList1.Images.SetKeyName(3, "ThingBrush.bmp");
            this.imageList1.Images.SetKeyName(4, "TileBrush.bmp");
            this.imageList1.Images.SetKeyName(5, "TriggerBrush.bmp");
            this.imageList1.Images.SetKeyName(6, "SectorPaintBrush.bmp");
            this.imageList1.Images.SetKeyName(7, "FloorTool.bmp");
            this.imageList1.Images.SetKeyName(8, "SaveIcon.bmp");
            this.imageList1.Images.SetKeyName(9, "NewIcon.bmp");
            this.imageList1.Images.SetKeyName(10, "OpenIcon.bmp");
            this.imageList1.Images.SetKeyName(11, "PreviewNone.bmp");
            this.imageList1.Images.SetKeyName(12, "PreviewCode.bmp");
            this.imageList1.Images.SetKeyName(13, "PreviewCeiling.bmp");
            this.imageList1.Images.SetKeyName(14, "PreviewFloor.bmp");
            this.imageList1.Images.SetKeyName(15, "TagTool.png");
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 602);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(853, 22);
            this.statusBar1.TabIndex = 6;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Text = "Ready";
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.Name = "statusBarPanel2";
            this.statusBarPanel2.Text = "Zoom: 0%";
            this.statusBarPanel2.Width = 125;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "WAD Files|*.wad";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "WAD Files|*.wad";
            // 
            // SizeTemplatePanel
            // 
            this.SizeTemplatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SizeTemplatePanel.Location = new System.Drawing.Point(0, 26);
            this.SizeTemplatePanel.Name = "SizeTemplatePanel";
            this.SizeTemplatePanel.Size = new System.Drawing.Size(172, 468);
            this.SizeTemplatePanel.TabIndex = 9;
            this.SizeTemplatePanel.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.ManageLayerButton);
            this.groupBox1.Controls.Add(this.LayerNumSpinner);
            this.groupBox1.Location = new System.Drawing.Point(0, 500);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 81);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Layers";
            // 
            // ManageLayerButton
            // 
            this.ManageLayerButton.Enabled = false;
            this.ManageLayerButton.Location = new System.Drawing.Point(12, 45);
            this.ManageLayerButton.Name = "ManageLayerButton";
            this.ManageLayerButton.Size = new System.Drawing.Size(75, 23);
            this.ManageLayerButton.TabIndex = 1;
            this.ManageLayerButton.Text = "Manage...";
            this.ManageLayerButton.UseVisualStyleBackColor = true;
            // 
            // LayerNumSpinner
            // 
            this.LayerNumSpinner.Enabled = false;
            this.LayerNumSpinner.Location = new System.Drawing.Point(12, 19);
            this.LayerNumSpinner.Name = "LayerNumSpinner";
            this.LayerNumSpinner.Size = new System.Drawing.Size(75, 20);
            this.LayerNumSpinner.TabIndex = 0;
            this.LayerNumSpinner.ValueChanged += new System.EventHandler(this.LayerNumSpinner_ValueChanged);
            // 
            // GLControlSizeTemplate
            // 
            this.GLControlSizeTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GLControlSizeTemplate.Location = new System.Drawing.Point(178, 26);
            this.GLControlSizeTemplate.Name = "GLControlSizeTemplate";
            this.GLControlSizeTemplate.Size = new System.Drawing.Size(675, 577);
            this.GLControlSizeTemplate.TabIndex = 11;
            this.GLControlSizeTemplate.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 624);
            this.Controls.Add(this.GLControlSizeTemplate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SizeTemplatePanel);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.tbToolPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(866, 663);
            this.Name = "Form1";
            this.Text = "eced";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Leave += new System.EventHandler(this.Form1_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LayerNumSpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem NewMenuItem;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem OpenMenuItem;
        private System.Windows.Forms.MenuItem SaveMenuItem;
        private System.Windows.Forms.MenuItem SaveAsMenuItem;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem ExitMenuItem;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem PreferencesMenuItem;
        private System.Windows.Forms.ToolBar tbToolPanel;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private System.Windows.Forms.ToolBarButton toolBarButton3;
        private System.Windows.Forms.ToolBarButton toolBarButton4;
        private System.Windows.Forms.ToolBarButton toolBarButton5;
        private System.Windows.Forms.ToolBarButton toolBarButton6;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.StatusBarPanel statusBarPanel2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolBarButton toolBarButton7;
        private System.Windows.Forms.ToolBarButton toolBarButton8;
        private System.Windows.Forms.ToolBarButton toolBarButton10;
        private System.Windows.Forms.ToolBarButton toolBarButton11;
        private System.Windows.Forms.ToolBarButton toolBarButton12;
        private System.Windows.Forms.ToolBarButton toolBarButton9;
        private System.Windows.Forms.ToolBarButton toolBarButton13;
        private System.Windows.Forms.ToolBarButton toolBarButton14;
        private System.Windows.Forms.ToolBarButton toolBarButton15;
        private System.Windows.Forms.ToolBarButton toolBarButton16;
        private System.Windows.Forms.ToolBarButton toolBarButton17;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolBarButton toolBarButton18;
        private System.Windows.Forms.MenuItem SaveIntoMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel SizeTemplatePanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ManageLayerButton;
        private System.Windows.Forms.NumericUpDown LayerNumSpinner;
        private System.Windows.Forms.MenuItem UndoMenuItem;
        private System.Windows.Forms.MenuItem RedoMenuItem;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem MapPropertiesMenuItem;
        private System.Windows.Forms.MenuItem ClearUnusedElementMenuItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.Panel GLControlSizeTemplate;
    }
}

