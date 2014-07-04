using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced
{
    public class SectorBrush : Brush
    {
        public Sector currentSector;

        public SectorBrush()
            : base()
        {
            this.repeatable = true;
        }

        public override void ApplyToTile(OpenTK.Vector2 pos, int z, Level level, int button)
        {
            int tx = (int)pos.X;
            int ty = (int)pos.Y;

            level.setSector(tx, ty, z, currentSector);
        }
    }
}
