using Silk.NET.Maths;

namespace Sokoban.primitives.components
{
    public interface ITransform
    {
        public Vector3D<float> Position { get; set; }
        public float Scale { get; set; }
        public Quaternion<float> Rotation { get; set; }

        public Matrix4X4<float> View => Matrix4X4<float>.Identity * Matrix4X4.CreateFromQuaternion(Rotation)
                                                                        * Matrix4X4.CreateScale(Scale)
                                                                        * Matrix4X4.CreateTranslation(Position);
    }
}
