using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced
{
    class Cell
    {
        public Tile tile;
        public Sector sector;
        public Zone zone;
        public List<Trigger> triggerList = new List<Trigger>();

        public bool highlighted = false;
    }

    class NumberCell
    {
        public int tile, sector, zone;
    }
}
