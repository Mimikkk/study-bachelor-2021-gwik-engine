using System.Numerics;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives.components;

namespace Sokoban.primitives
{
    internal class GameObject : IRenderable, ITransform
    {
        public string Name { get; }
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Matrix4x4 ViewMatrix => ((ITransform) this).ViewMatrix;

        private Mesh Mesh { get; }
        private ShaderProgram Spo { get; set; }

        public GameObject(string name, Mesh mesh, ShaderProgram? spo = null)
        {
            Spo = spo ?? ShaderProgram.Default;
            // Spo.Configuration = Configuration;
            Name = name;
            Mesh = mesh;
        }

        // This should be handled by renderer
        public bool IsConfigured { get; set; } = true;
        public void Render()
        {
            Renderer.Draw(Mesh.Vao, Spo);
        }

        private void Configuration()
        {
            Mesh.Material.DiffuseTexture?.Bind(0);
            Mesh.Material.ReflectionTexture?.Bind(1);
            Mesh.Material.AmbientTexture?.Bind(2);
            Mesh.Material.NormalTexture?.Bind(3);
            Mesh.Material.LightMapTexture?.Bind(4);
            Mesh.Material.OpacityTexture?.Bind(5);

            //Setup the coordinate systems for our view
            Spo.SetUniform("u_Projection", Game.Camera.ProjectionMatrix);
            Spo.SetUniform("u_View", Game.Camera.ViewMatrix);
            Spo.SetUniform("u_Model", Matrix4x4.Identity);

            //Let the shaders know where the Camera is looking from
            Spo.SetUniform("viewPos", Game.Camera.Position);

            //Configure the materials variables.
            Spo.SetUniform("material.diffuse", 1);
            Spo.SetUniform("material.specular", 1);
            Spo.SetUniform("material.shininess", 324);

            Spo.SetUniform("light.ambient", Mesh.Material.AmbientColor);
            Spo.SetUniform("light.diffuse", Mesh.Material.DiffuseColor);
            Spo.SetUniform("light.specular", Mesh.Material.SpecularColor);
            Spo.SetUniform("light.position", Game.Camera.Position);
        }
    }
}
