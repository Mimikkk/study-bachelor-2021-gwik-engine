#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Assimp;
using Sokoban.engine.renderer;

namespace Sokoban.utilities
{
    internal class Mesh : IHasLog
    {
        private string Name { get; }
        public readonly List<Vertex> Vertices;
        public readonly Material Material;
        public List<int> Indices { get; set; }

        public Mesh(string name, IEnumerable<Vertex> vertices, IEnumerable<int> indices, Material material)
        {
            Name = name;
            Material = material;
            Indices = indices.ToList();
            Vertices = vertices.ToList();
        }

        public void Log(int depth = 0)
        {
            $"Mesh: <c20 {Name}>".LogLine(depth);
            $"Number of Vertices: <c22 {Vertices.Count}>".LogLine(depth + 2);
            $"Number of Indices: <c22 {Indices.Count}>".LogLine(depth + 2);
        }
    }

    internal class GameObject
    {
    }

    internal class Material : IHasLog
    {
        public Material(string name)
        {
            Name = name;
        }
        public void Log(int depth = 0)
        {
            $"<c20 Material>: <c22 {Name}>".LogLine(depth);
            $"<c88 Values>".LogLine(2 + depth);
            $"<c70 Opacity>: {Opacity}".LogLine(4 + depth);
            $"<c70 Reflectivity>: {Reflectivity}".LogLine(4 + depth);
            $"<c70 Shininess>: {Shininess}".LogLine(4 + depth);
            $"<c70 BumpScaling>: {BumpScaling}".LogLine(4 + depth);
            $"<c70 ShininessStrength>: {ShininessStrength}".LogLine(4 + depth);
            $"<c70 TransparencyFactor>: {TransparencyFactor}".LogLine(4 + depth);
            $"<c88 Colors>".LogLine(2 + depth);
            $"<c70 Ambient>: {AmbientColor}".LogLine(4 + depth);
            $"<c70 Diffuse>: {DiffuseColor}".LogLine(4 + depth);
            $"<c70 Emissive>: {EmissiveColor}".LogLine(4 + depth);
            $"<c70 Reflective>: {ReflectiveColor}".LogLine(4 + depth);
            $"<c70 Specular>: {SpecularColor}".LogLine(4 + depth);
            $"<c70 Transparent>: {TransparentColor}".LogLine(4 + depth);
            $"<c88 Maps>".LogLine(2 + depth);
            $"<c70 AmbientMap>: {AmbientTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 DiffuseMap>: {DiffuseTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 DisplacementMap>: {DisplacementTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 EmissiveMap>: {EmissiveTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 HeightMap>: {HeightTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 NormalMap>: {NormalTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 OpacityMap>: {OpacityTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 ReflectionMap>: {ReflectionTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 AmbientOcclusionMap>: {AmbientOcclusionTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
            $"<c70 LightMap>: {LightMapTexture?.Name ?? "<c8 None>"}".LogLine(4 + depth);
        }
        public string Name { get; set; }

        public float Opacity { get; set; } = 1;
        public float Reflectivity { get; set; } = 0;
        public float Shininess { get; set; } = 0;
        public float ShininessStrength { get; set; } = 1;
        public float BumpScaling { get; set; } = 0;
        public float TransparencyFactor { get; set; } = 1f;

        public Texture? AmbientTexture { get; set; } = null;
        public Texture? DiffuseTexture { get; set; } = null;
        public Texture? DisplacementTexture { get; set; } = null;
        public Texture? EmissiveTexture { get; set; } = null;
        public Texture? HeightTexture { get; set; } = null;
        public Texture? NormalTexture { get; set; } = null;
        public Texture? OpacityTexture { get; set; } = null;
        public Texture? ReflectionTexture { get; set; } = null;
        public Texture? AmbientOcclusionTexture { get; set; } = null;
        public Texture? LightMapTexture { get; set; } = null;

        public Color AmbientColor { get; set; } = Color.Gray;
        public Color DiffuseColor { get; set; } = Color.White;
        public Color EmissiveColor { get; set; } = Color.Black;
        public Color ReflectiveColor { get; set; } = Color.Black;
        public Color SpecularColor { get; set; } = Color.Black;
        public Color TransparentColor { get; set; } = Color.White;
    }
    internal struct Color
    {
        public static readonly Color Black = new(0, 0, 0, 1);
        public static readonly Color White = new(1, 1, 1, 1);
        public static readonly Color Gray = new(0.2f, 0.2f, 0.2f, 1);

