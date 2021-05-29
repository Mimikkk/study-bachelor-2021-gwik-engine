using System.Collections.Generic;
using System.Numerics;
using Silk.NET.Maths;

namespace Sokoban.engine.renderer
{
    internal struct Vertex
    {
        public Vector3D<float> Position { get; set; }
        public Vector2D<float> TextureCoordinate { get; set; }
        public Vector3D<float> Normal { get; set; }

        public Vertex(Vector3D<float> position, Vector2D<float> textureCoordinate, Vector3D<float> normal)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
            Normal = normal;
        }


        public override string ToString()
        {
            return $"Vertex({Position.X},{Position.Y},{Position.Z};" +
                   $"{TextureCoordinate.X},{TextureCoordinate.Y};" +
                   $"{Normal.X},{Normal.Y},{Normal.Z})";
        }
        // [Pos,TexCords,Norm] Format
        public IEnumerable<float> ToFloats()
        {
            return new[]
            {
                Position.X, Position.Y, Position.Z,
                TextureCoordinate.X, TextureCoordinate.Y,
                Normal.X, Normal.Y, Normal.Z
            };
        }
    }
}
