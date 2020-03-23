using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced.Brushes
{
    public class TagTool : EditorBrush
    {
        public int tag;

        public TagTool(EditorState state)
            : base(state)
        {
            this.repeatable = true;
        }

        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;

            level.SetTag(tx, ty, z, tag);
        }
    }
}
