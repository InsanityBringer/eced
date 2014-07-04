using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeImp.DoomBuilder.IO;

namespace eced
{
    public class Trigger
    {
        public int x, y, z;

        public bool actn, acts, acte, actw;
        public int action;

        public int arg0, arg1, arg2, arg3, arg4;

        public bool useplayer, usemonster, cross, repeat, secret;

        public bool highlighted = false;

        public Trigger()
        {
        }

        public Trigger(Trigger clone)
        {
            this.x = clone.x;
            this.y = clone.y;
            this.z = clone.z;

            this.actn = clone.actn;
            this.acts = clone.acts;
            this.actw = clone.actw;
            this.acte = clone.acte;

            this.action = clone.action;

            this.arg0 = clone.arg0;
            this.arg1 = clone.arg1;
            this.arg2 = clone.arg2;
            this.arg3 = clone.arg3;
            this.arg4 = clone.arg4;

            this.useplayer = clone.useplayer;
            this.usemonster = clone.usemonster;
            this.cross = clone.cross;
            this.repeat = clone.repeat;
            this.secret = clone.secret;
        }

        public string getUWMFString()
        {
            StringBuilder stringmaker = new StringBuilder();
            stringmaker.Append("trigger\n{\n");
            stringmaker.Append("\tx = "); stringmaker.Append(x); stringmaker.Append(";");
            stringmaker.Append("\n\ty = "); stringmaker.Append(y); stringmaker.Append(";");
            stringmaker.Append("\n\tz = "); stringmaker.Append(z); stringmaker.Append(";");
            stringmaker.Append("\n\taction = "); stringmaker.Append(action); stringmaker.Append(";");

            stringmaker.Append("\n\tactivatenorth = "); stringmaker.Append(actn.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tactivatesouth = "); stringmaker.Append(acts.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tactivateeast = "); stringmaker.Append(acte.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tactivatewest = "); stringmaker.Append(actw.ToString().ToLower()); stringmaker.Append(";");

            stringmaker.Append("\n\targ0 = "); stringmaker.Append(arg0); stringmaker.Append(";");
            stringmaker.Append("\n\targ1 = "); stringmaker.Append(arg1); stringmaker.Append(";");
            stringmaker.Append("\n\targ2 = "); stringmaker.Append(arg2); stringmaker.Append(";");
            stringmaker.Append("\n\targ3 = "); stringmaker.Append(arg3); stringmaker.Append(";");
            stringmaker.Append("\n\targ4 = "); stringmaker.Append(arg4); stringmaker.Append(";");

            stringmaker.Append("\n\tplayeruse = "); stringmaker.Append(useplayer.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tmonsteruse = "); stringmaker.Append(usemonster.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tplayercross = "); stringmaker.Append(cross.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\trepeatable = "); stringmaker.Append(repeat.ToString().ToLower()); stringmaker.Append(";");
            stringmaker.Append("\n\tsecret = "); stringmaker.Append(secret.ToString().ToLower()); stringmaker.Append(";");

            stringmaker.Append("\n}");

            return stringmaker.ToString();
        }

        public static Trigger Reconstruct(UniversalCollection data)
        {
            Trigger trigger = new Trigger();

            trigger.x = UWMFSearch.getIntTag(data, "x", 0);
            trigger.y = UWMFSearch.getIntTag(data, "y", 0);
            trigger.z = UWMFSearch.getIntTag(data, "z", 0);
            trigger.action = UWMFSearch.getIntTag(data, "action", 0);

            trigger.actn = UWMFSearch.getBoolTag(data, "activatenorth", true);
            trigger.acts = UWMFSearch.getBoolTag(data, "activatesouth", true);
            trigger.acte = UWMFSearch.getBoolTag(data, "activateeast", true);
            trigger.actw = UWMFSearch.getBoolTag(data, "activatewest", true);

            trigger.arg0 = UWMFSearch.getIntTag(data, "arg0", 0);
            trigger.arg1 = UWMFSearch.getIntTag(data, "arg1", 0);
            trigger.arg2 = UWMFSearch.getIntTag(data, "arg2", 0);
            trigger.arg3 = UWMFSearch.getIntTag(data, "arg3", 0);
            trigger.arg4 = UWMFSearch.getIntTag(data, "arg4", 0);

            trigger.useplayer = UWMFSearch.getBoolTag(data, "playeruse", false);
            trigger.usemonster = UWMFSearch.getBoolTag(data, "monsteruse", false);
            trigger.cross = UWMFSearch.getBoolTag(data, "cross", false);
            trigger.repeat = UWMFSearch.getBoolTag(data, "repeatable", false);
            trigger.secret = UWMFSearch.getBoolTag(data, "secret", false);

            return trigger;
        }
    }
}
