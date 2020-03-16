using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eced.Brushes
{
    public class RoomBrush : EditorBrush
    {
        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;
            level.SetTile(tx, ty, z, null);
            if (level.GetTile(tx - 1, ty, z) != null)
                level.SetTile(tx - 1, ty, z, normalTile);
            if (level.GetTile(tx + 1, ty, z) != null)
                level.SetTile(tx + 1, ty, z, normalTile);
            if (level.GetTile(tx, ty + 1, z) != null)
                level.SetTile(tx, ty + 1, z, normalTile);
            if (level.GetTile(tx, ty - 1, z) != null)
                level.SetTile(tx, ty - 1, z, normalTile);
        }
    }
}
