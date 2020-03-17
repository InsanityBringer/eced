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

        //private int panx = 0, pany = 0;
        private Vector2 pan;

        private float zoom = 1.0f;

        private bool brushmode = false;
        private int heldMouseButton = 0;

        private int VAOid;
        //int program;

        private ShaderManager sm = new ShaderManager();

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

            tbToolPanel.Buttons[5].Pushed = true;
            this.gbThingSelect.Visible = false;
            this.gbTileSelection.Visible = true;
            this.gbTriggerData.Visible = false;
            this.gbZoneList.Visible = false;
            this.gbSectorPanel.Visible = false;
            this.gbTag.Visible = false;

            VAOid = GL.GenVertexArray();
            GL.BindVertexArray(VAOid);

            renderer = new RendererState(editorState);
            renderer.Init();
            renderer.SetViewSize(mainLevelPanel.Width, mainLevelPanel.Height);
            glInit();
            UpdateZoom();
            worldRenderer = new WorldRenderer(renderer);

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP GL Error: {0}", error.ToString());
            }

            GL.Disable(EnableCap.DepthTest);
        }

        private void glInit()
        {
            sm.makeProgram("./resources/VertexPanTexture.txt", "./resources/FragPanTextureAtlas.txt", "WorldRender");
            sm.makeProgram("./resources/VertexPanThing.txt", "./resources/FragPanThing.txt", "ThingRender");
            sm.makeProgram("./resources/VertexPanBasic.txt", "./resources/FragPanThing.txt", "BasicRender");
            renderer.setupThingRendering();
            renderer.setupTriggerRendering();
            renderer.setupLineRendering();
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
                    renderer.Textures.cleanup();
                }
                editorState.CreateNewLevel(nmd.CurrentMap);
                //renderer.setupLevelRendering(editorState.CurrentLevel, (uint)sm.programList["WorldRender"], new Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                //renderer.setupThingUniforms(sm.programList["ThingRender"]);
                RebuildResources();
                SelectTool(1);
                worldRenderer.LevelChanged();
            }

            nmd.Dispose();
        }

        private void RebuildResources()
        {
            renderer.Textures.cleanup();
            renderer.Textures.allocateAtlasTexture();
            renderer.Textures.readyAtlasCreation();
            for (int i = 0; i < editorState.CurrentLevel.loadedResources.Count; i++)
            {
                ResourceFiles.ResourceArchive file = editorState.CurrentLevel.loadedResources[i];
                file.openFile();
                renderer.Textures.getTextureList(file);
                file.closeFile();
            }
            renderer.Textures.createInfoTexture();
            renderer.Textures.uploadNumberTexture();

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
                    this.gbTileSelection.Visible = true;
                    gbTileSelection.pairedBrush = editorState.BrushList[toolID];
                    gbTileSelection.SetPalette(editorState.TileList.tileset);
                    break;
                case 2:
                    this.gbTileSelection.Visible = true;
                    gbTileSelection.pairedBrush = editorState.BrushList[toolID];
                    gbTileSelection.SetPalette(editorState.TileList.tileset);
                    break;
                case 4:
                    this.gbThingSelect.Visible = true;
                    gbThingSelect.AddThings(editorState.ThingList.thingList);
                    gbThingSelect.pairedBrush = (ThingBrush)editorState.BrushList[toolID];
                    break;
                case 5:
                    this.gbTriggerData.Visible = true;
                    break;
                case 6:
                    this.gbSectorPanel.Visible = true;
                    break;
                case 7:
                    this.gbZoneList.Visible = true;
                    break;
                case 8:
                    this.gbTag.Visible = true;
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
                if (ltag < 20)
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
                //renderer.renderLevel(currentLevel);
                /*renderer.updateWorldTexture(editorState.CurrentLevel);
                renderer.drawLevel(editorState.CurrentLevel, (uint)sm.programList["WorldRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                renderer.drawGrid(editorState.CurrentLevel, sm.programList["BasicRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));

                ErrorCode error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine("DRAW GL Error: {0}", error.ToString());
                }

                GL.UseProgram(sm.programList["ThingRender"]);
                List<Thing> thinglist = editorState.CurrentLevel.Things;
                for (int i = 0; i < thinglist.Count; i++)
                {
                    Thing thing = thinglist[i];

                    renderer.drawThing(thing, editorState.CurrentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                }
                List<OpenTK.Vector2> triggerList = editorState.CurrentLevel.GetTriggerLocations();

                for (int i = 0; i < triggerList.Count; i++)
                {
                    renderer.drawTrigger(triggerList[i], editorState.CurrentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                }

                //renderer.drawTrigger(new Vector2(2.0f, 2.0f), currentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                GL.UseProgram(0);*/
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
                //renderer.pan(panx, pany, mainLevelPanel.Width, mainLevelPanel.Height);
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
                if (editorState.CurrentLevel.highlighted != null)
                {
                    editorState.CurrentLevel.DeleteThing(editorState.CurrentLevel.highlighted);
                    editorState.CurrentLevel.highlighted = null;
                    mainLevelPanel.Invalidate();
                }
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
            renderer.SetZoom(zoom);
            //renderer.zoom = zoom;

            mainLevelPanel.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //zoom += .25f;
            zoom *= 1.111111111111f;
            UpdateZoom();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //zoom -= .25f;
            zoom *= 0.9f;
            UpdateZoom();
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

        public Vector2 Pick(Vector2 mouseCoords)
        {
            float sizex = editorState.CurrentLevel.Width / 2f;
            float sizey = editorState.CurrentLevel.Height / 2f;
            Vector2 center = new Vector2(mainLevelPanel.Width / 2, mainLevelPanel.Height / 2);
            Vector2 bstart = new Vector2(center.X - (sizex * 8 * zoom) + (pan.X * 64 * 8 * zoom), center.Y - (sizey * 8 * zoom) + (pan.Y * 64 * 8 * zoom));
            Vector2 curpos = new Vector2(mouseCoords.X - bstart.X, mouseCoords.Y - bstart.Y);
            Vector2 tile = new Vector2((curpos.X / (8 * zoom)), (curpos.Y / (8 * zoom)));

            return tile;
        }

        private void mainLevelPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            Console.WriteLine("placing brush");
            Vector2 tile = Pick(new Vector2(e.X, e.Y));

            SetMouseButton(e);
            brushmode = editorState.BrushDown(tile, heldMouseButton);
            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (editorState.CurrentLevel == null)
                return;

            Vector2 tile = Pick(new Vector2(e.X, e.Y));

            editorState.CurrentLevel.UpdateHighlight((int)((tile.X + .5) * 64), (int)((tile.Y + .5) * 64));

            if (brushmode)
            {
                Vector2 src = Pick(lastMousePos);
                editorState.BrushFromTo(src, tile, heldMouseButton);
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //thingBrush.thing = thinglist.thinglist[thinglist.idlist[listBox1.SelectedIndex]];
        }

        private void TriggerFlagChange(object sender, EventArgs e)
        {
            /*triggerBrush.trigger.acte = cbTrigEast.Checked;
            //triggerBrush.trigger.action = cbTriggerType.SelectedIndex;
            triggerBrush.trigger.actn = cbTrigNorth.Checked;
            triggerBrush.trigger.acts = cbTrigSouth.Checked;
            triggerBrush.trigger.actw = cbTrigWest.Checked;
            triggerBrush.trigger.cross = cbCross.Checked;
            triggerBrush.trigger.repeat = cbRepeat.Checked;
            triggerBrush.trigger.secret = cbSecret.Checked;
            triggerBrush.trigger.usemonster = cbUseMonst.Checked;
            triggerBrush.trigger.useplayer = cbUse.Checked;*/
        }

        private void triggerParamChange(object sender, EventArgs e)
        {
            /*triggerBrush.trigger.arg0 = (int)ndParam1.Value;
            triggerBrush.trigger.arg1 = (int)ndParam2.Value;
            triggerBrush.trigger.arg2 = (int)ndParam3.Value;
            triggerBrush.trigger.arg3 = (int)ndParam4.Value;
            triggerBrush.trigger.arg4 = (int)ndParam5.Value;*/
        }

        private void cbTriggerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //triggerBrush.trigger.action = cbTriggerType.SelectedIndex+1;
        }

        private void lbZoneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.zoneBrush.setCode = lbZoneList.SelectedIndex - 1;
        }

        private void tbFloorTex_TextChanged(object sender, EventArgs e)
        {
            Sector newSector = new Sector();
            //newSector.texceil = tbCeilingTex.Text;
            //newSector.texfloor = tbFloorTex.Text;

        }

        private void nudNewTag_ValueChanged(object sender, EventArgs e)
        {
            //this.tagBrush.tag = (int)nudNewTag.Value;
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

        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.DoNewMapDialog();
        }
    }
}
