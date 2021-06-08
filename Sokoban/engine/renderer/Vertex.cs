using System.Collections.Generic;
using Silk.NET.Maths;

namespace Sokoban.engine.renderer
{
internal readonly struct Vertex
{
    private Vector3D<float> Position { get; }
    public Vector2D<float> TextureCoordinate { get; }
    public Vector3D<float> Normal { get; }
    public Vector3D<float> Tangent { get; }
    public Vector3D<float> BiTangent { get; }

    public static ElementLayout ElementLayout = new(14, new[] {3, 3, 2, 3, 3});


    public Vertex(Vector3D<float> position, Vector2D<float> textureCoordinate, Vector3D<float> normal,
        Vector3D<float> tangent, Vector3D<float> biTangent)
    {
        Position = position;
        TextureCoordinate = textureCoordinate;
        Normal = normal;
        Tangent = tangent;
        BiTangent = biTangent;
    }


    public override string ToString()
    {
        return "TangentVertex("
               + $"{Position.X},{Position.Y},{Position.Z};"
               + $"{TextureCoordinate.X},{TextureCoordinate.Y};"
               + $"{Normal.X},{Normal.Y},{Normal.Z};"
               + $"{Tangent.X},{Tangent.Y},{Tangent.Z};"
               + $"{BiTangent.X},{BiTangent.Y},{BiTangent.Z})";
    }

    // [Pos,TexCords,Norm, Tan, BiTan] Format
    public IEnumerable<float> ToFloats()
    {
        return new[] {
            Position.X, Position.Y, Position.Z,
            TextureCoordinate.X, TextureCoordinate.Y,
            Normal.X, Normal.Y, Normal.Z,
            Tangent.X, Tangent.Y, Tangent.Z,
            BiTangent.X, BiTangent.Y, BiTangent.Z
        };
    }
}
}