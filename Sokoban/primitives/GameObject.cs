#nullable enable
using System;
using Silk.NET.Maths;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives.components;

namespace Sokoban.primitives
{
    internal class GameObject : IRenderable
    {
        public string Name { get; set; }
        public Mesh Mesh { get; }
        public ShaderProgram Spo { get; }

        public GameObject(string name, Mesh mesh, ShaderProgram? spo = null)
        {
            Spo = spo ?? ShaderProgram.Default;

            Name = name;
            Mesh = mesh;
        }
        protected GameObject(GameObject gameObject)
        {
            Spo = gameObject.Spo;
            Name = gameObject.Name;
            Mesh = gameObject.Mesh;
        }

        public virtual void ShaderConfiguration()
        {
            Mesh.Material.AmbientTexture?.Bind(0);

            Spo.SetUniform("u_Projection", Game.Camera.Projection);
            Spo.SetUniform("u_View", Game.Camera.Lens.View);
            Spo.SetUniform("u_Model", Matrix4X4<float>.Identity);
        }
    }
}