using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace eced
{
    public class ShaderManager
    {
        public Dictionary<string, int> programList = new Dictionary<string,int>();
        public string loadShader(string filename)
        {
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));

            string str = sr.ReadToEnd();

            sr.Close(); sr.Dispose();

            return str;
        }

        public int makeProgram(string vshader, string fshader, string name)
        {
            Console.WriteLine("Compiling program {0}", name);
            string vsource = loadShader(vshader);
            string fsource = loadShader(fshader);

            int vid = GL.CreateShader(ShaderType.VertexShader);
            int fid = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vid, vsource); GL.CompileShader(vid);
            GL.ShaderSource(fid, fsource); GL.CompileShader(fid);

            Console.Write("VERTEX SHADER: \n" + GL.GetShaderInfoLog(vid) + "\n");
            Console.Write("FRAGMENT SHADER: \n" + GL.GetShaderInfoLog(fid) + "\n");

            int progid = GL.CreateProgram();

            GL.AttachShader(progid, vid);
            GL.AttachShader(progid, fid);
            GL.LinkProgram(progid);

            Console.Write("LINKER: \n", GL.GetProgramInfoLog(progid));

            programList.Add(name, progid);

            Console.WriteLine("Done creating program {0}", name);

            return progid;
        }
    }
}
