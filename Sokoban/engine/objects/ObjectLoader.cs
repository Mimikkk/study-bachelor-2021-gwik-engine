#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Sokoban.engine.renderer;
using Sokoban.primitives;
using Sokoban.utilities;
using Path = Sokoban.utilities.Path;

namespace Sokoban.engine.objects
{
    internal static class ObjectLoader
    {
        private static List<GameObject> LoadedObjects { get; } = new();

        private static Path Objectpath => Path.Objects / $"{Name}.obj";

        private static string? Name { get; set; }

        public static IEnumerable<GameObject> MaybeLoad(string objectname)
        {
            Name = objectname;
            LoadedObjects.Clear();

            if (!Objectpath.IsFile())
            {
                Console.WriteLine($"ObjectLoader::Obj Load failed, Not a File: {objectname} at {Objectpath}");
                return LoadedObjects;
            }
            Load();
            return LoadedObjects;
        }

        private static void Load()
        {
            var materials = MaterialLibraryLoader.MaybeLoadLibrary(Name!).ToList();
            var meshes = MeshLoader.MaybeLoadMeshes(Name!).ToList();

            Dictionary<uint, Material> changeIndicesMaterials = new();
            foreach (var (i, name) in MeshLoader.MaterialChangeIndices)
            {
                var material = materials.Find(m => m.Name == name)!;
                Console.WriteLine(material.Info());
                changeIndicesMaterials[i] = material;
            }

            var o = new GameObject(meshes[0].Name, meshes[0], changeIndicesMaterials);
            LoadedObjects.Add(o);
        }

        private static class MeshLoader
        {
            public static IEnumerable<Mesh> MaybeLoadMeshes(string meshname)
            {
                Meshname = meshname;

                LoadedMeshes.Clear();
                if (!Meshpath.IsFile())
                {
                    Console.WriteLine($"ObjectLoader::Mesh Load failed: {Meshname} at {Meshpath}");
                    return LoadedMeshes;
                }

                _normalsOffset = 0;
                _positionOffset = 0;
                _textureCoordsOffset = 0;

                Reader = Meshpath.LoadFileToStream();

                LoadMeshes();
                return LoadedMeshes;
            }
            private static void LoadMeshes()
            {
                ClearMeshMeta();
                _normalsOffset = 0;
                _positionOffset = 0;
                _textureCoordsOffset = 0;

                string? line;
                while ((line = Reader?.ReadLine()) != null)
                {
                    var (identifier, contents) = SplitByIdentifierAndContents(line);
                    if (MapMeshIdentifiers(identifier) == MeshIdentifier.Object) LoadMesh(contents);
                }
            }
            private static void LoadMesh(string? meshname)
            {
                Console.WriteLine($"Loading Mesh: {meshname}");
                PrepareMetaData();
                uint currentFaceCount = 0;

                string? line;
                while ((line = Reader?.ReadLine()) != null)
                {
                    var (identifier, contents) = SplitByIdentifierAndContents(line);
                    if (contents == null) continue;

                    List<float> floats;
                    switch (MapMeshIdentifiers(identifier))
                    {
                        case MeshIdentifier.Object or MeshIdentifier.Group or MeshIdentifier.MaterialLibrary or null:
                            break;
                        case MeshIdentifier.VertexPosition:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            Positions.Add(new Vector3(floats[0], floats[1], floats[2]));
                            break;
                        case MeshIdentifier.VertexTexture:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            TextureCoords.Add(new Vector2(floats[0], floats[1]));
                            break;
                        case MeshIdentifier.VertexNormal:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            Normals.Add(new Vector3(floats[0], floats[1], floats[2]));
                            break;
                        case MeshIdentifier.Face:
                            ++currentFaceCount;
                            Vertices.AddRange(GenerateVertices(contents));
                            Indices.AddRange(new uint[] {0, 1, 2}.Select(i => (uint) (Vertices.Count - 3 + i)));
                            break;
                        case MeshIdentifier.SmoothShading:
                            SmoothShadingFlips.Add(currentFaceCount);
                            break;
                        case MeshIdentifier.UseMaterial:
                            MaterialChangeIndices.Add((currentFaceCount, contents));
                            break;
                    }

                    if (ShouldBreak()) break;
                }

                Console.WriteLine($"\t Loaded {Vertices.Count} Vertices");
                Console.WriteLine($"\t Loaded {Indices.Count} Indices");
                LoadedMeshes.Add(new Mesh(meshname, Vertices, Indices, new[] {(3, 8u), (2, 8u), (3, 8u)}));
            }
            private static void PrepareMetaData()
            {
                _normalsOffset += Normals.Count;
                _textureCoordsOffset += TextureCoords.Count;
                _positionOffset += Positions.Count;
                ClearMeshMeta();
            }
            private static void ClearMeshMeta()
            {
                MaterialChangeIndices.Clear();
                SmoothShadingFlips.Clear();

                Positions.Clear();
                TextureCoords.Clear();
                Normals.Clear();

                Vertices.Clear();
                Indices.Clear();
            }
            private static bool ShouldBreak()
            {
                return MapMeshIdentifiers(((char?) Reader?.Peek()).ToString()) == MeshIdentifier.Object;
            }

