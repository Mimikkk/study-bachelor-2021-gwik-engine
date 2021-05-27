using System.Numerics;

namespace Sokoban.primitives.components
{
    public interface ITransform
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public Quaternion Rotation { get; set; }

        public Matrix4x4 ViewMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation)
                                                          * Matrix4x4.CreateScale(Scale)
                                                          * Matrix4x4.CreateTranslation(Position);
    }
}