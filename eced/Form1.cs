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

        //Empty when no current filename
        private string currentFilename = "";

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

            gbZoneList = new ZoneUIPanel();
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
            //statusBar1.Panels[1].Text =

            //tbToolPanel.Buttons[5].Pushed = true;
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
                RebuildResources();
                SelectTool(1);
                worldRenderer.LevelChanged();
            }

            nmd.Dispose();
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

        private void menuItem5_Click(object sender, EventArgs e)
        {
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
                    string fixFilename = saveFileDialog1.FileName.Replace('/', '\\');
                    //TODO: move filename data
                    editorState.SaveMapToFile(fixFilename, saveinto);
                }
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
                    break;
                case 2:
                    gbTileSelection.Visible = true;
                    gbTileSelection.SetPalette(editorState.TileList.tileset);
                    gbTileSelection.SetPairedBrush(editorState.BrushList[toolID]);
                    break;
                case 4:
                    gbThingSelect.Visible = true;
                    gbThingSelect.AddThings(editorState.ThingList.thingList);
                    gbThingSelect.SetPairedBrush((ThingBrush)editorState.BrushList[toolID]);
                    break;
                case 5:
                    gbTriggerData.Visible = true;
                    gbTriggerData.SetPairedBrush((TriggerBrush)editorState.BrushList[toolID]);
                    gbTriggerData.SetTriggerList(editorState.TriggerList);
                    break;
                case 6:
                    gbSectorPanel.Visible = true;
                    gbSectorPanel.pairedBrush = (SectorBrush)editorState.BrushList[toolID];
                    break;
                case 7:
                    gbZoneList.Visible = true;
                    gbZoneList.SetPairedBrush((FloodBrush)editorState.BrushList[toolID]);
                    break;
                case 8:
                    gbTag.Visible = true;
                    gbTag.SetPairedBrush((TagTool)editorState.BrushList[toolID]);
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
                    if (this.currentFilename != "")
                    {
                        editorState.SaveMapToFile(this.currentFilename, true);
                        RebuildResources();
                    }
                    else
                    {
                        DoSaveDialog(true);
                        RebuildResources();
                    }
                }

                if (ltag == 21)
                {
                    /*
                    //TODO: Absolute path
                    CodeImp.DoomBuilder.IO.UniversalParser parser = new CodeImp.DoomBuilder.IO.UniversalParser("c:/dev/textmap.txt");

                    Console.WriteLine("Errors: {0}, line {1}", parser.ErrorDescription, parser.ErrorLine);
                    for (int x = 0; x < parser.Root.Count; x++)
                    {
                        Console.WriteLine("Found key {0}", parser.Root[x].Key);
                    }

                    //reconstruct the level
                    if (parser.ErrorDescription == "")
                    {
                        //try
                        {
                            //TODO: Redundant code with level creation
                            closeLevel();
                            tm.cleanup();
                            //TODO: Absolute path
                            ResourceFiles.ResourceArchive arc = ResourceFiles.WADResourceFile.loadResourceFile("c:/games/ecwolf/sneswolf.wad");

                            Level newLevel = LevelIO.makeNewLevel(parser.Root);
                            newLevel.localThingList = this.thinglist;

                            tm.allocateAtlasTexture();
                            tm.readyAtlasCreation();
                            tm.getTextureList(arc);
                            tm.createInfoTexture();
                            tm.uploadNumberTexture();

                            newLevel.tm = this.tm;

                            renderer.setupTextures(newLevel, tm.resourceInfoID, tm.atlasTextureID, tm.numberTextureID);

                            currentLevel = newLevel;

                            renderer.setupLevelRendering(currentLevel, (uint)sm.programList["WorldRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                            renderer.setupThingUniforms(sm.programList["ThingRender"]);

                            this.updateZoneList();
                        }
                        catch (Exception exc)
                        {
                            statusBar1.Panels[0].Text = "Error loading map: " + exc.Message;
                            Console.WriteLine(exc.ToString());
                        }
                    }
                    Console.WriteLine("heh");*/
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
                worldRenderer.UpdateLevel();
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
            Console.WriteLine("input");
            if (e.KeyCode == Keys.Down)
            {
                pan.Y -= 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Up)
            {
                pan.Y += 32f; 
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Left)
            {
                pan.X += 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Right)
            {
                pan.X -= 32f;
                renderer.SetPan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Delete)
            {
                //TODO: move to EditorState somehow
                /*if (editorState.CurrentLevel.highlighted != null)
                {
                    editorState.CurrentLevel.DeleteThing(editorState.CurrentLevel.highlighted);
                    editorState.CurrentLevel.highlighted = null;
                    mainLevelPanel.Invalidate();
                }*/
            }
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) { return false; }

        private void UpdateZoom()
        {
            statusBar1.Panels[1].Text = String.Format("Zoom: {0:P}", zoom);
            renderer.SetZoom(zoom, (int)lastMousePos.X, (int)lastMousePos.Y);
            //renderer.zoom = zoom;

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
                brushmode = editorState.BrushDown(new Vector2(pickTest.x, pickTest.y), heldMouseButton);
            }

            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            PickResult pickTest = renderer.Pick(e.X, e.Y);
            //statusBar1.Panels[0].Text = string.Format("({0} {1} ({2} {3}))", pickTest.x, pickTest.y, pickTest.xf, pickTest.yf);
            editorState.UpdateHighlight(pickTest);

            if (brushmode)
            {
                PickResult src = renderer.Pick((int)lastMousePos.X, (int)lastMousePos.Y);
                editorState.BrushFromTo(new Vector2(src.x, src.y), new Vector2(pickTest.x, pickTest.y), heldMouseButton);
            }
            else if ((e.Button & MouseButtons.Middle) != 0) //middle click to pan
            {
                renderer.AddPan(e.X - (int)lastMousePos.X, e.Y - (int)lastMousePos.Y);
            }
            mainLevelPanel.Invalidate();

            lastMousePos.X = e.X;
            lastMousePos.Y = e.Y;
        }

        private void mainLevelPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            Console.WriteLine("lifting brush");

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

        private void menuItem6_Click(object sender, EventArgs e)
        {
            //update current wad if you just click save
            if (this.currentFilename != "")
            {
                editorState.SaveMapToFile(this.currentFilename, true);
                RebuildResources();
            }
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            //save as, delete all contents of the destination WAD
            DoSaveDialog(false);
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
            //save as, preserve all contents of the destination WAD
            DoSaveDialog(true);
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            //this was needed for an experiment, but it turned out not to be needed
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.DoNewMapDialog();
        }
    }
}
