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
using System.Xml.Linq;
using System.IO;

namespace eced
{
    public enum FieldType
    {
        Normal,
        Tag,
        Dir,
        ThingID
    }
    public class TriggerType
    {
        public string name;

        public string p1 = "-", p2 = "-", p3 = "-", p4 = "-", p5 = "-";
        public FieldType p1Type, p2Type, p3Type, p4Type, p5Type;

        public TriggerType()
        {
        }

        public TriggerType(string name, string p1, string p2, string p3, string p4, string p5)
        {
            this.name = name;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
        }

        //why
        public FieldType FieldTypeFromName(string str)
        {
            switch (str)
            {
                case "tag":
                    return FieldType.Tag;
                case "dir":
                    return FieldType.Dir;
                case "tid":
                    return FieldType.ThingID;
            }
            return FieldType.Normal;
        }

        public static TriggerType FromXContainer(XContainer container)
        {
            TriggerType newTrigger = new TriggerType();

            foreach (XElement elem in container.Elements())
            {
                switch (elem.Name.LocalName)
                {
                    case "name":
                        newTrigger.name = (string)elem;
                        break;
                    case "arg1":
                        newTrigger.p1 = (string)elem;
                        break;
                    case "arg2":
                        newTrigger.p2 = (string)elem;
                        break;
                    case "arg3":
                        newTrigger.p3 = (string)elem;
                        break;
                    case "arg4":
                        newTrigger.p4 = (string)elem;
                        break;
                    case "arg5":
                        newTrigger.p5 = (string)elem;
                        break;
                    case "arg1type":
                        newTrigger.p1Type = newTrigger.FieldTypeFromName((string)elem);
                        break;
                    case "arg2type":
                        newTrigger.p2Type = newTrigger.FieldTypeFromName((string)elem);
                        break;
                    case "arg3type":
                        newTrigger.p3Type = newTrigger.FieldTypeFromName((string)elem);
                        break;
                    case "arg4type":
                        newTrigger.p4Type = newTrigger.FieldTypeFromName((string)elem);
                        break;
                    case "arg5type":
                        newTrigger.p5Type = newTrigger.FieldTypeFromName((string)elem);
                        break;
                }
            }

            return newTrigger;
        }
    }

    public class TriggerManager
    {
        public List<TriggerType> triggerList = new List<TriggerType>();
        public Dictionary<string, int> triggerMapping = new Dictionary<string, int>();

        public TriggerManager()
        {
        }

        public void LoadTriggerDefinitions(string filename)
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            try
            {
                XDocument doc = XDocument.Load(sr);
                IEnumerable<XNode> nodelist = doc.Nodes();
                foreach (XNode node in nodelist)
                {
                    if (node is XContainer)
                    {
                        XContainer container = (XContainer)node;
                        IEnumerable<XNode> sublist = container.Nodes();

                        foreach (XNode subnode in sublist)
                        {
                            if (subnode is XContainer)
                            {
                                XContainer triggerData = (XContainer)subnode;
                                try
                                {
                                    TriggerType trigger = TriggerType.FromXContainer(triggerData);
                                    triggerList.Add(trigger);
                                    triggerMapping.Add(trigger.name, triggerList.Count - 1);
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine("Failure when parsing trigger type");
                                    Console.WriteLine(exc.Message);
                                    Console.WriteLine(exc.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failure in reading: " + exc.Message);
            }
            finally
            {
                sr.Close();
                Console.WriteLine("{0} trigger types successfully registered", this.triggerList.Count);
            }

            //return list;
        }
    }
}