        public Color(float r = 0, float g = 0, float b = 0, float a = 1)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public override string ToString()
        {
            return $"Color(<c9 {R:F2}> <c10 {G:F2}> <c12 {B:F2}> <c15 {A:F2}>)";
        }
    }

    internal static class ObjectLoader
    {
        private static AssimpContext Api { get; } = new();
        private static string Name { get; set; } = null!;
        private static Scene Scene { get; set; } = null!;
        private static string Filepath => $"{Path.Objects / Name}.obj";
        private static readonly List<Mesh> Meshes = new();
        private static readonly List<Material> Materials = new();
        private static readonly PostProcessSteps PostProcess =
            PostProcessPreset.TargetRealTimeMaximumQuality | PostProcessSteps.Triangulate;

        public static GameObject LoadObject(string name)
        {
            Materials.Clear();
            Meshes.Clear();

            Name = name;
            Scene = Api.ImportFile(Filepath, PostProcess);
            LoadMaterials();
            LoadMeshes();


            $"Loaded <c17 Scene>: <c22{Scene.RootNode.Name}>".LogLine();
            $"Number of <c15 Meshes>: <c25 {Scene.MeshCount}>".LogLine(2);
            $"Number of <c15 Materials>: <c25 {Scene.MaterialCount}>".LogLine(2);
            $"Loaded <c17 Meshes>".LogLine(2);
            $"Loaded <c17 Materials>".LogLine(2);
            foreach (var mesh in Meshes) mesh.Log(4);
            foreach (var material in Materials) material.Log(4);
            $"Nothing <c9 Loaded>".LogLine();
            $"{string.Join(' ', Meshes[0].Vertices)}".LogLine();
            return new GameObject();
        }
        private static void LoadMeshes()
        {
            Meshes.AddRange(Scene.Meshes.Select(raw => new Mesh(raw.Name, raw.Vertices
                    .Select(p => new Vector3(p.X, p.Y, p.Z))
                    .Zip(raw.TextureCoordinateChannels[0]
                        .Select(vec => new Vector2(vec.X, vec.Y)))
                    .Zip(raw.Normals.Select(n => new Vector3(n.X, n.Y, n.Z)))
                    .Select(ptn => new Vertex(ptn.First.First, ptn.First.Second, ptn.Second)),
                raw.Faces.SelectMany(face => face.Indices), Materials[raw.MaterialIndex])));
        }
        private static void LoadMaterials()
        {
            Materials.AddRange(Scene.Materials.Select(raw => new Material(raw.Name ?? "Unnamed")
            {
                Opacity = raw.Opacity,
                Reflectivity = raw.Reflectivity,
                Shininess = raw.Shininess,
                ShininessStrength = raw.ShininessStrength,
                BumpScaling = raw.BumpScaling,
                TransparencyFactor = raw.TransparencyFactor,
                AmbientColor = raw.ColorAmbient.ToColor(),
                DiffuseColor = raw.ColorDiffuse.ToColor(),
                EmissiveColor = raw.ColorEmissive.ToColor(),
                ReflectiveColor = raw.ColorReflective.ToColor(),
                SpecularColor = raw.ColorSpecular.ToColor(),
                TransparentColor = raw.ColorTransparent.ToColor(),
                AmbientTexture = raw.TextureAmbient.ToTexture(),
                DiffuseTexture = raw.TextureDiffuse.ToTexture(),
                DisplacementTexture = raw.TextureDisplacement.ToTexture(),
                EmissiveTexture = raw.TextureEmissive.ToTexture(),
                HeightTexture = raw.TextureHeight.ToTexture(),
                NormalTexture = raw.TextureNormal.ToTexture(),
                OpacityTexture = raw.TextureOpacity.ToTexture(),
                ReflectionTexture = raw.TextureReflection.ToTexture(),
                AmbientOcclusionTexture = raw.TextureAmbientOcclusion.ToTexture(),
                LightMapTexture = raw.TextureLightMap.ToTexture()
            }).ToList());
        }
        private static Color ToColor(this Color4D color4D)
        {
            return new Color(color4D.R, color4D.G, color4D.B, color4D.A);
        }
        private static Texture? ToTexture(this TextureSlot slot)
        {
            return slot.FilePath == null ? null : new Texture(slot.FilePath.Split("\\\\").Last());
        }
    }
}
