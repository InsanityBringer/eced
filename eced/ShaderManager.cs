using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace eced
{
    class ShaderManager
    {
        public string loadShader(string filename)
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));

            string str = sr.ReadToEnd();

            sr.Close(); sr.Dispose();

            return str;
        }
    }
}
