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

using eced.UIPanels;

namespace eced
{
    public partial class Form1 : Form
    {
        private bool lockangle = false;

        private TileManager tilelist;
        private ThingManager thinglist;
        private Tile selectedTile;

        //brushes
        Brush defaultBrush = new Brush();
        ThingBrush thingBrush = new ThingBrush();
        TriggerBrush triggerBrush = new TriggerBrush();
        FloodBrush zoneBrush = new FloodBrush();
        SectorBrush sectorBrush = new SectorBrush();
        TagTool tagBrush = new TagTool();
        private TriggerTypeList triggerlist = new TriggerTypeList();

        private int toolid = 1, oldtoolid = 1;
        private MapInformation currentMapinfo;
        private Level currentLevel;// = new Level(64, 64, 1);
        private GraphicsManager renderer = new GraphicsManager();
        private bool ready = false;

        //private int panx = 0, pany = 0;
        private Vector2 pan;

        private float zoom = 1.0f;

        private bool brushmode = false;
        private int heldMouseButton = 0;
        private bool locked = false;

        private int VAOid;
        //int program;

        private TextureManager tm = new TextureManager();
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
            UpdateZoom();
            tilelist = new TileManager("./resources/wolftiles.xml");
            tilelist.LoadPalette();
            thinglist = new ThingManager();
            thinglist.LoadThingDefintions("./resources/wolfactors.xml");

            gbThingSelect.AddThings(thinglist.thingList);

            tbToolPanel.Buttons[5].Pushed = true;
            this.gbThingSelect.Visible = false;
            this.gbTileSelection.Visible = true;
            this.gbTriggerData.Visible = false;
            this.gbZoneList.Visible = false;
            this.gbSectorPanel.Visible = false;
            this.gbTag.Visible = false;

            this.thingBrush.thing = thinglist.thingList[0];
            this.thingBrush.thinglist = thinglist;
            VAOid = GL.GenVertexArray();
            GL.BindVertexArray(VAOid);

            glInit();

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
                if (this.currentLevel != null)
                {
                    CloseLevel();
                    tm.cleanup();
                }
                this.CreateNewLevel(nmd.CurrentMap);
            }

