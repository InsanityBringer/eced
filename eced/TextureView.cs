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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using eced.Renderer;

namespace eced
{
    public partial class TextureView : ScrollableControl
    {
        public int TextureMargin { get; set; } = 8;
        public int FontSize { get; set; } = 12;
        public string SelectedTextureName
        {
            get => mSelectedTextureName;
            set
            {
                mSelectedTextureName = value;
                //Console.WriteLine("Setting texture name {0}", value);

                if (nameToNumberMapping.ContainsKey(value))
                {
                    Console.WriteLine("it's in there");
                    int yOffset = nameToNumberMapping[value] / numTexturesX;
                    int startY = yOffset * (128 + FontSize + TextureMargin) + TextureMargin - 128;
                    if (startY < 0) startY = 0;

                    desiredVerticalScroll = startY;
                }
            }
        }
        private int numTexturesX;
        private int numTexturesY;
        private int numTextures = 1400; //testing
        private List<TextureInformation> mTextureList;
        private string mSearchString = "";
        private string mSelectedTextureName = "-";
        private List<TextureInformation> currentList;
        private Dictionary<string, int> nameToNumberMapping;
        private int desiredVerticalScroll = -1;

        public TextureCache Cache { get; set; }
        public List<TextureInformation> TextureList { 
            get => mTextureList;
            set
            {
                mTextureList = value;
                nameToNumberMapping = new Dictionary<string, int>();
                SortInputList();
                BuildLocalList();
            }
        }
        public string SearchString
        {
            get => mSearchString;
            set
            {
                mSearchString = value.ToUpperInvariant();
                BuildLocalList();
            }
        }
        public TextureView()
        {
            InitializeComponent();
            AutoScroll = true;
            VScroll = true;
        }

        private void SortInputList()
        {
            mTextureList.Sort();
        }

        private void BuildLocalList()
        {
            if (currentList != null)
                currentList.Clear();
            else
                currentList = new List<TextureInformation>();

            if (nameToNumberMapping != null)
                nameToNumberMapping.Clear();

            /*if (SearchString == "")
            {
                currentList = mTextureList.ToList();
            }
            else*/
            {
                foreach (TextureInformation info in mTextureList)
                {
                    //This is a hack, but the above solution wouldn't update the dict
                    if (mSearchString == "" || info.name.Contains(mSearchString))
                    {
                        nameToNumberMapping.Add(info.name, currentList.Count);
                        currentList.Add(info);
                    }
                }
            }
            numTextures = currentList.Count;
            CalcBounds();
        }

