﻿using System;
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
        Brush defaultBrush = new Brush();
        ThingBrush thingBrush = new ThingBrush();
        TriggerBrush triggerBrush = new TriggerBrush();
        FloodBrush zoneBrush = new FloodBrush();
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

        private int VAOid;
        int program;

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

            Bitmap atlas = new Bitmap("./resources/sneswolftiles.PNG");
            tilelistimg = tilelist.fillOutTiles(atlas);
            atlas.Dispose();

            pbTileList.Image = tilelistimg;

            currentLevel = new Level(64, 64, 1, tilelist.tileset[4]);
            currentLevel.localThingList = this.thinglist;
            selectedTile = tilelist.tileset[0];

            tbToolPanel.Buttons[5].Pushed = true;
            this.gbThingSelect.Visible = false;
            this.gbTileSelection.Visible = true;
            this.gbTriggerData.Visible = false;
            this.gbZoneList.Visible = false;

            this.thingBrush.thing = thinglist.thinglist[1];
            this.thingBrush.thinglist = thinglist;

            this.updateZoneList();

            //OpenGL 3.x setup
            VAOid = GL.GenVertexArray();
            GL.BindVertexArray(VAOid);

            //temp shader compilation test
            ShaderManager sm = new ShaderManager();

            program = GL.CreateProgram();
            int vshader = GL.CreateShader(ShaderType.VertexShader);
            int fshader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vshader, sm.loadShader(".\\resources\\VertexPanTexture.txt"));
            GL.ShaderSource(fshader, sm.loadShader(".\\resources\\FragPanTextureAtlas.txt"));

            GL.CompileShader(vshader); Console.Write("VERTEX SHADER: \n" + GL.GetShaderInfoLog(vshader) + "\n");
            GL.CompileShader(fshader); Console.Write("FRAGMENT SHADER: \n" + GL.GetShaderInfoLog(fshader) + "\n");
            GL.AttachShader(program, vshader); GL.AttachShader(program, fshader);
            GL.LinkProgram(program); Console.Write("LINKER: \n", GL.GetProgramInfoLog(program));

            renderer.tempSetupShaderRenderer(currentLevel, (uint)program, new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("SETUP GL Error: {0}", error.ToString());
            }

            GL.Disable(EnableCap.CullFace);
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

                    switch (toolid)
                    {
                        case 1:
                            this.defaultBrush = new Brush();
                            this.gbThingSelect.Visible = false;
                            this.gbTileSelection.Visible = true;
                            this.gbTriggerData.Visible = false;
                            this.gbZoneList.Visible = false;
                            break;
                        case 2:
                            this.defaultBrush = new TileBrush();
                            this.gbThingSelect.Visible = false;
                            this.gbTileSelection.Visible = true;
                            this.gbTriggerData.Visible = false;
                            this.gbZoneList.Visible = false;
                            break;
                        case 4:
                            this.defaultBrush = thingBrush;
                            this.gbThingSelect.Visible = true;
                            this.gbTileSelection.Visible = false;
                            this.gbTriggerData.Visible = false;
                            this.gbZoneList.Visible = false;
                            break;
                        case 5:
                            this.defaultBrush = triggerBrush;
                            this.gbThingSelect.Visible = false;
                            this.gbTileSelection.Visible = false;
                            this.gbTriggerData.Visible = true;
                            this.gbZoneList.Visible = false;
                            break;
                        case 7:
                            this.defaultBrush = zoneBrush;
                            this.gbThingSelect.Visible = false;
                            this.gbTileSelection.Visible = false;
                            this.gbTriggerData.Visible = false;
                            this.gbZoneList.Visible = true;
                            break;
                    }
                }

                if (ltag == 22)
                {
                    currentLevel.saveToUWMFFile("d:/textmap.txt");
                }

                if (ltag == 21)
                {
                    CodeImp.DoomBuilder.IO.UniversalParser parser = new CodeImp.DoomBuilder.IO.UniversalParser("d:/textmap.txt");

                    Console.WriteLine("Errors: {0}, line {1}", parser.ErrorDescription, parser.ErrorLine);
                    for (int x = 0; x < parser.Root.Count; x++)
                    {
                        Console.WriteLine("Found key {0}", parser.Root[x].Key);
                    }

                    //reconstruct the level
                    if (parser.ErrorDescription == "")
                    {
                        try
                        {
                            Level newLevel = LevelIO.makeNewLevel(parser.Root);
                            newLevel.localThingList = this.thinglist;
                            this.currentLevel = newLevel;
                            this.updateZoneList();
                        }
                        catch (Exception exc)
                        {
                            statusBar1.Panels[0].Text = "Error loading map: " + exc.Message;
                            Console.WriteLine(exc.ToString());
                        }
                    }
                    Console.WriteLine("heh");
                }
                if (ltag == 20)
                {
                }
            }
        }

        private void SetupViewport()
        {
            GL.Viewport(0, 0, mainLevelPanel.Width, mainLevelPanel.Height); // Use all of the glControl painting area

            Matrix4 projection = Matrix4.CreateOrthographic(mainLevelPanel.Width, mainLevelPanel.Height, 0.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        private void mainLevelPanel_Load(object sender, EventArgs e)
        {
            ready = true;

            SetupViewport();

            //GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            //TextureManager.getTexture("./resources/sneswolftiles.PNG");

            //renderer.pan(panx, pany, mainLevelPanel.Width, mainLevelPanel.Height);
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
                renderer.tempShaderRendererDraw(currentLevel, (uint)program, new OpenTK.Vector2(mainLevelPanel.Width, mainLevelPanel.Height));
            }
            GL.Flush();
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine("DRAW GL Error: {0}", error.ToString());
            }
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
            zoom += .10f;
            updateZoom();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zoom -= .10f;
            if (zoom < .10f)
                zoom = .10f;
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

        private void mainLevelPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            brushmode = true;
            defaultBrush.normalTile = selectedTile;
            //int tilex = (e.X + panx);// -(panx % zoom);
            //int tiley = (e.Y + pany);// -(panx % zoom);
            //Console.WriteLine("coords: {0}, {1}, tiles: {2}, {3}, pan: {4}, {5}, raw {6}, {7}", e.X, e.Y, tilex / zoom, tiley / zoom, panx, pany, tilex, tiley);

            setMouseButton(e);
            //defaultBrush.ApplyToTile(tilex, tiley, 0, zoom, this.currentLevel, this.heldMouseButton);
            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Vector2 center = new Vector2(mainLevelPanel.Width / 2, mainLevelPanel.Height / 2);
            Vector2 bstart = new Vector2(center.X - (32 * 8 * zoom) + (pan.X * 64 * 8 * zoom), center.Y - (32 * 8 * zoom) + (pan.Y * 64 * 8 * zoom));
            Vector2 bend = new Vector2(32 * 8 * zoom, 32 * 8 * zoom);
            Vector2 curpos = new Vector2(e.X - bstart.X, e.Y - bstart.Y);
            Vector2 tile = new Vector2((int)(curpos.X / (8 * zoom)), (int)(curpos.Y / (8 * zoom)));

            Console.WriteLine("{0} {1}, center {2} {3}", tile.X, tile.Y, pan.X, pan.Y);

            if (brushmode && defaultBrush.repeatable)
            {
                //defaultBrush.ApplyToTile(tilex, tiley, 0, zoom, this.currentLevel, heldMouseButton);
                //mainLevelPanel.Invalidate();
            }
            //int mapcoordx = (int)((double)e.X * (64d / (double)zoom)) + (int)((double)panx * (64d / (double)zoom));
            //int mapcoordy = (int)((double)e.Y * (64d / (double)zoom)) + (int)((double)pany * (64d / (double)zoom));
            //currentLevel.updateHighlight(mapcoordx, mapcoordy);

            if (defaultBrush is TriggerBrush)
            {
                //currentLevel.updateTriggerHighlight(tilex / zoom, tiley / zoom, 0);
            }
            mainLevelPanel.Invalidate();
        }

        private void mainLevelPanel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            brushmode = false;
            defaultBrush.EndBrush(currentLevel);
            this.updateZoneList();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pbTileList_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int tx = e.X / 16;
            int ty = e.Y / 16;

            //Console.WriteLine("hit {0}, {1}", tx, ty);

            int tile = tx + (ty * 8);

            if (tile < tilelist.tileset.Count)
                selectedTile = tilelist.tileset[tile];
        }

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
    }
}
