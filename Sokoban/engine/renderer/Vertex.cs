using System.Numerics;

namespace Sokoban.engine.renderer
{
    internal struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector2 TextureCoordinate { get; set; }
        public Vector3 Normal { get; set; }

        public Vertex(Vector3 position, Vector2 textureCoordinate, Vector3 normal)
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
        public float[] ToFloats()
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
