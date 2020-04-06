/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
 *
 *  eced is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *   eced is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with eced.  If not, see <http://www.gnu.org/licenses/>.
 *  -------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using eced.Renderer;

using eced.UIPanels;
using eced.Brushes;
using eced.ResourceFiles;

namespace eced
{
    public partial class Form1 : Form
    {
        private int toolid = 1, oldtoolid = 1;
        private RendererState renderer;
        private WorldRenderer worldRenderer;
        private bool ready = false;

        private Vector2 pan;

        private float zoom = 1.0f;

        private bool brushmode = false;
        private int heldMouseButton = 0;

        private OpenTK.Vector2 lastMousePos = new Vector2();

        private WallUIPanel gbTileSelection;
        private ThingUIPanel gbThingSelect;
        private ZoneUIPanel gbZoneList;
        private TriggerUIPanel gbTriggerData;
        private SectorUIPanel gbSectorPanel;
        private TagUIPanel gbTag;

        private EditorState editorState = new EditorState();

        public Form1()
        {
            InitializeComponent();
            this.SuspendLayout();
            gbTileSelection = new WallUIPanel();
            components.Add(gbTileSelection);
            gbTileSelection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            gbTileSelection.Location = SizeTemplatePanel.Location;
            gbTileSelection.Size = SizeTemplatePanel.Size;
            Controls.Add(gbTileSelection);

            gbThingSelect = new ThingUIPanel();
            components.Add(gbThingSelect);
            gbThingSelect.Location = SizeTemplatePanel.Location;
            gbThingSelect.Size = SizeTemplatePanel.Size;
            gbThingSelect.Anchor |= AnchorStyles.Bottom;
            Controls.Add(gbThingSelect);

            gbZoneList = new ZoneUIPanel(editorState);
            components.Add(gbZoneList);
            gbZoneList.Location = SizeTemplatePanel.Location;
            gbZoneList.Size = SizeTemplatePanel.Size;
            gbZoneList.Anchor |= AnchorStyles.Bottom;
            Controls.Add(gbZoneList);

            gbTriggerData = new TriggerUIPanel();
            components.Add(gbTriggerData);
            gbTriggerData.Location = SizeTemplatePanel.Location;
            gbTriggerData.Size = SizeTemplatePanel.Size;
            gbTriggerData.Anchor |= AnchorStyles.Bottom;
            Controls.Add(gbTriggerData);

            gbSectorPanel = new SectorUIPanel();
            components.Add(gbSectorPanel);
            gbSectorPanel.Location = SizeTemplatePanel.Location;
            gbSectorPanel.Size = SizeTemplatePanel.Size;
            gbSectorPanel.Anchor |= AnchorStyles.Bottom;
            Controls.Add(gbSectorPanel);

            gbTag = new TagUIPanel();
            components.Add(gbTag);
            gbTag.Location = SizeTemplatePanel.Location;
            gbTag.Size = SizeTemplatePanel.Size;
            gbTag.Anchor |= AnchorStyles.Bottom;
            Controls.Add(gbTag);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.gbThingSelect.Visible = false;
            this.gbTileSelection.Visible = false;
            this.gbTriggerData.Visible = false;
            this.gbZoneList.Visible = false;
            this.gbSectorPanel.Visible = false;
            this.gbTag.Visible = false;

            renderer = new RendererState(editorState);
            renderer.Init();
            renderer.SetViewSize(mainLevelPanel.Width, mainLevelPanel.Height);
            UpdateZoom();
            worldRenderer = new WorldRenderer(renderer);

            GL.Disable(EnableCap.DepthTest);
            mainLevelPanel.MouseWheel += MainLevelPanel_MouseWheel;
        }

        private void UpdateWindowTitle()
        {
            if (editorState.CurrentLevel == null)
            {
                Text = "eced";
            }
            else
            {
                Text = string.Format("{0} - eced", editorState.EditorIOState.LastFilename);
            }
        }

