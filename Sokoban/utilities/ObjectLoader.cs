#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Assimp;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Material = Sokoban.engine.objects.Material;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Mesh = Sokoban.engine.objects.Mesh;
using Quaternion = System.Numerics.Quaternion;

namespace Sokoban.utilities
{
    internal static class ObjectLoader
    {
        private static AssimpContext Api { get; } = new();
        private static string Name { get; set; } = null!;
        private static Scene Scene { get; set; } = null!;
        private static string Filepath => $"{Path.Objects / Name}.obj";
        private static readonly List<Mesh> Meshes = new();
        private static readonly List<Material> Materials = new();
        private const PostProcessSteps PostProcess =
            PostProcessSteps.Triangulate
            | PostProcessSteps.GenerateNormals
            | PostProcessSteps.GenerateUVCoords
            | PostProcessSteps.ImproveCacheLocality
            | PostProcessSteps.OptimizeGraph
            | PostProcessSteps.OptimizeMeshes;

        public static IEnumerable<GameObject> Load(string name)
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
            return Meshes.Select(m => new GameObject(m.Name, m));
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
