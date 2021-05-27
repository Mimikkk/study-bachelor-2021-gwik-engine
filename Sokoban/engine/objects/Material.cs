#nullable enable
using System.Numerics;
using GWiK_Sokoban.engine.interfaces;
using GWiK_Sokoban.engine.renderer;
using Silk.NET.OpenGL;
using Shader = GWiK_Sokoban.engine.renderer.Shader;

namespace GWiK_Sokoban.engine.objects
{
    internal class Material : IHasInfo
    {
        public override string ToString()
        {
            return $"Material: {Name}";
        }

        public string Info()
        {
            return $"Material '{Name}' Properties:\n"
                   + $"\tAmbient Color: {AmbientColor.X} {AmbientColor.Y} {AmbientColor.Z}\n"
                   + $"\tDiffuse Color: {DiffuseColor.X} {DiffuseColor.Y} {DiffuseColor.Z}\n"
                   + $"\tSpecular Color: {SpecularColor.X} {SpecularColor.Y} {SpecularColor.Z}\n"
                   + $"\tEmission Color: {EmissionColor.X} {EmissionColor.Y} {EmissionColor.Z}\n"
                   + $"\tOptical Density: {OpticalDensity}\n"
                   + $"\tDissolve: {Dissolve}\n"
                   + $"\tIllumination: {Illumination}\n"
                   + $"\tSpecular Exponent: {SpecularExponent}\n"
                   + $"\tTextures:\n"
                   + (AmbientMap != null ? $"\t\tAmbientMap: {AmbientMap.Name}\n" : "")
                   + (DiffuseMap != null ? $"\t\tAmbientMap: {DiffuseMap.Name}\n" : "")
                   + (SpecularMap != null ? $"\t\tAmbientMap: {SpecularMap.Name}\n" : "")
                   + (HighlightMap != null ? $"\t\tAmbientMap: {HighlightMap.Name}\n" : "")
                   + (AlphaMap != null ? $"\t\tAmbientMap: {AlphaMap.Name}\n" : "")
                   + (BumpMap != null ? $"\t\tAmbientMap: {BumpMap.Name}\n" : "");
        }

        public string Name { get; }

        public Vector3 AmbientColor { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public Vector3 SpecularColor { get; set; }
        public Vector3 EmissionColor { get; set; }

        public float OpticalDensity { get; set; }
        public float Dissolve { get; set; }
        public float Illumination { get; set; }
        public float SpecularExponent { get; set; }

        public Texture? AmbientMap { get; set; }
        public Texture? DiffuseMap { get; set; }
        public Texture? SpecularMap { get; set; }
        public Texture? HighlightMap { get; set; }
        public Texture? AlphaMap { get; set; }
        public Texture? BumpMap { get; set; }

        public Material(string name)
        {
            Name = name;
        }

        public static readonly Material Default = new("Default")
        {
            AmbientColor = Vector3.Zero,
            DiffuseColor = Vector3.Zero,
            EmissionColor = Vector3.Zero,
            SpecularColor = Vector3.Zero,
            OpticalDensity = 1,
            SpecularExponent = 1,
            Dissolve = 2,
            Illumination = 2,
            AlphaMap = null,
            DiffuseMap = new Texture("MissingTexture.png"),
            HighlightMap = null,
            AmbientMap = null,
            BumpMap = null,
            SpecularMap = null,
        };
    }
}