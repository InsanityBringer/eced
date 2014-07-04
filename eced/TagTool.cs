using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced
{
    public class TagTool : Brush
    {
        public int tag;

        public TagTool()
            : base()
        {
            this.repeatable = true;
        }

        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;

            level.setTag(tx, ty, z, tag);
        }
    }
}
