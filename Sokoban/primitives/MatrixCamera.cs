using System;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.engine.input;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban.primitives
{
    public class MatrixCamera : ITransform, IUpdateable, ICamera
    {
        public Vector3D<float> Position { get; set; }
        public float Scale { get; set; }
        public Quaternion<float> Rotation { get; set; }

        public Matrix4X4<float> View
        {
            get => Matrix4X4
                .Transform(Matrix4X4.CreateLookAt(Position, Position + Forward, Up), Rotation);
            set => throw new NotImplementedException();
        }
        public Matrix4X4<float> Projection
        {
            get => Matrix4X4
                .CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Zoom), Game.GameWindow.AspectRatio, 0.1f,
                    100.0f);
            set => throw new NotImplementedException();
        }

        public Vector3D<float> Forward { get; set; }
        public Vector3D<float> Up { get; private set; }

        private float Yaw { get; set; }
        private float Pitch { get; set; }
        private float Roll { get; set; }

        private float Zoom { get; set; }

        public MatrixCamera(Vector3D<float> position, Vector3D<float> forward, Vector3D<float> up)
        {
            Position = position;
            Rotation = Quaternion<float>.Identity;

            Scale = 1;
            Forward = forward;
            Up = up;

            Yaw = -90f;
            Zoom = 90;
            // InitializeController();
        }

        public void ModifyZoom(float zoomAmount)
        {
            Zoom = Math.Clamp(Zoom - 5 * zoomAmount, 70, 90);
        }

        public void ModifyDirection(float xOffset = 0, float yOffset = 0)
        {
            Yaw += xOffset;
            Pitch = Math.Clamp(Pitch - yOffset, -89f, 89f);

            var cameraDirection = Vector3D<float>.Zero;
            cameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) *
                                MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            cameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            cameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw))
                                * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            Forward = Vector3D.Normalize(cameraDirection);
        }

        public void Update(double deltaTime)
        {
            CameraController.Update(deltaTime);
        }

        private Controller CameraController { get; } = new();

        private const float LookSensitivity = 0.1f;
        private const float MoveSpeed = 30f;
        private Vector2D<float> _lastMousePosition;
        private void InitializeController()
        {
            CameraController.AddKeys(
                (Key.W, dt => Position += Forward * MoveSpeed * (float) dt),
                (Key.S, dt => Position -= Forward * MoveSpeed * (float) dt),
                (Key.D, dt => Position += Vector3D.Normalize(Vector3D.Cross(Forward, Up)) * MoveSpeed * (float) dt),
                (Key.A, dt => Position -= Vector3D.Normalize(Vector3D.Cross(Forward, Up)) * MoveSpeed * (float) dt),
                (Key.Space, dt => Position += Up * MoveSpeed * (float) dt),
                (Key.ControlLeft, dt => Position -= Up * MoveSpeed * (float) dt));
            CameraController.AddMouseMoves(position =>
            {
                if (_lastMousePosition == default) _lastMousePosition = position;
                else
                {
                    var xOffset = (position.X - _lastMousePosition.X) * LookSensitivity;
                    var yOffset = (position.Y - _lastMousePosition.Y) * LookSensitivity;
                    _lastMousePosition = position;

                    ModifyDirection(xOffset, yOffset);
                }
            });
            CameraController.AddMouseScrolls(scroll => ModifyZoom(scroll.Y));
        }
    }
}
