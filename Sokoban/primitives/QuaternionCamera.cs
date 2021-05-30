#nullable enable
using Silk.NET.Maths;
using Sokoban.utilities;
using static Sokoban.utilities.FieldExtensions;

namespace Sokoban.primitives
{
    internal class QuaternionCamera : ICamera, IHasLog
    {
        public QuaternionCamera(Vector3D<float> position, float fieldOfView, float aspectRatio, float nearDistance,
            float farDistance)
        {
            Fov = fieldOfView;
            AspectRatio = aspectRatio;
            NearDistance = nearDistance;
            FarDistance = farDistance;

            Position = position;
            Left = new Vector3D<float>(-1, 0, 0);
            Up = new Vector3D<float>(0, 1, 0);
            Forward = new Vector3D<float>(0, 0, -1);
            Orientation = Quaternion<float>.CreateFromRotationMatrix(new Matrix3X3<float>(-Left, Up, -Forward));
        }

        public Matrix4X4<float> Projection
        {
            get => OperationIfConditionAndGet(_projection, ShouldRecalculateProjection, RecalculateProjection);
            set => _projection = value;
        }
        public float Fov
        {
            get => _fov;
            set => SetAndOperation(ref _fov, value, () => ShouldRecalculateProjection = true);
        }
        public float AspectRatio
        {
            get => _aspectRatio;
            set => SetAndOperation(ref _aspectRatio, value, () => ShouldRecalculateProjection = true);
        }
        public float NearDistance
        {
            get => _nearDistance;
            set => SetAndOperation(ref _nearDistance, value, () => ShouldRecalculateProjection = true);
        }
        public float FarDistance
        {
            get => _farDistance;
            set => SetAndOperation(ref _farDistance, value, () => ShouldRecalculateProjection = true);
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
        public Matrix4X4<float> View
        {
            get => OperationIfConditionAndGet(_view, ShouldRecalculateView, RecalculateView);
            set => _view = value;
        }
        public Quaternion<float> Orientation
        {
            get => _orientation;
            set => SetAndOperation(ref _orientation, value, RegisterRotation);
        }

        public void RotateForward(float angle)
        {
            var rotation = Quaternion<float>.CreateFromAxisAngle(Forward, angle);
            Orientation = rotation * Orientation;

            Left = Vector3D.Transform(Left, rotation);
            Up = Vector3D.Transform(Up, rotation);
        }
        public void RotateLeft(float angle)
        {
            var rotation = Quaternion<float>.CreateFromAxisAngle(-Left, angle);
            Orientation = rotation * Orientation;

            Up = Vector3D.Transform(Up, rotation);
            Forward = Vector3D.Transform(Forward, rotation);
        }
        public void RotateUp(float angle)
        {
            var rotation = Quaternion<float>.CreateFromAxisAngle(Up, angle);
            Orientation = rotation * Orientation;

            Left = Vector3D.Transform(Left, rotation);
            Forward = Vector3D.Transform(Forward, rotation);
        }
        public void Rotate(float angle, Vector3D<float> axis)
        {
            var rotation = Quaternion<float>.CreateFromAxisAngle(Vector3D.Normalize(axis), angle);
            Orientation = rotation * Orientation;

            Left = Vector3D.Transform(Left, rotation);
            Up = Vector3D.Transform(Up, rotation);
            Forward = Vector3D.Transform(Forward, rotation);
        }

        public void Translate(Vector3D<float> translation)
        {
            Position += translation;
        }
        public void TranslateLocal(Vector3D<float> translation)
        {
            Position += translation.X * Left + translation.Y * Up + translation.Z * Forward;
        }

        public void LookAt(Vector3D<float> center)
        {
            Forward = Vector3D.Normalize(center - Position);
            Up = Vector3D.Normalize(Up - Vector3D.Dot(Forward, Up) * Forward);
            Left = Vector3D.Cross(Up, Forward);

            Orientation = Quaternion<float>.CreateFromRotationMatrix(new Matrix3X3<float>(-Left, Up, -Forward));
        }
        public void LookAt(Vector3D<float> eye, Vector3D<float> center, Vector3D<float> up)
        {
            Position = eye;
            Forward = Vector3D.Normalize(center - eye);
            Left = Vector3D.Normalize(Vector3D.Cross(up, Forward));
            Up = Vector3D.Cross(Forward, Left);
            Orientation = Quaternion<float>.CreateFromRotationMatrix(new Matrix3X3<float>(-Left, Up, -Forward));
        }

        private void RecalculateProjection()
        {
            ShouldRecalculateProjection = false;
            _projection =
                Matrix4X4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, NearDistance,
                    FarDistance);
        }
        private void RecalculateView()
        {
            ShouldRecalculateView = false;
            _view = Matrix4X4.CreateFromQuaternion(Quaternion<float>.Negate(Orientation));
            _view.Row4 = Matrix4X4.Multiply(new Vector4D<float>(Vector3D.Negate(_position), 1), _view);
        }
        private void RegisterRotation()
        {
            if (++RotationHitCount <= RotationHitCountMax) return;
            RotationHitCount = 0;
            NormalizeCamera();
        }
        private void NormalizeCamera()
        {
            Up = Vector3D.Normalize(Up);
            Forward = Vector3D.Normalize(Forward);
            Orientation = Quaternion<float>.Normalize(Orientation);
            Left = Vector3D.Cross(Up, Forward);
            Up = Vector3D.Cross(Forward, Up);
        }

        private bool ShouldRecalculateView { get; set; }
        private bool ShouldRecalculateProjection { get; set; }
        private int RotationHitCount { get; set; }
        private const int RotationHitCountMax = 1000;
        private Matrix4X4<float> _projection;
        private float _fov;
        private float _aspectRatio;
        private float _nearDistance;
        private float _farDistance;
        private Quaternion<float> _orientation;
        private Vector3D<float> _position;
        private Vector3D<float> _left;
        private Vector3D<float> _up;
        private Vector3D<float> _forward;
        private Matrix4X4<float> _view;
        public void Log(int depth = 0)
        {
            $"Current Camera: <c19 {View}>".LogLine(depth);
            $"Position:\t<c25 {Position}>".LogLine(depth + 2);
            $"Left:\t{Left}".LogLine(depth + 2);
            $"Up:\t\t{Up}".LogLine(depth + 2);
            $"Forward:\t{Forward}".LogLine(depth + 2);
        }
    }
}