        private void DoNewMapDialog()
        {
            NewMapDialog nmd = new NewMapDialog();
            nmd.ShowDialog();

            if (nmd.DialogResult == DialogResult.OK)
            {
                if (editorState.CurrentLevel != null)
                {
                    editorState.CloseLevel();
                    renderer.Textures.DestroyAtlas();
                }
                editorState.CreateNewLevel(nmd.CurrentMap);
                LevelChanged();
            }

            nmd.Dispose();
        }

        private void LevelChanged()
        {
            RebuildResources();
            SelectTool(1);
            worldRenderer.LevelChanged();
            UpdateWindowTitle();
            mainLevelPanel.Invalidate();
        }

        private void RebuildResources()
        {
            renderer.Textures.DestroyAtlas();
            renderer.Textures.AllocateAtlasTexture();
            renderer.Textures.InitAtlas();
            for (int i = 0; i < editorState.CurrentLevel.loadedResources.Count; i++)
            {
                ResourceFiles.Archive file = editorState.CurrentLevel.loadedResources[i];
                file.OpenFile();
                renderer.Textures.AddArchiveTextures(file);
                file.CloseFile();
            }
            renderer.Textures.GenerateAtlasInfoTexture();
            renderer.Textures.CreateZoneNumberTexture();

            //world.setupTextures(editorState.CurrentLevel);
        }

        private void UpdateCurrentZoneList()
        {
            //TODO: hack
            List<Zone> zonelist = editorState.CurrentLevel.GetZones();
            gbZoneList.SetZones(zonelist);
        }

