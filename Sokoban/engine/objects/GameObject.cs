using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GWiK_Sokoban.engine.interfaces;
using GWiK_Sokoban.engine.renderer;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using Shader = GWiK_Sokoban.engine.renderer.Shader;

namespace GWiK_Sokoban.engine.objects
{
    public class GameObject : IRenderable, ITransform
    {
        public string Name { get; }
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Matrix4x4 ViewMatrix => ((ITransform) this).ViewMatrix;

        private Mesh Mesh { get; }

        private Material CurrentMaterial { get; }
        private Dictionary<uint, Material> Materials { get; }
        private ShaderProgram Spo { get; set; }

        public static Camera Camera { get; set; }

        internal GameObject(string name, Mesh mesh,
            Dictionary<uint, Material> changeIndicesMaterials = null, ShaderProgram spo = null)
        {
            Spo = spo ?? ShaderProgram.Default;
            Name = name;
            Mesh = mesh;
            Materials = changeIndicesMaterials ?? new Dictionary<uint, Material> {{0, Material.Default}};
            CurrentMaterial = Materials[0];
        }

        public bool IsConfigured { get; set; }

        public void Render()
        {
            Renderer.Draw(Mesh.Vao, Spo);
        }

        public void ConfigureShaderProgram(Action configuration = null)
        {
            Spo.Configuration = configuration ?? DefaultConfiguration;
            IsConfigured = true;
        }

        private void DefaultConfiguration()
        {
            CurrentMaterial.DiffuseMap?.Bind(0);
            CurrentMaterial.SpecularMap?.Bind(1);
            CurrentMaterial.AmbientMap?.Bind(2);
            CurrentMaterial.BumpMap?.Bind(3);
            CurrentMaterial.HighlightMap?.Bind(4);
            CurrentMaterial.AlphaMap?.Bind(5);

            //Setup the coordinate systems for our view
            Spo.SetUniform("u_Projection", Camera.ProjectionMatrix);
            Spo.SetUniform("u_View", Camera.ViewMatrix);
            Spo.SetUniform("u_Model", Matrix4x4.Identity);

            //Let the shaders know where the Camera is looking from
            Spo.SetUniform("viewPos", Camera.Position);

            //Configure the materials variables.
            Spo.SetUniform("material.diffuse", CurrentMaterial.Dissolve);
            Spo.SetUniform("material.specular", CurrentMaterial.OpticalDensity);
            Spo.SetUniform("material.shininess", CurrentMaterial.SpecularExponent);

            Spo.SetUniform("light.ambient", CurrentMaterial.AmbientColor);
            Spo.SetUniform("light.diffuse", CurrentMaterial.DiffuseColor);
            Spo.SetUniform("light.specular", CurrentMaterial.SpecularColor);
            Spo.SetUniform("light.position", Camera.Position);
        }
        public void DefaultInitialization(Camera camera = null, Action configuration = null)
        {
            Camera = camera ?? Game.Camera;
            ConfigureShaderProgram(configuration);
        }
    }
}