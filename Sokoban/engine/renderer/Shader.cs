using System;
using Silk.NET.OpenGL;

namespace GWiK_Sokoban.engine.renderer
{
    public class Shader : IDisposable
    {
        public override string ToString()
        {
            return $"Shader : {Type.ToString()} -  {Name}";
        }

        public Shader(ShaderType type, string name)
        {
            Type = type;
            Name = name;
            Handle = Game.Gl.CreateShader(Type);

            Game.Gl.ShaderSource(Handle, Source);
            Game.Gl.CompileShader(Handle);

            var infoLog = Game.Gl.GetShaderInfoLog(Handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        public uint Handle { get; }
        public ShaderType Type { get; }
        public string Name { get; }

        private string Source => Shaderpath.LoadFileToString();
        private Path Shaderpath => Path.Shaders / $"{Name}{Extension}";
        private string Extension => FindCorrespondingExtension();

        private string FindCorrespondingExtension()
        {
            return Type switch
            {
                ShaderType.FragmentShader => ".frag",
                ShaderType.VertexShader => ".vert",
                ShaderType.GeometryShader => ".geom",
                ShaderType.TessEvaluationShader => ".tese",
                ShaderType.TessControlShader => ".tesc",
                ShaderType.ComputeShader => ".comp",
                _ => throw new ArgumentOutOfRangeException($"Unsupported ShaderType : {Type}")
            };
        }

        public void Dispose()
        {
            Game.Gl.DeleteShader(Handle);
        }
    }
}