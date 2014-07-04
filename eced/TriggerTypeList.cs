/*  ---------------------------------------------------------------------
 *  Copyright (c) 2013 ISB
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eced
{
    public class TriggerType
    {
        public string name;

        public string p1, p2, p3, p4, p5;

        public TriggerType(string name, string p1, string p2, string p3, string p4, string p5)
        {
            this.name = name;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
        }
    }

    public class TriggerTypeList
    {
        public Dictionary<int, TriggerType> triggerList = new Dictionary<int, TriggerType>();
        public List<int> keyList = new List<int>();

        public TriggerTypeList()
        {
            addTrigger(1, new TriggerType("Door_Open", "speed", "lock", "type", "<none>", "<none>"));
            addTrigger(2, new TriggerType("Pushwall_Move", "speed", "direction", "distance", "<none>", "<none>"));
            addTrigger(3, new TriggerType("Exit_Normal", "pos", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(4, new TriggerType("Exit_Secret", "pos", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(5, new TriggerType("Teleport_NewMap", "map", "pos", "flags", "<none>", "<none>"));
            addTrigger(6, new TriggerType("Exit_VictorySpin", "<none>", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(7, new TriggerType("Exit_Victory", "<none>", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(8, new TriggerType("Trigger_Execute", "<none>", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(9, new TriggerType("StartConversation", "<none>", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(10, new TriggerType("Door_Elevator", "<none>", "<none>", "<none>", "<none>", "<none>"));
            addTrigger(11, new TriggerType("Elevator_SwitchFloor", "<none>", "<none>", "<none>", "<none>", "<none>"));
        }

        public void addTrigger(int key, TriggerType trigger)
        {
            triggerList.Add(key, trigger);
            keyList.Add(key);
        }

        public void fillOutComboBox(ref ComboBox box)
        {
            for (int i = 0; i < keyList.Count; i++)
            {
                box.Items.Add(String.Format("{0}: {1}", i + 1, triggerList[keyList[i]].name));
            }
        }
    }
}
