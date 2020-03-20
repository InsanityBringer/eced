using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eced
{
    public class Cell
    {
        public int tile;
        public Sector sector;
        public Zone zone;
        public int tag;
        public List<Trigger> triggerList = new List<Trigger>();

        public bool highlighted = false;
    }

    public class NumberCell
    {
        public int tile, sector, zone, tag;
    }
}