        private void CalcBounds()
        {
            //Console.WriteLine("resizing");
            //TODO: This is just an approximation, needs to be changed to work for high DPI displays and all that
            FontSize = (int)Font.SizeInPoints + 4;
            numTexturesX = this.ClientSize.Width / (128 + TextureMargin);
            if (numTexturesX <= 0) return; //Control is too small
            numTexturesY = (numTextures / numTexturesX) + 1;
            AutoScrollMinSize = new Size(numTexturesX * (128 + TextureMargin), numTexturesY * (128 + TextureMargin + FontSize) + 8);
            //This seems required to get the scroll bar state to be synced properly, but also can call into itself? Uh oh...
            AdjustFormScrollbars(true);
            //When the amount of textures changes, the entire list may need to be redrawn. 
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //Console.WriteLine("Painting {0}", pe.ClipRectangle);
            SolidBrush brush = new SolidBrush(Color.Black);
            pe.Graphics.FillRectangle(brush, pe.ClipRectangle);
            brush.Dispose();

            if (Cache != null)
            {
                //Create a new rectangle in the space of the entire control's view. 
                Rectangle newRect = pe.ClipRectangle;
                newRect.Y += VerticalScroll.Value;
                //Measure how many textures this clip rectangle will involve drawing.
                int firstTextureX = newRect.X / (128 + TextureMargin);
                int lastTextureX = newRect.Right / (128 + TextureMargin);
                lastTextureX = Math.Min(lastTextureX, numTexturesX);

                int firstTextureY = newRect.Y / (128 + TextureMargin + FontSize);
                int lastTextureY = newRect.Bottom / (128 + TextureMargin + FontSize);
                lastTextureY = Math.Min(lastTextureY, numTexturesY);

                int textureNum;
                Rectangle textureBox;
                Bitmap image;
                TextureInformation info;
                Brush textBrush = new SolidBrush(Color.White);
                Brush selectBrush = new SolidBrush(SystemColors.Highlight);
                Brush alphaBrush = new SolidBrush(Color.FromArgb(128, SystemColors.Highlight));
                //Draw the texture cells
                for (int x = firstTextureX; x < lastTextureX; x++)
                {
                    for (int y = firstTextureY; y <= lastTextureY; y++)
                    {
                        textureNum = y * (numTexturesX) + x;
                        if (textureNum < numTextures)
                        {
                            info = currentList[textureNum];

                            if (info.name == SelectedTextureName)
                            {
                                textureBox = new Rectangle(x * (128 + TextureMargin), y * (128 + TextureMargin + FontSize) + TextureMargin - VerticalScroll.Value, 128 + (TextureMargin << 1), 128 + (TextureMargin << 1));
                                pe.Graphics.FillRectangle(selectBrush, textureBox);
                            }

                            textureBox = new Rectangle(x * (128 + TextureMargin) + TextureMargin, y * (128 + TextureMargin + FontSize) + TextureMargin - VerticalScroll.Value, 128, 128);
                            if (!Cache.RequestImage(info.id, out image))
                            {
                                Console.WriteLine("cache miss on texture {0}, id {1}", info.name, info.id);
                            }
                            pe.Graphics.DrawImageUnscaled(image, textureBox.X, textureBox.Y);
                            if (info.name == SelectedTextureName)
                            {
                                pe.Graphics.FillRectangle(alphaBrush, textureBox);
                            }
                            pe.Graphics.DrawString(info.name, Font, textBrush, new PointF(textureBox.X, textureBox.Y + 130));
                            //brush = new SolidBrush(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)));
                            //pe.Graphics.FillRectangle(brush, textureBox);
                            //brush.Dispose();
                        }
                    }
                }
                textBrush.Dispose();
                alphaBrush.Dispose();
                selectBrush.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalcBounds();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (nameToNumberMapping.ContainsKey(SelectedTextureName))
            {
                int oldIndex = nameToNumberMapping[SelectedTextureName];
                int oldCellX = oldIndex % numTexturesX;
                int oldCellY = oldIndex / numTexturesX;

                Rectangle oldTextureBox = new Rectangle(oldCellX * (128 + TextureMargin), oldCellY * (128 + TextureMargin + FontSize) + TextureMargin - VerticalScroll.Value, 128 + (TextureMargin << 1), 128 + (TextureMargin << 1));
                Invalidate(oldTextureBox);
            }

            int cellX = (-TextureMargin + e.X) / (128 + TextureMargin);
            int cellY = (-TextureMargin + e.Y + VerticalScroll.Value) / (128 + TextureMargin + FontSize);
            Console.WriteLine("({0} {1})", cellX, cellY);

            if (cellX < 0 || cellY < 0) return;

            if (cellX >= numTexturesX) return;

            int textureNum = cellY * numTexturesX + cellX;
            if (textureNum >= currentList.Count) return;

            SelectedTextureName = currentList[textureNum].name;
            //only invalidate the one region, since it takes a while to draw
            Rectangle textureBox = new Rectangle(cellX * (128 + TextureMargin), cellY * (128 + TextureMargin + FontSize) + TextureMargin - VerticalScroll.Value, 128 + (TextureMargin << 1), 128 + (TextureMargin << 1));
            Invalidate(textureBox);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (desiredVerticalScroll > 0 && desiredVerticalScroll < VerticalScroll.Maximum)
            {
                VerticalScroll.Value = desiredVerticalScroll;
                desiredVerticalScroll = -1;
            }
        }
    }

    public class TextureCache
    {
        private Dictionary<int, Bitmap> cache = new Dictionary<int, Bitmap>();
        public TextureManager TexturesManager { get; }

        public TextureCache(TextureManager manager)
        {
            TexturesManager = manager;
        }

        public void Purge()
        {
            //Need to make sure all the bitmaps are disposed of
            foreach (Bitmap image in cache.Values)
            {
                image.Dispose();
            }
            cache.Clear();
        }

        public bool RequestImage(int id, out Bitmap image)
        {
            if (cache.ContainsKey(id))
            {
                image = cache[id];
                return true;
            }
            image = new Bitmap(128, 128);
            byte[] data = TexturesManager.GetTextureImage(id);
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, 128, 128), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bmpData.Scan0, 128 * 128 * 4);
            image.UnlockBits(bmpData);
            cache[id] = image;
            return false;
        }
    }
}
