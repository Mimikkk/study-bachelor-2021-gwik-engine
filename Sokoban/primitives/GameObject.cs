#nullable enable
using Silk.NET.Maths;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives.components;

namespace Sokoban.primitives
{
    internal class GameObject : IRenderable, ITransform
    {
        public string Name { get; }
        public Vector3D<float> Position { get; set; }
        public float Scale { get; set; }
        public Quaternion<float> Rotation { get; set; }
        public Matrix4X4<float> View => ((ITransform) this).View;

        private Mesh Mesh { get; }
        private ShaderProgram Spo { get; set; }

        public GameObject(string name, Mesh mesh, ShaderProgram? spo = null)
        {
            Spo = spo ?? ShaderProgram.Default;
            Spo.Configuration = Configuration;
            Name = name;
            Mesh = mesh;
        }

        // This should be handled by renderer
        public void Render()
        {
            Renderer.Draw(Mesh.Vao, Spo);
        }

        private void Configuration()
        {
            // Mesh.Material.DiffuseTexture?.Bind(0);
            // Mesh.Material.ReflectionTexture?.Bind(1);
            Mesh.Material.AmbientTexture?.Bind(0);
            // Mesh.Material.NormalTexture?.Bind(3);
            // Mesh.Material.LightMapTexture?.Bind(4);
            // Mesh.Material.OpacityTexture?.Bind(5);

            //Setup the coordinate systems for our view
            Spo.SetUniform("u_Projection", Game.Camera.Projection);
            Spo.SetUniform("u_View", Game.Camera.View);
            Spo.SetUniform("u_Model", Matrix4X4<float>.Identity);

            //Let the shaders know where the Camera is looking from
            // Spo.SetUniform("viewPos", Game.Camera.Position);

            //Configure the materials variables.
            // Spo.SetUniform("material.diffuse", Mesh.Material.Opacity);
            // Spo.SetUniform("material.specular", Mesh.Material.Reflectivity);
            // Spo.SetUniform("material.shininess", Mesh.Material.Shininess);
            //
            // Spo.SetUniform("light.ambient", Mesh.Material.AmbientColor);
            // Spo.SetUniform("light.diffuse", Mesh.Material.DiffuseColor);
            // Spo.SetUniform("light.specular", Mesh.Material.SpecularColor);
            // Spo.SetUniform("light.position", Game.Camera.Position);
        }
    }
}