            nmd.Dispose();
        }

        private void LoadResources(MapInformation mapinfo, Level level)
        {
            tm.allocateAtlasTexture();
            tm.readyAtlasCreation();
            for (int i = 0; i < mapinfo.files.Count; i++)
            {
                ResourceFiles.ResourceArchive file;
                if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.FORMAT_WAD)
                {
                    string fixfilename = mapinfo.files[i].filename;
                    //replace all slashes with backslashes to prevent issues
                    fixfilename = fixfilename.Replace('/', '\\');
                    file = ResourceFiles.WADResourceFile.loadResourceFile(mapinfo.files[i].filename);
                    file.openFile();
                    tm.getTextureList(file);
                    level.loadedResources.Add(file);
                    file.closeFile();
                }
                else if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.FORMAT_ZIP)
                {
                    file = ResourceFiles.ZIPResourceFile.loadResourceFile(mapinfo.files[i].filename);
                    file.openFile();
                    tm.getTextureList(file);
                    level.loadedResources.Add(file);
                    file.closeFile();
                }
            }
            tm.createInfoTexture();
            tm.uploadNumberTexture();

            level.tm = this.tm;

            renderer.setupTextures(level, tm.resourceInfoID, tm.atlasTextureID, tm.numberTextureID);
            this.currentMapinfo = mapinfo;
        }

        private void CreateNewLevel(MapInformation mapinfo)
        {
            Level level = new Level(mapinfo.sizex, mapinfo.sizey, mapinfo.layers, tilelist.tileset[0]);
            level.localThingList = this.thinglist;
            this.selectedTile = tilelist.tileset[0];

            LoadResources(mapinfo, level);
            
            currentLevel = level;

            renderer.setupLevelRendering(currentLevel, (uint)sm.programList["WorldRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
            renderer.setupThingUniforms(sm.programList["ThingRender"]);

            this.UpdateCurrentZoneList();
            gbTileSelection.SetPalette(tilelist.tileset);
        }

        private void CloseLevel()
        {
            //arc.closeResource();
            currentLevel.DisposeLevel();
        }

        private void UpdateCurrentZoneList()
        {
            List<Zone> zonelist = currentLevel.GetZone();
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
                    SaveMapToFile(fixFilename, saveinto);
                }
            }
        }

        /// <summary>
        /// Saves a map a specified WAD file
        /// <param name="filename">Filename to save to</param>
        /// <param name="saveinto">Makes the save code put the WAD onto the resource stack before saving</param>
        /// </summary>
        private void SaveMapToFile(string filename, bool saveinto)
        {
            //TODO: Temp
            bool destArchiveLoaded = this.currentMapinfo.ContainsResource(filename);

            //init it off the bat because Visual Studio is being lovely about detecting whether or not it was loaded
            ResourceFiles.WADResourceFile savearchive = new ResourceFiles.WADResourceFile();

            //make sure the save into archive actually exists
            if (saveinto && !System.IO.File.Exists(filename))
            {
                //if it doesn't turn off save into
                saveinto = false;
            }

            if (destArchiveLoaded)
            {
                //find the resource file for this map
                bool found = false;
                foreach (ResourceFiles.ResourceArchive file in currentLevel.loadedResources)
                {
                    if (file.archiveName.Equals(filename, StringComparison.OrdinalIgnoreCase))
                    {
                        savearchive = (ResourceFiles.WADResourceFile)file;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("The archive for the current save desination is not loaded");
                }
            }
            else
            {
                //add the resource onto the stack before doing anything
                if (saveinto)
                {
                    savearchive = (ResourceFiles.WADResourceFile)ResourceFiles.WADResourceFile.loadResourceFile(filename);
                    this.currentLevel.loadedResources.Add(savearchive);
                    ResourceFiles.ResourceArchiveHeader lhead = new ResourceFiles.ResourceArchiveHeader();
                    lhead.filename = filename;
                    this.currentMapinfo.files.Add(lhead);
                    destArchiveLoaded = true;
                }
            }

            //Do some special things to save the map special lumps
            if (destArchiveLoaded)
            {
                List<int> idlist = savearchive.findSpecialMapLumps(this.currentMapinfo.lumpname);

                //TODO: actually preserve special lumps

                //Delete the current version of the map out of the archive
                savearchive.deleteMap(currentMapinfo.lumpname);
            }

            string mapstring = currentLevel.Serialize();

            //Console.WriteLine(mapstring);

            //Get an ascii representation of the map
            byte[] mapdata = Encoding.ASCII.GetBytes(mapstring);

            //Console.WriteLine(mapdata.Length);

            List<ResourceFiles.ResourceFile> lumps = new List<ResourceFiles.ResourceFile>();

            ResourceFiles.ResourceFile mapheader = new ResourceFiles.ResourceFile(this.currentMapinfo.lumpname, ResourceFiles.ResourceType.RES_GENERIC, 0);
            mapheader.pointer = 0; lumps.Add(mapheader);
            ResourceFiles.ResourceFile mapdatal = new ResourceFiles.ResourceFile("TEXTMAP", ResourceFiles.ResourceType.RES_GENERIC, mapdata.Length);
            mapdatal.pointer = 0; lumps.Add(mapdatal);
            ResourceFiles.ResourceFile mapend = new ResourceFiles.ResourceFile("ENDMAP", ResourceFiles.ResourceType.RES_GENERIC, 0);
            mapend.pointer = 0; lumps.Add(mapend);


            savearchive.updateToNewWad(filename, ref lumps, ref mapdata, destArchiveLoaded);

            if (!destArchiveLoaded)
            {
                //hock the new map onto the resource list
                ResourceFiles.ResourceArchiveHeader head = new ResourceFiles.ResourceArchiveHeader();
                head.filename = filename;
                head.format = ResourceFiles.ResourceFormat.FORMAT_WAD;

                this.currentMapinfo.files.Add(head);
            }

            //trash all the old resources to be sure we're up to date
            currentLevel.DisposeLevel();
            tm.cleanup();
            LoadResources(currentMapinfo, currentLevel);
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
                    tbToolPanel.Buttons[oldtoolid+4].Pushed = false;
                    tbToolPanel.Buttons[toolid+4].Pushed = true;

                    this.gbThingSelect.Visible = false;
                    this.gbTileSelection.Visible = false;
                    this.gbTriggerData.Visible = false;
                    this.gbZoneList.Visible = false;
                    this.gbSectorPanel.Visible = false;
                    this.gbTag.Visible = false;

                    switch (toolid)
                    {
                        case 1:
                            this.defaultBrush = new Brush();
                            this.gbTileSelection.Visible = true;
                            break;
                        case 2:
                            this.defaultBrush = new TileBrush();
                            this.gbTileSelection.Visible = true;
                            break;
                        case 4:
                            this.defaultBrush = thingBrush;
                            this.gbThingSelect.Visible = true;
                            break;
                        case 5:
                            this.defaultBrush = triggerBrush;
                            this.gbTriggerData.Visible = true;
                            break;
                        case 6:
                            this.defaultBrush = sectorBrush;
                            this.gbSectorPanel.Visible = true;
                            break;
                        case 7:
                            this.defaultBrush = zoneBrush;
                            this.gbZoneList.Visible = true;
                            break;
                        case 8:
                            this.defaultBrush = tagBrush;
                            this.gbTag.Visible = true;
                            break;
                    }
                }

                if (ltag == 22)
                {
                    if (this.currentFilename != "")
                    {
                        SaveMapToFile(this.currentFilename, true);
                    }
                    else
                    {
                        DoSaveDialog(true);
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
            if (currentLevel != null)
            {
                //renderer.renderLevel(currentLevel);
                renderer.updateWorldTexture(currentLevel);
                renderer.drawLevel(currentLevel, (uint)sm.programList["WorldRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                renderer.drawGrid(currentLevel, sm.programList["BasicRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));

                ErrorCode error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine("DRAW GL Error: {0}", error.ToString());
                }

                GL.UseProgram(sm.programList["ThingRender"]);
                List<Thing> thinglist = currentLevel.GetThings();
                for (int i = 0; i < thinglist.Count; i++)
                {
                    Thing thing = thinglist[i];

                    renderer.drawThing(thing, currentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                }
                List<OpenTK.Vector2> triggerList = currentLevel.GetTriggerLocations();

                for (int i = 0; i < triggerList.Count; i++)
                {
                    renderer.drawTrigger(triggerList[i], currentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                }

                //renderer.drawTrigger(new Vector2(2.0f, 2.0f), currentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                GL.UseProgram(0);
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
                pan.Y -= .05f;
                renderer.setpan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Up)
            {
                pan.Y += .05f; 
                renderer.setpan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Left)
            {
                pan.X += .05f;
                renderer.setpan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Right)
            {
                pan.X -= .05f;
                renderer.setpan(pan);
                mainLevelPanel.Invalidate();
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (this.currentLevel.highlighted != null)
                {
                    this.currentLevel.DeleteThing(this.currentLevel.highlighted);
                    this.currentLevel.highlighted = null;
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
            renderer.zoom = zoom;

            mainLevelPanel.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //zoom += .25f;
            zoom *= 2.0f;
            UpdateZoom();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //zoom -= .25f;
            zoom *= 0.5f;
            if (zoom < .25f)
                zoom = .25f;
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
            float sizex = currentLevel.width / 2f;
            float sizey = currentLevel.height / 2f;
            Vector2 center = new Vector2(mainLevelPanel.Width / 2, mainLevelPanel.Height / 2);
            Vector2 bstart = new Vector2(center.X - (sizex * 8 * zoom) + (pan.X * 64 * 8 * zoom), center.Y - (sizey * 8 * zoom) + (pan.Y * 64 * 8 * zoom));
            Vector2 curpos = new Vector2(mouseCoords.X - bstart.X, mouseCoords.Y - bstart.Y);
            Vector2 tile = new Vector2((curpos.X / (8 * zoom)), (curpos.Y / (8 * zoom)));

            return tile;
        }

        private void mainLevelPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (currentLevel == null)
                return;

            Console.WriteLine("placing brush");
            
            defaultBrush.normalTile = selectedTile;

            SetMouseButton(e);
            defaultBrush.ApplyToTile(Pick(new Vector2(e.X, e.Y)), 0, this.currentLevel, this.heldMouseButton);
            if (defaultBrush.repeatable)
            {
                brushmode = true;
            }
            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (currentLevel == null)
                return;

            Vector2 tile = Pick(new Vector2(e.X, e.Y));

            //Console.WriteLine("{0} {1}, center {2} {3}", tile.X, tile.Y, pan.X, pan.Y);

            currentLevel.UpdateHighlight((int)((tile.X + .5) * 64), (int)((tile.Y + .5) * 64));

            if (brushmode && defaultBrush.repeatable)
            {
                //defaultBrush.ApplyToTile(pick(new Vector2(e.X, e.Y)), 0, this.currentLevel, this.heldMouseButton);
                Vector2 src = Pick(lastMousePos);
                LineDrawer.applyBrushOverLine(src, tile, ref this.currentLevel, heldMouseButton, this.defaultBrush);
            }

            if (defaultBrush is TriggerBrush)
            {
                //currentLevel.updateTriggerHighlight(tilex / zoom, tiley / zoom, 0);
            }
            mainLevelPanel.Invalidate();

            lastMousePos.X = e.X;
            lastMousePos.Y = e.Y;

            if (tile.X >= 0 && tile.Y >= 0 && tile.X < currentLevel.width && tile.Y < currentLevel.height)
            {
                Cell cell = currentLevel.GetCell((int)tile.X, (int)tile.Y, 0);
                statusBarPanel1.Text = String.Format("tileid: {0}, zonenum: {1}, tag: {2}", currentLevel.GetTileID(cell.tile), currentLevel.GetZoneID(cell), cell.tag);
            }
            else
            {
                statusBarPanel1.Text = "-";
            }
        }

        private void mainLevelPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (currentLevel == null)
                return;

            Console.WriteLine("lifting brush");

            brushmode = false;
            defaultBrush.EndBrush(currentLevel);
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

        private void thingBitChange(object sender, EventArgs e)
        {
            /*thingBrush.flags.ambush = this.cbThingAmbush.Checked;
            thingBrush.flags.patrol = this.cbThingPatrol.Checked;
            thingBrush.flags.skill1 = this.cbThingSkill1.Checked;
            thingBrush.flags.skill2 = this.cbThingSkill2.Checked;
            thingBrush.flags.skill3 = this.cbThingSkill3.Checked;
            thingBrush.flags.skill4 = this.cbThingSkill4.Checked;*/
        }

        private void ndThingAngle_ValueChanged(object sender, EventArgs e)
        {
            /*if (this.ndThingAngle.Value == 360)
                this.ndThingAngle.Value = 0;

            thingBrush.flags.angle = (int)this.ndThingAngle.Value;

            this.lockangle = true;

            if (thingBrush.flags.angle == 0)
                this.rbThingEast.Checked = true;
            else this.rbThingEast.Checked = false;

            if (thingBrush.flags.angle == 90)
                this.rbThingNorth.Checked = true;
            else this.rbThingNorth.Checked = false;

            if (thingBrush.flags.angle == 180)
                this.rbThingWest.Checked = true;
            else this.rbThingWest.Checked = false;

            if (thingBrush.flags.angle == 270)
                this.rbThingSouth.Checked = true;
            else this.rbThingSouth.Checked = false;

            if (thingBrush.flags.angle == 45)
                this.rbThingNE.Checked = true;
            else this.rbThingNE.Checked = false;

            if (thingBrush.flags.angle == 135)
                this.rbThingNW.Checked = true;
            else this.rbThingNW.Checked = false;

            if (thingBrush.flags.angle == 225)
                this.rbThingSW.Checked = true;
            else this.rbThingSW.Checked = false;

            if (thingBrush.flags.angle == 315)
                this.rbThingSE.Checked = true;
            else this.rbThingSE.Checked = false;
            
            lockangle = false;*/
        }

        private void rbThingEast_CheckedChanged(object sender, EventArgs e)
        {
            if (lockangle) return;
            /*if (rbThingEast.Checked)
                this.ndThingAngle.Value = 0;
            if (rbThingNorth.Checked)
                this.ndThingAngle.Value = 90;
            if (rbThingWest.Checked)
                this.ndThingAngle.Value = 180;
            if (rbThingSouth.Checked)
                this.ndThingAngle.Value = 270;

            if (rbThingNE.Checked)
                this.ndThingAngle.Value = 45;
            if (rbThingNW.Checked)
                this.ndThingAngle.Value = 135;
            if (rbThingSW.Checked)
                this.ndThingAngle.Value = 225;
            if (rbThingSE.Checked)
                this.ndThingAngle.Value = 315;*/
        }

        private void lbZoneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.zoneBrush.setCode = lbZoneList.SelectedIndex - 1;
        }

        private void createTile_Events(object sender, EventArgs e)
        {
            if (locked) return;
            /*Tile newTile = new Tile();

            newTile.NorthTex = tbNorthTex.Text;
            newTile.SouthTex = tbSouthTex.Text;
            newTile.EastTex = tbEastTex.Text;
            newTile.WestTex = tbWestTex.Text;

            newTile.NorthBlock = cbTileNorth.Checked;
            newTile.SouthBlock = cbTileSouth.Checked;
            newTile.EastBlock = cbTileEast.Checked;
            newTile.WestBlock = cbTileWest.Checked;

            newTile.HorizOffset = cbCenterHoriz.Checked;
            newTile.VerticalOffset = cbCenterVert.Checked;

            this.selectedTile = newTile;*/
        }

        private void tbFloorTex_TextChanged(object sender, EventArgs e)
        {
            Sector newSector = new Sector();
            //newSector.texceil = tbCeilingTex.Text;
            //newSector.texfloor = tbFloorTex.Text;

            this.sectorBrush.currentSector = newSector;
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
                SaveMapToFile(this.currentFilename, true);
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
