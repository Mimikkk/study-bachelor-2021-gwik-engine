using System;
using System.Numerics;
using Silk.NET.Input;
using Sokoban.engine.input;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban.primitives
{
    public class Camera : ITransform, IUpdateable
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; }
        public Quaternion Rotation { get; set; }

        public Matrix4x4 ViewMatrix
        {
            get => Matrix4x4
                .Transform(Matrix4x4.CreateLookAt(Position, Position + Forward, Up), Rotation);
        }
        public Matrix4x4 ProjectionMatrix
        {
            get => Matrix4x4
                .CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Zoom), Game.GameWindow.AspectRatio, 0.1f,
                    100.0f);
        }

        public Vector3 Forward { get; set; }
        public Vector3 Up { get; private set; }

        private float Yaw { get; set; }
        private float Pitch { get; set; }
        private float Roll { get; set; }

        private float Zoom { get; set; }

        public Camera(Vector3 position, Vector3 forward, Vector3 up)
        {
            Position = position;
            Rotation = Quaternion.Identity;

            Scale = 1;
            Forward = forward;
            Up = up;

            Yaw = -90f;
            Zoom = 90;
            InitializeController();
        }

        public void ModifyZoom(float zoomAmount)
        {
            Zoom = Math.Clamp(Zoom - 5 * zoomAmount, 70, 90);
        }

        public void ModifyDirection(float xOffset = 0, float yOffset = 0)
        {
            Yaw += xOffset;
            Pitch = Math.Clamp(Pitch - yOffset, -89f, 89f);

            var cameraDirection = Vector3.Zero;
            cameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) *
                                MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            cameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            cameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw))
                                * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            Forward = Vector3.Normalize(cameraDirection);
        }

        public void Update(double deltaTime)
        {
            CameraController.Update(deltaTime);
        }

        public bool IsControllerActive { get; } = true;
        private Controller CameraController { get; } = new();


        private const float LookSensitivity = 0.1f;
        private const float MoveSpeed = 30f;
        private Vector2 _lastMousePosition;
        private void InitializeController()
        {
            CameraController.AddKeys(
                (Key.W, dt => Position += Forward * MoveSpeed * (float) dt),
                (Key.S, dt => Position -= Forward * MoveSpeed * (float) dt),
                (Key.D, dt => Position += Vector3.Normalize(Vector3.Cross(Forward, Up)) * MoveSpeed * (float) dt),
                (Key.A, dt => Position -= Vector3.Normalize(Vector3.Cross(Forward, Up)) * MoveSpeed * (float) dt),
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
