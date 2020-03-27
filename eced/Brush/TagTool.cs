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
            this.Repeatable = true;
        }

        public override void ApplyToTile(PickResult pos, Level level, int button)
        {
            level.SetTag(pos.x, pos.y, pos.z, tag);
        }
    }
}
