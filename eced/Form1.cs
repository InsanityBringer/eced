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

namespace eced
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
        private Level currentLevel;// = new Level(64, 64, 1);
        private GraphicsManager renderer = new GraphicsManager();
        private bool ready = false;

        //private int panx = 0, pany = 0;
        private Vector2 pan;

        private float zoom = 1.0f;

        private bool brushmode = false;
        private int heldMouseButton = 0;
        private Bitmap tilelistimg;
        private bool locked = false;

        private int VAOid;
        //int program;

        private TextureManager tm = new TextureManager();
        private ShaderManager sm = new ShaderManager();

        private ResourceFiles.ResourceArchive arc;

        private OpenTK.Vector2 lastMousePos = new Vector2();

        private void Form1_Load(object sender, EventArgs e)
        {
            //statusBar1.Panels[1].Text =
            updateZoom();
            tilelist = new TileManager("./resources/wolftiles.xml");
            tilelist.loadTileset();
            thinglist = new ThingManager("./resources/wolfactors.xml");
            thinglist.processData();

            for (int x = 0; x < thinglist.idlist.Count; x++)
            {
                ThingDefinition thing = thinglist.thinglist[thinglist.idlist[x]];
                listBox1.Items.Add(thing.name);
            }

            /*Bitmap atlas = new Bitmap("./resources/sneswolftiles.PNG");
            tilelistimg = tilelist.fillOutTiles(atlas);
            atlas.Dispose();

            pbTileList.Image = tilelistimg;*/

            tbToolPanel.Buttons[5].Pushed = true;
            this.gbThingSelect.Visible = false;
            this.gbTileSelection.Visible = true;
            this.gbTriggerData.Visible = false;
            this.gbZoneList.Visible = false;
            this.gbSectorPanel.Visible = false;
            this.gbTag.Visible = false;

            this.thingBrush.thing = thinglist.thinglist[1];
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

            //createNewLevel(null); //heh
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

        private void doNewMapDialog()
        {
            NewMapDialog nmd = new NewMapDialog();
            nmd.ShowDialog();

            if (nmd.DialogResult == DialogResult.OK)
            {
                if (this.currentLevel != null)
                {
                    closeLevel();
                    tm.cleanup();
                }
                this.createNewLevel(nmd.getMapInfo());
            }

            nmd.Dispose();
        }

        private void createNewLevel(MapInformation mapinfo/*List<ResourceFiles.ResourceArchive> resources*/)
        {
            //TODO: Absolute path
            //arc = ResourceFiles.WADResourceFile.loadResourceFile("c:/games/ecwolf/sneswolf.wad");
            Level level = new Level(mapinfo.sizex, mapinfo.sizey, mapinfo.layers, tilelist.tileset[0]);
            level.localThingList = this.thinglist;
            this.selectedTile = tilelist.tileset[0];

            tm.allocateAtlasTexture();
            tm.readyAtlasCreation();
            for (int i = 0; i < mapinfo.files.Count; i++)
            {
                ResourceFiles.ResourceArchive file;
                if (mapinfo.files[i].format == ResourceFiles.ResourceFormat.FORMAT_WAD)
                {
                    file = ResourceFiles.WADResourceFile.loadResourceFile(mapinfo.files[i].filename);
                    tm.getTextureList(file);
                    level.loadedResources.Add(file);
                }
            }
            tm.createInfoTexture();
            tm.uploadNumberTexture();

            level.tm = this.tm;

            renderer.setupTextures(level, tm.resourceInfoID, tm.atlasTextureID, tm.numberTextureID);
            
            currentLevel = level;

            renderer.setupLevelRendering(currentLevel, (uint)sm.programList["WorldRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
            renderer.setupThingUniforms(sm.programList["ThingRender"]);

            this.updateZoneList();
        }

        private void closeLevel()
        {
            //arc.closeResource();
            currentLevel.disposeLevel();
        }

        private void updateZoneList()
        {
            lbZoneList.Items.Clear();
            lbZoneList.Items.Add("Automatic");
            List<Zone> zonelist = currentLevel.getZones();

            for (int x = 0; x < zonelist.Count; x++)
            {
                lbZoneList.Items.Add(String.Format("Zone {0}", x));
            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
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
                    //TODO: Absolute path
                    currentLevel.saveToUWMFFile("c:/dev/textmap.txt");
                }

                if (ltag == 21)
                {
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
                        /*catch (Exception exc)
                        {
                            statusBar1.Panels[0].Text = "Error loading map: " + exc.Message;
                            Console.WriteLine(exc.ToString());
                        }*/
                    }
                    Console.WriteLine("heh");
                }
                if (ltag == 20)
                {
                    doNewMapDialog();
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
                List<Thing> thinglist = currentLevel.getThings();
                for (int i = 0; i < thinglist.Count; i++)
                {
                    Thing thing = thinglist[i];

                    renderer.drawThing(thing, currentLevel, sm.programList["ThingRender"], new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
                }
                List<OpenTK.Vector2> triggerList = currentLevel.getTriggerLocations();

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
                    this.currentLevel.deleteThing(this.currentLevel.highlighted);
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

        private void updateZoom()
        {
            statusBar1.Panels[1].Text = String.Format("Zoom: {0:P}", zoom);
            renderer.zoom = zoom;

            mainLevelPanel.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //zoom += .25f;
            zoom *= 2.0f;
            updateZoom();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //zoom -= .25f;
            zoom *= 0.5f;
            if (zoom < .25f)
                zoom = .25f;
            updateZoom();
        }

        private void setMouseButton(System.Windows.Forms.MouseEventArgs e)
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

        public Vector2 pick(Vector2 mouseCoords)
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
            
            brushmode = true;
            defaultBrush.normalTile = selectedTile;

            setMouseButton(e);
            defaultBrush.ApplyToTile(pick(new Vector2(e.X, e.Y)), 0, this.currentLevel, this.heldMouseButton);
            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (currentLevel == null)
                return;

            Vector2 tile = pick(new Vector2(e.X, e.Y));

            //Console.WriteLine("{0} {1}, center {2} {3}", tile.X, tile.Y, pan.X, pan.Y);

            currentLevel.updateHighlight((int)((tile.X + .5) * 64), (int)((tile.Y + .5) * 64));

            if (brushmode && defaultBrush.repeatable)
            {
                //defaultBrush.ApplyToTile(pick(new Vector2(e.X, e.Y)), 0, this.currentLevel, this.heldMouseButton);
                Vector2 src = pick(lastMousePos);
                LineDrawer.applyBrushOverLine(src, tile, ref this.currentLevel, heldMouseButton, this.defaultBrush);
            }

            if (defaultBrush is TriggerBrush)
            {
                //currentLevel.updateTriggerHighlight(tilex / zoom, tiley / zoom, 0);
            }
            mainLevelPanel.Invalidate();

            lastMousePos.X = e.X;
            lastMousePos.Y = e.Y;
        }

        private void mainLevelPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (currentLevel == null)
                return; 

            brushmode = false;
            defaultBrush.EndBrush(currentLevel);
            this.updateZoneList();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        //old swatches system
        /*private void pbTileList_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int tx = e.X / 16;
            int ty = e.Y / 16;

            //Console.WriteLine("hit {0}, {1}", tx, ty);

            int tile = tx + (ty * 8);

            if (tile < tilelist.tileset.Count)
            {
                locked = true;
                Tile newTile = tilelist.tileset[tile];
                cbTileNorth.Checked = newTile.blockn;
                cbTileSouth.Checked = newTile.blocks;
                cbTileEast.Checked = newTile.blocke;
                cbTileWest.Checked = newTile.blockw;

                tbNorthTex.Text = newTile.texn;
                tbSouthTex.Text = newTile.texs;
                tbEastTex.Text = newTile.texe;
                tbWestTex.Text = newTile.texw;

                cbCenterHoriz.Checked = newTile.offh;
                cbCenterVert.Checked = newTile.offv;

                selectedTile = tilelist.tileset[tile];
                locked = false;
            }
                //selectedTile = tilelist.tileset[tile];
        }*/

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            thingBrush.thing = thinglist.thinglist[thinglist.idlist[listBox1.SelectedIndex]];
        }

        private void triggerFlagChange(object sender, EventArgs e)
        {
            triggerBrush.trigger.acte = cbTrigEast.Checked;
            //triggerBrush.trigger.action = cbTriggerType.SelectedIndex;
            triggerBrush.trigger.actn = cbTrigNorth.Checked;
            triggerBrush.trigger.acts = cbTrigSouth.Checked;
            triggerBrush.trigger.actw = cbTrigWest.Checked;
            triggerBrush.trigger.cross = cbCross.Checked;
            triggerBrush.trigger.repeat = cbRepeat.Checked;
            triggerBrush.trigger.secret = cbSecret.Checked;
            triggerBrush.trigger.usemonster = cbUseMonst.Checked;
            triggerBrush.trigger.useplayer = cbUse.Checked;
        }

        private void triggerParamChange(object sender, EventArgs e)
        {
            triggerBrush.trigger.arg0 = (int)ndParam1.Value;
            triggerBrush.trigger.arg1 = (int)ndParam2.Value;
            triggerBrush.trigger.arg2 = (int)ndParam3.Value;
            triggerBrush.trigger.arg3 = (int)ndParam4.Value;
            triggerBrush.trigger.arg4 = (int)ndParam5.Value;
        }

        private void cbTriggerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            triggerBrush.trigger.action = cbTriggerType.SelectedIndex+1;
        }

        private void thingBitChange(object sender, EventArgs e)
        {
            thingBrush.flags.ambush = this.cbThingAmbush.Checked;
            thingBrush.flags.patrol = this.cbThingPatrol.Checked;
            thingBrush.flags.skill1 = this.cbThingSkill1.Checked;
            thingBrush.flags.skill2 = this.cbThingSkill2.Checked;
            thingBrush.flags.skill3 = this.cbThingSkill3.Checked;
            thingBrush.flags.skill4 = this.cbThingSkill4.Checked;
        }

        private void ndThingAngle_ValueChanged(object sender, EventArgs e)
        {
            if (this.ndThingAngle.Value == 360)
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
            
            lockangle = false;
        }

        private void rbThingEast_CheckedChanged(object sender, EventArgs e)
        {
            if (lockangle) return;
            if (rbThingEast.Checked)
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
                this.ndThingAngle.Value = 315;
        }

        private void lbZoneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.zoneBrush.setCode = lbZoneList.SelectedIndex - 1;
        }

        private void createTile_Events(object sender, EventArgs e)
        {
            if (locked) return;
            Tile newTile = new Tile(0);

            newTile.texn = tbNorthTex.Text;
            newTile.texs = tbSouthTex.Text;
            newTile.texe = tbEastTex.Text;
            newTile.texw = tbWestTex.Text;

            newTile.blockn = cbTileNorth.Checked;
            newTile.blocks = cbTileSouth.Checked;
            newTile.blocke = cbTileEast.Checked;
            newTile.blockw = cbTileWest.Checked;

            newTile.offh = cbCenterHoriz.Checked;
            newTile.offv = cbCenterVert.Checked;

            this.selectedTile = newTile;
        }

        private void tbFloorTex_TextChanged(object sender, EventArgs e)
        {
            Sector newSector = new Sector();
            newSector.texceil = tbCeilingTex.Text;
            newSector.texfloor = tbFloorTex.Text;

            this.sectorBrush.currentSector = newSector;
        }

        private void nudNewTag_ValueChanged(object sender, EventArgs e)
        {
            this.tagBrush.tag = (int)nudNewTag.Value;
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.doNewMapDialog();
        }
    }
}