            private static IEnumerable<Vertex> GenerateVertices(string facestring)
            {
                var vertexStrings = facestring.Split(" ");

                var verts = new List<Vertex>();
                var hasNormalAlready = false;
                foreach (var vertexString in vertexStrings)
                {
                    var faceIndicesType = FindFaceIndicesType(vertexString);
                    var indices = vertexString.Split("/").Select(x => int.Parse(x) - 1).ToList();
                    var position = Positions[indices[0] - _positionOffset];
                    var normal = Vector3.Zero;
                    var textureCoordinate = Vector2.Zero;

                    switch (faceIndicesType)
                    {
                        case FaceIndicesType.Position:
                            hasNormalAlready = false;
                            break;
                        case FaceIndicesType.PositionTexture:
                            textureCoordinate = TextureCoords[indices[1] - _textureCoordsOffset];
                            hasNormalAlready = false;
                            break;
                        case FaceIndicesType.PositionTextureNormal:
                            textureCoordinate = TextureCoords[indices[1] - _textureCoordsOffset];
                            normal = Normals[indices[2] - _normalsOffset];
                            hasNormalAlready = true;
                            break;
                        case FaceIndicesType.PositionNormal:
                            normal = Normals[indices[1] - _normalsOffset];
                            hasNormalAlready = true;
                            break;
                        default: throw new ArgumentOutOfRangeException($"{faceIndicesType}");
                    }
                    verts.Add(new Vertex(position, textureCoordinate, normal));
                }

                return hasNormalAlready ? verts : HandleEmptyNormals(verts);
            }
            private static IEnumerable<Vertex> HandleEmptyNormals(List<Vertex> vertices)
            {
                var generatedNormal =
                    MathHelper.GenerateNormal(vertices[0].Position, vertices[1].Position, vertices[2].Position);
                vertices.ForEach(v => v.Normal = generatedNormal);
                return vertices;
            }

            #region [[Private Variables]]

            private static readonly List<Mesh> LoadedMeshes = new();

            private static readonly List<Vertex> Vertices = new();
            private static readonly List<uint> Indices = new();

            private static readonly List<Vector3> Positions = new();
            private static readonly List<Vector2> TextureCoords = new();
            private static readonly List<Vector3> Normals = new();

            private static int _positionOffset = 0;
            private static int _textureCoordsOffset = 0;
            private static int _normalsOffset = 0;

            public static readonly List<(uint, string)> MaterialChangeIndices = new();
            private static readonly List<uint> SmoothShadingFlips = new();

            private static string? Meshname { get; set; }
            private static Path Meshpath => Path.Objects / $"{Meshname}.obj";
            private static StreamReader? Reader { get; set; }

            #endregion

            private enum MeshIdentifier
            {
                Object, Group, SmoothShading,
                VertexPosition, VertexTexture, VertexNormal, Face,
                UseMaterial, MaterialLibrary
            }
            private static MeshIdentifier? MapMeshIdentifiers(string? identifier)
            {
                return identifier switch
                {
                    "o"      => MeshIdentifier.Object,
                    "g"      => MeshIdentifier.Group,
                    "v"      => MeshIdentifier.VertexPosition,
                    "vt"     => MeshIdentifier.VertexTexture,
                    "vn"     => MeshIdentifier.VertexNormal,
                    "f"      => MeshIdentifier.Face,
                    "s"      => MeshIdentifier.SmoothShading,
                    "usemtl" => MeshIdentifier.UseMaterial,
                    "mtllib" => MeshIdentifier.MaterialLibrary,
                    _        => null
                };
            }

            private enum FaceIndicesType
            {
                Position, PositionTexture, PositionTextureNormal, PositionNormal
            }
            private static FaceIndicesType FindFaceIndicesType(string face)
            {
                if (face.Contains("//")) return FaceIndicesType.PositionNormal;

                var indicesPerFace = face.Split("/").Length;
                return indicesPerFace switch
                {
                    1 => FaceIndicesType.Position,
                    2 => FaceIndicesType.PositionTexture,
                    3 => FaceIndicesType.PositionTextureNormal,
                    _ => throw new Exception("Unsupported number of faces")
                };
            }
        }

        private static class MaterialLibraryLoader
        {
            public static IEnumerable<Material> MaybeLoadLibrary(string materialibraryname)
            {
                MaterialLibraryname = materialibraryname;
                LoadedMaterials.Clear();

                if (!MaterialLibrarypath.IsFile())
                {
                    Console.WriteLine(
                        $"ObjectLoader::MatLib Load failed: {MaterialLibraryname} at {MaterialLibrarypath}");
                    return LoadedMaterials;
                }

                Reader = MaterialLibrarypath.LoadFileToStream();
                LoadLibrary();
                return LoadedMaterials;
            }