        private void DoLoadDialog()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != "")
                {
                    WADArchive archive = (WADArchive)WADArchive.loadResourceFile(openFileDialog1.FileName);
                    List<int> mapLumpNums = archive.FindMaps();
                    if (mapLumpNums.Count == 0)
                        MessageBox.Show("Chosen WAD file does not have any maps present.");
                    else
                    {
                        OpenMapDialog mapDialog = new OpenMapDialog(archive, mapLumpNums);
                        //TODO: Organize better and add some functions for common tasks
                        if (mapDialog.ShowDialog() == DialogResult.OK)
                        {
                            archive.OpenFile();
                            byte[] data = archive.LoadLumpByIndex(mapDialog.CurrentMapIndex + 1);
                            archive.CloseFile();
                            if (editorState.CurrentLevel != null)
                            {
                                editorState.CloseLevel();
                                renderer.Textures.DestroyAtlas();
                            }
                            if (editorState.CreateLevelFromData(mapDialog.CurrentMap, data))
                                LevelChanged();
                        }
                        mapDialog.Dispose();
                    }
                }
            }
        }

        private void DoQuickSave()
        {
            if (editorState.EditorIOState.HasSavedBefore)
            {
                DoSave("", true);
            }
            else
            {
                DoSaveDialog(true);
            }
        }

        /// <summary>
        /// Spawns the save file dialog to save the current map
        /// </summary>
        /// <param name="saveinto">Whether or not the map should be inserted into the wad, instead of destroying the wad beforehand</param>
        private void DoSaveDialog(bool saveinto)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    DoSave(saveFileDialog1.FileName, saveinto);
                }
            }
        }

        private void DoSave(string filename, bool saveinto)
        {
            string fixFilename = filename.Replace('/', '\\');
            //TODO: move filename data
            if (editorState.SaveMapToFile(fixFilename, saveinto))
            {
                RebuildResources();
                UpdateWindowTitle();
                worldRenderer.LevelChanged();
            }
        }

        private void SelectTool(int toolID)
        {
            for (int i = 0; i < 9; i++)
            {
                tbToolPanel.Buttons[i + 4].Pushed = false;
            }
            tbToolPanel.Buttons[toolID + 4].Pushed = true;

            this.gbThingSelect.Visible = false;
            this.gbTileSelection.Visible = false;
            this.gbTriggerData.Visible = false;
            this.gbZoneList.Visible = false;
            this.gbSectorPanel.Visible = false;
            this.gbTag.Visible = false;

            switch (toolID)
            {
                case 1:
                    gbTileSelection.Visible = true;
                    gbTileSelection.SetPalette(editorState.TileList.tileset);
                    gbTileSelection.SetPairedBrush(editorState.BrushList[toolID]);
                    worldRenderer.showGrid = true;
                    break;
                case 2:
                    gbTileSelection.Visible = true;
                    gbTileSelection.SetPalette(editorState.TileList.tileset);
                    gbTileSelection.SetPairedBrush(editorState.BrushList[toolID]);
                    worldRenderer.showGrid = true;
                    break;
                case 4:
                    gbThingSelect.Visible = true;
                    gbThingSelect.AddThings(editorState.ThingList.thingList);
                    gbThingSelect.SetPairedBrush((ThingBrush)editorState.BrushList[toolID]);
                    worldRenderer.showGrid = true;
                    break;
                case 5:
                    gbTriggerData.Visible = true;
                    gbTriggerData.SetPairedBrush((TriggerBrush)editorState.BrushList[toolID]);
                    gbTriggerData.SetTriggerList(editorState.TriggerList);
                    worldRenderer.showGrid = true;
                    break;
                case 6:
                    gbSectorPanel.Visible = true;
                    gbSectorPanel.pairedBrush = (SectorBrush)editorState.BrushList[toolID];
                    worldRenderer.showGrid = true;
                    break;
                case 7:
                    gbZoneList.Visible = true;
                    gbZoneList.SetPairedBrush((FloodBrush)editorState.BrushList[toolID]);
                    worldRenderer.showGrid = false;
                    break;
                case 8:
                    gbTag.Visible = true;
                    gbTag.SetPairedBrush((TagTool)editorState.BrushList[toolID]);
                    worldRenderer.showGrid = true;
                    break;
            }
            editorState.SetBrush(toolid);
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button.Tag != null)
            {
                oldtoolid = toolid;
                int ltag = Int32.Parse((String)e.Button.Tag);
                if (ltag >= 100)
                {
                    SetViewMode(ltag - 100);
                }
                else if (ltag < 20)
                {
                    toolid = ltag;
                    SelectTool(toolid);
                }
                if (ltag == 22)
                {
                    DoQuickSave();
                }
                if (ltag == 21)
                {
                    DoLoadDialog();
                }
                if (ltag == 20)
                {
                    DoNewMapDialog();
                }
            }
        }

        private void SetupViewport()
        {
            GL.Viewport(0, 0, mainLevelPanel.Width, mainLevelPanel.Height); // Use all of the glControl painting area
        }

        private void SetViewMode(int mode)
        {
            worldRenderer.SetViewMode(mode);
            if (editorState.CurrentLevel != null)
                editorState.CurrentLevel.Invalidate();

            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_Load(object sender, EventArgs e)
        {
            ready = true;
            SetupViewport();
        }

        private void mainLevelPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!ready)
                return;

            mainLevelPanel.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (editorState.CurrentLevel != null)
            {
                worldRenderer.DrawLevel();
            }
            GL.Flush();
            mainLevelPanel.SwapBuffers();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (ready)
            {
                SetupViewport();
                renderer.SetViewSize(mainLevelPanel.Width, mainLevelPanel.Height);
                mainLevelPanel.Invalidate();
            }
            if (statusBar1.Width > 0)
                statusBar1.Panels[0].Width = statusBar1.Width - statusBar1.Panels[1].Width;
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (ready)
            {
                mainLevelPanel.Invalidate();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Handle UI events first
            if (e.KeyCode == Keys.Down)
            {
                pan.Y -= 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            else if (e.KeyCode == Keys.Up)
            {
                pan.Y += 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            else if (e.KeyCode == Keys.Left)
            {
                pan.X += 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            else if (e.KeyCode == Keys.Right)
            {
                pan.X -= 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }
            //Pass events to the editor state
            else
            {
                InputEvent ev = new InputEvent();
                ev.keycode = e.KeyCode;
                ev.down = true;
                if (editorState.HandleInputEvent(ev))
                    mainLevelPanel.Invalidate();
            }
        }
        private void mainLevelPanel_KeyUp(object sender, KeyEventArgs e)
        {
            InputEvent ev = new InputEvent();
            ev.keycode = e.KeyCode;
            ev.down = false;
            if (editorState.HandleInputEvent(ev))
                mainLevelPanel.Invalidate();
        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Console.WriteLine("mouse input");
        }

        //stuff swiped from DB2 to prevent changing control when using keyboard interface to move around
        protected override bool IsInputChar(char charCode) { return false; }
        protected override bool IsInputKey(Keys keyData) { return false; }
        protected override bool ProcessKeyPreview(ref Message m) { return false; }
        protected override bool ProcessDialogKey(Keys keyData) { return false; }
        //[ISB] actually needed for accelerators? Hopefully it isn't a problem
        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData) { return false; }

        private void UpdateZoom()
        {
            statusBar1.Panels[1].Text = String.Format("Zoom: {0:P}", zoom);
            renderer.SetZoom(zoom, (int)lastMousePos.X, (int)lastMousePos.Y);

            mainLevelPanel.Invalidate();
        }

        private void SetMouseButton(System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    this.heldMouseButton = 0;
                    break;
                case MouseButtons.Right:
                    this.heldMouseButton = 1;
                    break;
                case MouseButtons.Middle:
                    this.heldMouseButton = 2;
                    break;
            }
        }

        private void mainLevelPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            if ((e.Button & (MouseButtons.Left | MouseButtons.Right)) != 0)
            {
                PickResult pickTest = renderer.Pick(e.X, e.Y);
                SetMouseButton(e);
                brushmode = editorState.BrushDown(pickTest, heldMouseButton);
            }

            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            PickResult pickTest = renderer.Pick(e.X, e.Y);

            if (brushmode)
            {
                PickResult src = renderer.Pick((int)lastMousePos.X, (int)lastMousePos.Y);
                editorState.BrushFromTo(src, pickTest, heldMouseButton);
            }
            else if ((e.Button & MouseButtons.Middle) != 0) //middle click to pan
            {
                renderer.AddPan(e.X - (int)lastMousePos.X, e.Y - (int)lastMousePos.Y);
            }

            editorState.HandlePick(pickTest);
            if (editorState.CurrentTool == CurrentToolNum.ZoneTool)
            {
                gbZoneList.UpdateZoneHighlight(editorState.HighlightedZone);
            }
            mainLevelPanel.Invalidate();

            lastMousePos.X = e.X;
            lastMousePos.Y = e.Y;
        }

        private void mainLevelPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            brushmode = false;
            editorState.BrushEnd();
            this.UpdateCurrentZoneList();
        }

        private void MainLevelPanel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int numClicks = Math.Abs(e.Delta / 120);
            if (e.Delta < 0)
            {
                zoom *= (0.9f * numClicks);
                UpdateZoom();
            }
            else if (e.Delta > 0)
            {
                zoom *= (1.11111111111f * numClicks);
                UpdateZoom();
            }
        }

        private void mainLevelPanel_MouseEnter(object sender, EventArgs e)
        {
            //The GL control should be where any hotkeys are passed, so focus immediately when hovered over
            mainLevelPanel.Focus();
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            DoNewMapDialog();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            DoLoadDialog();
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            DoQuickSave();
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            //save as, delete all contents of the destination WAD
            DoSaveDialog(false);
        }

        private void SaveIntoMenuItem_Click(object sender, EventArgs e)
        {
            //save as, preserve all contents of the destination WAD
            DoSaveDialog(true);
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            //this was needed for an experiment, but it turned out not to be needed
        }
    }
}
