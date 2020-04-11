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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eced
{
    public partial class TextureView : ScrollableControl
    {
        public int TextureMargin { get; set; } = 8;
        public int FontSize { get; set; } = 12;
        private int numTexturesX;
        private int numTexturesY;
        private int numTextures = 1400; //testing
        private Random random;
        public TextureView()
        {
            InitializeComponent();
            AutoScroll = true;
            VScroll = true;
            random = new Random();
        }

        private void CalcBounds()
        {
            //Console.WriteLine("resizing");
            //TODO: This is just an approximation, needs to be changed to work for high DPI displays and all that
            FontSize = (int)Font.SizeInPoints + 4;
            numTexturesX = this.ClientSize.Width / (128 + TextureMargin);
            if (numTexturesX <= 0) return; //Control is too small
            numTexturesY = (numTextures / numTexturesX);
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
            Brush textBrush = new SolidBrush(Color.White);
            //Draw the texture cells
            for (int x = firstTextureX; x < lastTextureX; x++)
            {
                for (int y = firstTextureY; y <= lastTextureY; y++)
                {
                    textureNum = y * (numTexturesX) + x;
                    if (textureNum < numTextures)
                    {
                        random = new Random(textureNum); //there's not a SetSeed function why exactly?
                        brush = new SolidBrush(Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)));
                        textureBox = new Rectangle(x * (128 + TextureMargin) + TextureMargin, y * (128 + TextureMargin + FontSize) + TextureMargin - VerticalScroll.Value, 128, 128);
                        pe.Graphics.FillRectangle(brush, textureBox);
                        pe.Graphics.DrawString(textureNum.ToString(), Font, textBrush, new PointF(textureBox.X, textureBox.Y + 130));
                        brush.Dispose();
                    }
                }
            }
            textBrush.Dispose();
            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalcBounds();
        }
    }
}