            private static void LoadLibrary()
            {
                string? line;
                while ((line = Reader?.ReadLine()) != null)
                {
                    var (identifier, contents) = SplitByIdentifierAndContents(line);
                    if (MapMaterialIdentifiers(identifier) == MaterialIdentifier.NewMaterial && contents != null)
                        LoadMaterial(contents);
                }
            }
            private static void LoadMaterial(string materialname)
            {
                var material = new Material(materialname);

                string? line;
                while ((line = Reader?.ReadLine()) != null)
                {
                    var (identifier, contents) = SplitByIdentifierAndContents(line);
                    if (identifier == null || contents == null) break;

                    List<float> floats;
                    switch (MapMaterialIdentifiers(identifier))
                    {
                        case MaterialIdentifier.NewMaterial: break;
                        case MaterialIdentifier.AmbientColor:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            material.AmbientColor = new Vector3(floats[0], floats[1], floats[2]);
                            break;
                        case MaterialIdentifier.DiffuseColor:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            material.DiffuseColor = new Vector3(floats[0], floats[1], floats[2]);
                            break;
                        case MaterialIdentifier.SpecularColor:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            material.SpecularColor = new Vector3(floats[0], floats[1], floats[2]);
                            break;
                        case MaterialIdentifier.EmissionColor:
                            floats = contents.Split(" ").Select(float.Parse).ToList();
                            material.EmissionColor = new Vector3(floats[0], floats[1], floats[2]);
                            break;
                        case MaterialIdentifier.SpecularExponent:
                            material.SpecularExponent = float.Parse(contents);
                            break;
                        case MaterialIdentifier.OpticalDensity:
                            material.OpticalDensity = float.Parse(contents);
                            break;
                        case MaterialIdentifier.Dissolve:
                            material.Dissolve = float.Parse(contents);
                            break;
                        case MaterialIdentifier.Illumination:
                            material.Illumination = float.Parse(contents);
                            break;
                        case MaterialIdentifier.AmbientTextureMap:
                            material.AmbientMap = new Texture(FindMapName(contents));
                            break;
                        case MaterialIdentifier.DiffuseTextureMap:
                            material.DiffuseMap = new Texture(FindMapName(contents));
                            break;
                        case MaterialIdentifier.SpecularTextureMap:
                            material.SpecularMap = new Texture(FindMapName(contents));
                            break;
                        case MaterialIdentifier.SpecularHighlightMap:
                            material.HighlightMap = new Texture(FindMapName(contents));
                            break;
                        case MaterialIdentifier.AlphaTextureMap:
                            material.AlphaMap = new Texture(FindMapName(contents));
                            break;
                        case MaterialIdentifier.BumpMap:
                            material.BumpMap = new Texture(FindMapName(contents));
                            break;
                        default: throw new ArgumentOutOfRangeException(nameof(materialname));
                    }
                }

                LoadedMaterials.Add(material);
            }

            private static readonly List<Material> LoadedMaterials = new();
            private static string? MaterialLibraryname { get; set; }
            private static Path MaterialLibrarypath => Path.Objects / $"{MaterialLibraryname}.mtl";
            private static StreamReader? Reader { get; set; }

            private enum MaterialIdentifier
            {
                NewMaterial,
                AmbientColor, DiffuseColor, SpecularColor, EmissionColor,
                SpecularExponent, OpticalDensity, Dissolve, Illumination,
                AmbientTextureMap, DiffuseTextureMap, SpecularTextureMap,
                SpecularHighlightMap, AlphaTextureMap, BumpMap,
            }
            private static MaterialIdentifier? MapMaterialIdentifiers(string? identifier)
            {
                return identifier switch
                {
                    "illum"  => MaterialIdentifier.Illumination,
                    "newmtl" => MaterialIdentifier.NewMaterial,
                    "Ka"     => MaterialIdentifier.AmbientColor,
                    "Kd"     => MaterialIdentifier.DiffuseColor,
                    "Ks"     => MaterialIdentifier.SpecularColor,
                    "Ke"     => MaterialIdentifier.EmissionColor,
                    "Ns"     => MaterialIdentifier.SpecularExponent,
                    "Ni"     => MaterialIdentifier.OpticalDensity,
                    "d"      => MaterialIdentifier.Dissolve,
                    "map_Ka" => MaterialIdentifier.AmbientTextureMap,
                    "map_Kd" => MaterialIdentifier.DiffuseTextureMap,
                    "map_Ks" => MaterialIdentifier.SpecularTextureMap,
                    "map_Ns" => MaterialIdentifier.SpecularHighlightMap,
                    "map_Ni" => MaterialIdentifier.AlphaTextureMap,
                    "bump"   => MaterialIdentifier.BumpMap,
                    _        => null
                };
            }
            private static string FindMapName(string contents)
            {
                return contents.Split("\\\\").Last();
            }
        }

        private static (string?, string?) SplitByIdentifierAndContents(string line)
        {
            var identifierAndContents = line.Split(" ", 2);

            return identifierAndContents.Length switch
            {
                0 => (null, null),
                1 => (identifierAndContents[0], null),
                _ => (identifierAndContents[0], identifierAndContents[1]),
            };
        }
    }
}