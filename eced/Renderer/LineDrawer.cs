using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace eced
{
    public class LineDrawer
    {
        /// <summary>
        /// Applies a brush on all tiles in between the two points
        /// </summary>
        /// <param name="src">The picked src point</param>
        /// <param name="dst">The picked destination point</param>
        public static void applyBrushOverLine(Vector2 src, Vector2 dst, ref Level currentLevel, int heldMouseButton, Brush defaultBrush)
        {
            //Vector2 srct = pick(src);
            //Vector2 dstt = pick(dst);
            float swapTemp;

            if (src.Y > dst.Y)
            {
                swapTemp = src.Y;
                src.Y = dst.Y;
                dst.Y = swapTemp;
                swapTemp = src.X;
                src.X = dst.X;
                dst.X = swapTemp;
            }

            int dx = (int)(dst.X - src.X);
            int dy = (int)(dst.Y - src.Y);

            //Console.WriteLine("{0} {1} {2} {3}", dx, dy, src.Y, dst.Y);

            if (dx > 0)
            {
                if (dx > dy)
                    lineOctant0(src, dx, dy, 1, ref currentLevel, heldMouseButton, defaultBrush);
                else
                    lineOctant1(src, dx, dy, 1, ref currentLevel, heldMouseButton, defaultBrush);
            }
            else
            {
                dx = -dx;
                if (dx > dy)
                    lineOctant0(src, dx, dy, -1, ref currentLevel, heldMouseButton, defaultBrush);
                else
                    lineOctant1(src, dx, dy, -1, ref currentLevel, heldMouseButton, defaultBrush);
            }

            //apply brush to tile
            //defaultBrush.ApplyToTile(srct, 0, this.currentLevel, this.heldMouseButton);
        }

        private static void lineOctant0(Vector2 src, int dx, int dy, int direction, ref Level currentLevel, int heldMouseButton, Brush defaultBrush)
        {
            //Console.WriteLine("octant 0 line");
            int doubledy = dy * 2;
            int doubledymdoubledx = doubledy - dx * 2;
            int error = doubledy - dx;

            defaultBrush.ApplyToTile(src, 0, currentLevel, heldMouseButton);

            while (dx != 0)
            {
                if (error >= 0)
                {
                    src.Y++;
                    error += doubledymdoubledx;
                }
                else
                {
                    error += doubledy;
                }
                src.X += direction;
                defaultBrush.ApplyToTile(src, 0, currentLevel, heldMouseButton);

                dx--;
            }
        }

        private static void lineOctant1(Vector2 src, int dx, int dy, int direction, ref Level currentLevel, int heldMouseButton, Brush defaultBrush)
        {
            //Console.WriteLine("octant 1 line");
            int doubledx = dx * 2;
            int doubledxmdoubledy = doubledx - dy * 2;
            int error = doubledx - dy;

            defaultBrush.ApplyToTile(src, 0, currentLevel, heldMouseButton);

            while (dy != 0)
            {
                if (error >= 0)
                {
                    //src.X++;
                    src.X += direction;
                    error += doubledxmdoubledy;
                }
                else
                {
                    error += doubledx;
                }
                //src.Y += direction;
                src.Y++;
                defaultBrush.ApplyToTile(src, 0, currentLevel, heldMouseButton);

                dy--;
            }
        } 
    }
}
