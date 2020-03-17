/*  ---------------------------------------------------------------------
 *  Copyright (c) 2020 ISB
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
using System.IO;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace eced.Renderer
{
    public class Shader
    {
        public int ShaderID { get; private set; }
        public Dictionary<string, int> UniformLocations { get; } = new Dictionary<string, int>();
        //For debugging purposes atm
        private string name;

        public bool isValid = false;


        public Shader(string name)
        {
            this.name = name;
        }

        public void UseShader()
        {
            GL.UseProgram(ShaderID);
        }

        public void Init()
        {
            ShaderID = GL.CreateProgram();
        }

        public void AddShader(string filename, ShaderType type)
        {
            Console.WriteLine("Adding shader {0}", filename);
            StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open));
            string shaderSource = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            int id = GL.CreateShader(type);
            GL.ShaderSource(id, shaderSource);
            GL.CompileShader(id);

            int status;
            GL.GetShader(id, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                Console.WriteLine("Error compiling shader {0}:", filename);
                string infolog = GL.GetShaderInfoLog(id);
                Console.WriteLine(infolog);
            }
            else
            {
                GL.AttachShader(ShaderID, id);
            }
        }

        public void LinkShader()
        {
            GL.LinkProgram(ShaderID);
            int status;
            GL.GetProgram(ShaderID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine("Linking program");
            if (status != 1)
            {
                Console.WriteLine("Error linking program {0}: ");
                string log = GL.GetProgramInfoLog(ShaderID);
                Console.WriteLine(log);
            }
            else
            {
                isValid = true;
            }
        }

        public void AddUniform(string uniformName)
        {
            int location = GL.GetUniformLocation(ShaderID, uniformName);
            if (location != -1)
                UniformLocations[uniformName] = location;
            else
                Console.Error.WriteLine("Shader::AddUniform: Uniform added to shader doesn't exist");
        }
    }
}
