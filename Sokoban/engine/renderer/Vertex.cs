using System.Numerics;

namespace GWiK_Sokoban.engine.objects
{
    internal class Vertex
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