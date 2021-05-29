#nullable enable
using System;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.engine.input;
using Sokoban.utilities;
using static Sokoban.utilities.FieldExtensions;

namespace Sokoban.primitives
{
    internal class QuaternionCamera : ICamera
    {
        public QuaternionCamera(Vector3D<float> position, float fieldOfView, float aspectRatio, float nearDistance,
            float farDistance)
        {
            _position = position;
            _fov = fieldOfView;
            _aspectRatio = aspectRatio;
            _nearDistance = nearDistance;
            _farDistance = farDistance;

            _left = new Vector3D<float>(-1, 0, 0);
            _up = new Vector3D<float>(0, 1, 0);
            _forward = new Vector3D<float>(0, 0, -1);

            RecalculateProjection();
            RecalculateView();
        }

        private void RegisterRotation()
        {
            if (++RotationHitCount <= RotationHitCountMax) return;
            RotationHitCount = 0;
            NormalizeCamera();
        }
        public void NormalizeCamera()
        {
            Left = Vector3D.Normalize(Left);
            Up = Vector3D.Normalize(Up);
            Forward = Vector3D.Normalize(Forward);
            Orientation = Quaternion<float>.Normalize(Orientation);

            Left = Vector3D.Cross(Up, Forward);
            Up = Vector3D.Cross(Forward, Left);
        }
        private void RecalculateView()
        {
            View = Matrix4X4.CreateFromQuaternion(Orientation);
            _view.Row4 = Matrix4X4.Multiply(new Vector4D<float>(Vector3D.Negate(_position), 1), _view);
            ShouldRecalculateView = false;
        }

        public Vector3D<float> Position
        {
            get => _position;
            set => SetAndOperation(ref _position, value, () => ShouldRecalculateView = true);
        }

        public Vector3D<float> Left
        {
            get => _left;
            set => SetAndOperation(ref _left, value, () => ShouldRecalculateView = true);
        }
        public Vector3D<float> Up
        {
            get => _up;
            set => SetAndOperation(ref _up, value, () => ShouldRecalculateView = true);
        }
        public Vector3D<float> Forward
        {
            get => _forward;
            set => SetAndOperation(ref _forward, value, () => ShouldRecalculateView = true);
        }

        public Quaternion<float> Orientation { get; set; }
        public Matrix4X4<float> View
        {
            get => OperationIfConditionAndGet(ref _view, ShouldRecalculateView, RecalculateView);
            set => _view = value;
        }

        public void Roll(float angle)
        {
            var angleAxis = Quaternion<float>.CreateFromAxisAngle(Forward, angle);
            Up = Vector3D.Transform(Up, angleAxis);
            Left = Vector3D.Transform(Left, angleAxis);
            Orientation = angleAxis * Orientation;

            RegisterRotation();
            ShouldRecalculateView = true;
        }
        public void Pitch(float angle)
        {
            var angleAxis = Quaternion<float>.CreateFromAxisAngle(-Left, angle);
            Up = Vector3D.Transform(Up, angleAxis);
            Forward = Vector3D.Transform(Forward, angleAxis);
            Orientation = angleAxis * Orientation;

            RegisterRotation();
            ShouldRecalculateView = true;
        }
        public void Yaw(float angle)
        {
            var angleAxis = Quaternion<float>.CreateFromAxisAngle(Up, angle);
            Left = Vector3D.Transform(Left, angleAxis);
            Forward = Vector3D.Transform(Forward, angleAxis);
            Orientation = angleAxis * Orientation;

            RegisterRotation();
            ShouldRecalculateView = true;
        }
        public void Rotate(float angle, Vector3D<float> axis)
        {
            var normal = Vector3D.Normalize(axis);
            var angleAxis = Quaternion<float>.CreateFromAxisAngle(normal, angle);

            Left = Vector3D.Transform(Left, angleAxis);
            Up = Vector3D.Transform(Up, angleAxis);
            Forward = Vector3D.Transform(Forward, angleAxis);
            Orientation = angleAxis * Orientation;

            RegisterRotation();
            ShouldRecalculateView = true;
        }

        public void Translate(Vector3D<float> translation)
        {
            Position += translation;
            ShouldRecalculateView = true;
        }

        public void TranslateLocal(Vector3D<float> translation)
        {
            Position += translation.X * Left;
            Position += translation.Y * Up;
            Position += translation.Z * Forward;

            ShouldRecalculateView = true;
        }
        private void RecalculateProjection()
        {
            Projection = Matrix4X4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(Fov), AspectRatio, NearDistance, FarDistance);
        }


        public Controller CameraController { get; } = new();
        public void ModifyZoom(float zoomAmount)
        {
            Fov = Math.Clamp(Fov - 5 * zoomAmount, 70, 90);
        }

        private ushort RotationHitCount { get; set; }
        private const ushort RotationHitCountMax = 1000;

        public Matrix4X4<float> Projection { get; set; }
        public float FarDistance
        {
            get => _farDistance;
            set => SetAndOperation(ref _farDistance, value, RecalculateProjection);
        }
        public float NearDistance
        {
            get => _nearDistance;
            set => SetAndOperation(ref _nearDistance, value, RecalculateProjection);
        }
        public float AspectRatio
        {
            get => _aspectRatio;
            set => SetAndOperation(ref _aspectRatio, value, RecalculateProjection);
        }
        public float Fov
        {
            get => _fov;
            set => SetAndOperation(ref _fov, value, RecalculateProjection);
        }

        private float _farDistance;
        private float _nearDistance;
        private float _aspectRatio;
        private float _fov;

        private Vector3D<float> _position;
        private Vector3D<float> _left;
        private Vector3D<float> _up;
        private Vector3D<float> _forward;
        private Matrix4X4<float> _view;
        private bool ShouldRecalculateView { get; set; }
    }
}
