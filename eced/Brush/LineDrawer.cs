using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace eced.Brushes
{
    public class LineDrawer
    {
        /// <summary>
        /// Applies a brush on all tiles in between the two points
        /// </summary>
        /// <param name="src">The picked src point</param>
        /// <param name="dst">The picked destination point</param>
        public static void DrawLineWithBrush(PickResult src, PickResult dst, Level currentLevel, int heldMouseButton, EditorBrush defaultBrush)
        {
            //Vector2 srct = pick(src);
            //Vector2 dstt = pick(dst);
            int swapTemp;

            if (src.y > dst.y)
            {
                swapTemp = src.y;
                src.y = dst.y;
                dst.y = swapTemp;
                swapTemp = src.x;
                src.x = dst.x;
                dst.x = swapTemp;
            }

            int dx = (int)(dst.x - src.x);
            int dy = (int)(dst.y - src.y);

            //Console.WriteLine("{0} {1} {2} {3}", dx, dy, src.Y, dst.Y);

            if (dx > 0)
            {
                if (dx > dy)
                    LineOctant0(src, dx, dy, 1, currentLevel, heldMouseButton, defaultBrush);
                else
                    LineOctant1(src, dx, dy, 1, currentLevel, heldMouseButton, defaultBrush);
            }
            else
            {
                dx = -dx;
                if (dx > dy)
                    LineOctant0(src, dx, dy, -1, currentLevel, heldMouseButton, defaultBrush);
                else
                    LineOctant1(src, dx, dy, -1, currentLevel, heldMouseButton, defaultBrush);
            }

            //apply brush to tile
            //defaultBrush.ApplyToTile(srct, 0, this.currentLevel, this.heldMouseButton);
        }

        private static void LineOctant0(PickResult src, int dx, int dy, int direction, Level currentLevel, int heldMouseButton, EditorBrush defaultBrush)
        {
            //Console.WriteLine("octant 0 line");
            int doubledy = dy * 2;
            int doubledymdoubledx = doubledy - dx * 2;
            int error = doubledy - dx;

            defaultBrush.ApplyToTile(src, currentLevel, heldMouseButton);

            while (dx != 0)
            {
                if (error >= 0)
                {
                    src.y++;
                    error += doubledymdoubledx;
                }
                else
                {
                    error += doubledy;
                }
                src.x += direction;
                defaultBrush.ApplyToTile(src, currentLevel, heldMouseButton);

                dx--;
            }
        }

        private static void LineOctant1(PickResult src, int dx, int dy, int direction, Level currentLevel, int heldMouseButton, EditorBrush defaultBrush)
        {
            //Console.WriteLine("octant 1 line");
            int doubledx = dx * 2;
            int doubledxmdoubledy = doubledx - dy * 2;
            int error = doubledx - dy;

            defaultBrush.ApplyToTile(src, currentLevel, heldMouseButton);

            while (dy != 0)
            {
                if (error >= 0)
                {
                    //src.X++;
                    src.x += direction;
                    error += doubledxmdoubledy;
                }
                else
                {
                    error += doubledx;
                }
                //src.Y += direction;
                src.y++;
                defaultBrush.ApplyToTile(src, currentLevel, heldMouseButton);

                dy--;
            }
        } 
    }
}
