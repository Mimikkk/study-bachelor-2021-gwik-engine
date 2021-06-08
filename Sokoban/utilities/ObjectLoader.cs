#nullable enable
using System.Collections.Generic;
using System.Linq;
using Assimp;
using Silk.NET.Maths;
using Sokoban.engine.objects;
using Sokoban.engine.renderer;
using Sokoban.primitives;
using Material = Sokoban.engine.objects.Material;
using Mesh = Sokoban.engine.objects.Mesh;
using Vector3D = Assimp.Vector3D;

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

    private static readonly PostProcessSteps PostProcess = PostProcessPreset.TargetRealTimeMaximumQuality
                                                           | PostProcessSteps.CalculateTangentSpace
                                                           | PostProcessSteps.JoinIdenticalVertices;

    public static IEnumerable<GameObject> Load(string name)
    {
        Initialize(name);
        Scene = Api.ImportFile(Filepath, PostProcess);
        LoadMaterials();
        LoadMeshes();
        Log();
        return Meshes.Select(m => new GameObject(m.Name, m));
    }

    public static void Log()
    {
        $"Loaded <c17 Scene|>: <c22{Scene.RootNode.Name}|>".LogLine();
        $"Number of <c15 Meshes|>: <c25 {Scene.MeshCount}|>".LogLine(2);
        $"Number of <c15 Materials|>: <c25 {Scene.MaterialCount}|>".LogLine(2);
        $"Loaded <c17 Meshes|>".LogLine(2);
        $"Loaded <c17 Materials|>".LogLine(2);
        foreach (var mesh in Meshes) mesh.Log(4);
        foreach (var material in Materials) material.Log(4);
    }

    private static void LoadMeshes()
        => Meshes.AddRange(Scene.Meshes.Select(ToMesh));
    private static void LoadMaterials()
        => Materials.AddRange(Scene.Materials.Select(ToMaterial));

    private static void Initialize(string name)
    {
        Materials.Clear();
        Meshes.Clear();
        Name = name;
    }


    private static Texture? ToTexture(this TextureSlot slot)
        => slot.FilePath != null ? new Texture(slot.FilePath.Split("\\\\").Last()) : null;


    private static Mesh ToMesh(Assimp.Mesh raw)
    {
        var positions = raw.Vertices.Select(ToVector3D);
        var textureCoordinates = raw.TextureCoordinateChannels[0].Select(ToVector2D);
        var normals = raw.Normals.Select(ToVector3D);
        var tangents = raw.Tangents.Select(ToVector3D);
        var biTangents = raw.BiTangents.Select(ToVector3D);

        var vertices = new List<Vertex>();
        foreach (var (pos, (tex, (norm, (tan, biTan))))
            in positions.Zip(textureCoordinates.Zip(normals.Zip(tangents.Zip(biTangents)))))
            vertices.Add(new Vertex(pos, tex, norm, tan, biTan));

        var indices = raw.Faces.SelectMany(face => face.Indices);

        return new Mesh(raw.Name, Materials[raw.MaterialIndex], vertices, indices);
    }
    private static Material ToMaterial(Assimp.Material raw) => new(raw.Name ?? "Unnamed") {
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
        DiffuseTexture = raw.TextureDiffuse.ToTexture() ?? Texture.Missing,
        DisplacementTexture = raw.TextureDisplacement.ToTexture(),
        EmissiveTexture = raw.TextureEmissive.ToTexture(),
        HeightTexture = raw.TextureHeight.ToTexture(),
        NormalTexture = raw.TextureNormal.ToTexture(),
        OpacityTexture = raw.TextureOpacity.ToTexture(),
        ReflectionTexture = raw.TextureReflection.ToTexture(),
        AmbientOcclusionTexture = raw.TextureAmbientOcclusion.ToTexture(),
        LightMapTexture = raw.TextureLightMap.ToTexture()
    };

    private static Vector3D<float> ToVector3D(Vector3D v) => new(v.X, v.Y, v.Z);
    private static Vector2D<float> ToVector2D(Vector3D v) => new(v.X, v.Y);
    private static Color ToColor(this Color4D color4D) => new(color4D.R, color4D.G, color4D.B, color4D.A);
}
}