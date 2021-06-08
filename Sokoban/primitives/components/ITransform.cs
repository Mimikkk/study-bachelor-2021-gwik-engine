using Silk.NET.Maths;
using Sokoban.utilities;

namespace Sokoban.primitives.components
{
    public interface ITransform : IHasLog
    {
        public Vector3D<float> Position { get; set; }
        public Quaternion<float> Rotation { get; set; }
        public float Scale { get; set; }

        public Matrix4X4<float> View => Matrix4X4.Transform(Matrix4X4.CreateTranslation(-Position), Rotation);

        public void Rotate(float angle, Vector3D<float> axis)
            => Rotate(Quaternion<float>.CreateFromAxisAngle(axis, angle));
        public void Rotate(Quaternion<float> rotation) => Rotation = rotation * Rotation;

        public Vector3D<float> Left
            => Vector3D.Negate(Matrix3X3.CreateFromQuaternion(Quaternion<float>.Conjugate(Rotation)).Row1);
        public Vector3D<float> Up
            => Matrix3X3.CreateFromQuaternion(Quaternion<float>.Conjugate(Rotation)).Row2;
        public Vector3D<float> Forward
            => Vector3D.Negate(Matrix3X3.CreateFromQuaternion(Quaternion<float>.Conjugate(Rotation)).Row3);

        public void Turn(float angle)
            => Rotate(Quaternion<float>.CreateFromAxisAngle(Matrix3X3.CreateFromQuaternion(Rotation).Row2, angle));


        public void Translate(Vector3D<float> translation) => Position += translation;
        public void TranslateLocal(Vector3D<float> translation)
            => Position += translation.X * Left + translation.Y * Up + translation.Z * Forward;

        public new void Log(int depth = 0)
        {
            $"Current Camera: <c19 {View}|>".LogLine(depth);
            $"Position:\t<c25 {Position}|>".LogLine(depth + 2);
            $"Left:\t{Left}".LogLine(depth + 2);
            $"Up:\t\t{Up}".LogLine(depth + 2);
            $"Forward:\t{Forward}".LogLine(depth + 2);
            $"Roll/Pitch/Yaw | Local:\t{Rotation.ToRollPitchYaw()}".LogLine(depth + 2);
        }
    }
}