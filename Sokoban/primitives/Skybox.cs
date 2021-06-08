#nullable enable
using System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives.components;
using Shader = Sokoban.engine.renderer.Shader;

namespace Sokoban.primitives
{
    internal class Skybox : IRenderable
    {
        public string Name { get; } = "Skybox";
        public Mesh Mesh { get; }
        public ShaderProgram Spo { get; }

        private static float[] Vertices { get; } =
        {
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            -1.0f,  1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f
        };

        private static int[] Indices { get; }
        
        public Skybox()
        {
            Spo = new ShaderProgram(new Shader(ShaderType.VertexShader, Name),
                new Shader(ShaderType.FragmentShader, Name));
        }
        protected Skybox(GameObject gameObject)
        {
            Spo = gameObject.Spo;
            Name = gameObject.Name;
            Mesh = gameObject.Mesh;
        }

        public virtual void ShaderConfiguration()
        {
            Mesh.Material.DiffuseTexture?.Bind(0);

            Spo.SetUniform("u_Projection", Game.Camera.Projection);
            Spo.SetUniform("u_View", Game.Camera.Lens.View);
            Spo.SetUniform("u_Model", Matrix4X4<float>.Identity);
        }
    }
}