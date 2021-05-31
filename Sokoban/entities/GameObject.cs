#nullable enable
using System;
using Silk.NET.Maths;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives.components;

namespace Sokoban.entities
{
    internal class GameObject : IRenderable
    {
        public string Name { get; }
        public Mesh Mesh { get; }
        public ShaderProgram Spo { get; }

        public GameObject(string name, Mesh mesh, ShaderProgram? spo = null)
        {
            Spo = spo ?? ShaderProgram.Default;
            if (spo == null) Spo.Configuration = Configuration;
            
            Name = name;
            Mesh = mesh;
        }

        private void Configuration()
        {
            Mesh.Material.AmbientTexture?.Bind(0);

            Spo.SetUniform("u_Projection", Game.Camera.Projection);
            Spo.SetUniform("u_View", Game.Camera.View);
            Spo.SetUniform("u_Model", Matrix4X4<float>.Identity);
        }
    }
}
