#nullable enable
using System;
using Assimp;
using Silk.NET.Maths;
using Sokoban.primitives;
using Sokoban.utilities;
using static Sokoban.utilities.FieldExtensions;
using Vector3D = Silk.NET.Maths.Vector3D;

namespace Sokoban.entities
{
    internal class QuaternionCamera : Frustum, ICamera
    {
        public void Log(int depth = 0)
        {
            $"Current Camera: <c19 {View}>".LogLine(depth);
            $"Position:\t<c25 {Position}>".LogLine(depth + 2);
            $"Left:\t{Left}".LogLine(depth + 2);
            $"Up:\t\t{Up}".LogLine(depth + 2);
            $"Forward:\t{Forward}".LogLine(depth + 2);
            $"Roll/Pitch/Yaw | Local:\t{Rotation.ToRollPitchYaw()}".LogLine(depth + 2);
        }

        public QuaternionCamera(Vector3D<float> position, float fieldOfView, float aspectRatio, float nearDistance,
            float farDistance) : base(fieldOfView, aspectRatio, nearDistance, farDistance)
        {
            Fov = fieldOfView;
            AspectRatio = aspectRatio;
            NearDistance = nearDistance;
            FarDistance = farDistance;

            Position = position;
            Rotation = Quaternion<float>.Identity;
        }
        public Matrix4X4<float> View
            => Matrix4X4.Transform(Matrix4X4.CreateTranslation(-Position), Rotation);
        public Vector3D<float> Position { get; set; }
        public Quaternion<float> Rotation { get; set; }

        public void Rotate(float angle, Vector3D<float> axis)
        {
            var rotation = Quaternion<float>.CreateFromAxisAngle(axis, angle);
            Rotate(rotation);
        }
        public void Rotate(Quaternion<float> rotation) { Rotation = rotation * Rotation; }

        public void Turn(float angle)
        {
            var rotation = Quaternion<float>.CreateFromAxisAngle(Matrix3X3.CreateFromQuaternion(Rotation).Row2, angle);
            Rotate(rotation);
        }
        public Vector3D<float> Left
            => Vector3D.Negate(Matrix3X3.CreateFromQuaternion(Quaternion<float>.Conjugate(Rotation)).Row1);
        public Vector3D<float> Up
            => Matrix3X3.CreateFromQuaternion(Quaternion<float>.Conjugate(Rotation)).Row2;
        public Vector3D<float> Forward
            => Vector3D.Negate(Matrix3X3.CreateFromQuaternion(Quaternion<float>.Conjugate(Rotation)).Row3);

        public void Translate(Vector3D<float> translation) { Position += translation; }
        public void TranslateLocal(Vector3D<float> translation)
        {
            Position += translation.X * Left + translation.Y * Up + translation.Z * Forward;
        }
        public void LookAt(Vector3D<float> target)
        {
            var m = Matrix4X4.CreateLookAt(Position, new Vector3D<float>(target.X, target.Y, target.Z), Up);
            Rotation = Quaternion<float>.CreateFromRotationMatrix(m);
        }
    }

    internal abstract class Frustum
    {
        protected Frustum(float fieldOfView, float aspectRatio, float nearDistance, float farDistance)
        {
            Fov = fieldOfView;
            AspectRatio = aspectRatio;
            NearDistance = nearDistance;
            FarDistance = farDistance;
        }

        private void RecalculateProjection()
        {
            ShouldRecalculateProjection = false;
            _projection = Matrix4X4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio,
                NearDistance, FarDistance);
        }

        public Matrix4X4<float> Projection
        {
            get => OperationIfConditionAndGet(_projection, ShouldRecalculateProjection, RecalculateProjection);
            set => _projection = value;
        }
        public float Fov
        {
            get => _fov;
            set => SetAndOperation(() => _fov = value, () => ShouldRecalculateProjection = true);
        }
        public float AspectRatio
        {
            get => _aspectRatio;
            set => SetAndOperation(() => _aspectRatio = value, () => ShouldRecalculateProjection = true);
        }
        public float NearDistance
        {
            get => _nearDistance;
            set => SetAndOperation(() => _nearDistance = value, () => ShouldRecalculateProjection = true);
        }
        public float FarDistance
        {
            get => _farDistance;
            set => SetAndOperation(() => _farDistance = value, () => ShouldRecalculateProjection = true);
        }

        private bool ShouldRecalculateProjection { get; set; }
        private Matrix4X4<float> _projection;
        private float _fov;
        private float _aspectRatio;
        private float _nearDistance;
        private float _farDistance;
    }

    internal static class QuaternionExtensions
    {
        public static Vector3D<float> ToRollPitchYaw(this Quaternion<float> quaternion)
        {
            var x = quaternion.X;
            var y = quaternion.Y;
            var z = quaternion.Z;
            var w = quaternion.W;
            var roll = MathHelper.RadiansToDegrees(MathF.Atan2(2.0f * (x * y + w * z), w * w + x * x - y * y - z * z));
            var pitch = MathHelper.RadiansToDegrees(MathF.Asin(-2.0f * (x * z - w * y)));
            var yaw = MathHelper.RadiansToDegrees(MathF.Atan2(2.0f * (y * z + w * x), w * w - x * x - y * y + z * z));
            return new Vector3D<float>(roll, pitch, yaw);
        }
    }
}