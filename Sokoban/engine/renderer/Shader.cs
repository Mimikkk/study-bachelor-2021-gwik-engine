using System;
using Silk.NET.OpenGL;
using Sokoban.utilities;

namespace Sokoban.engine.renderer
{
    public class Shader : IDisposable
    {
        public override string ToString()
        {
            return $"Shader({Name}: {Type})";
        }

        public Shader(ShaderType type, string name)
        {
            Type = type;
            Name = name;
            Handle = Api.Gl.CreateShader(Type);

            Api.Gl.ShaderSource(Handle, Source);
            Api.Gl.CompileShader(Handle);
            VerifyCompilation();
        }

        private void VerifyCompilation()
        {
            var infoLog = Api.Gl.GetShaderInfoLog(Handle);
            if (string.IsNullOrWhiteSpace(infoLog)) return;
            $"<c6 Error compiling shader of type> <c124 {Type}>, <c6 failed with error> <c124 {infoLog}>".LogLine();
            throw new Exception();
        }

        public uint Handle { get; }
        public ShaderType Type { get; }
        public string Name { get; }

        private string Source => Shaderpath.LoadFileToString();
        private Path Shaderpath => Path.Shaders / $"{Name}{Extension}";
        private string Extension
        {
            get => Type switch
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
            Api.Gl.DeleteShader(Handle);
        }
    }
}
