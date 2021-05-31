using System;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban.entities
{
    public class MatrixCamera : ICamera
    {
        public Vector3D<float> Position { get; set; }
        public Quaternion<float> Rotation { get; set; }

        public Matrix4X4<float> View
            => Matrix4X4.Transform(Matrix4X4.CreateLookAt(Position, Position + Forward, Up), Rotation);
        public Matrix4X4<float> Projection
            => Matrix4X4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Zoom), Game.GameWindow.AspectRatio,
                0.1f, 100.0f);

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

            Forward = forward;
            Up = up;

            Yaw = -90f;
            Zoom = 90;
        }

        public void ModifyZoom(float zoomAmount) { Zoom = Math.Clamp(Zoom - 5 * zoomAmount, 70, 90); }

        public void ModifyDirection(float xOffset = 0, float yOffset = 0)
        {
            Yaw += xOffset;
            Pitch = Pitch - yOffset;

            var direction = Vector3D<float>.Zero;
            direction.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw))
                                * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            direction.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
            direction.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw))
                                * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
            Forward = Vector3D.Normalize(direction);
        }
    }
}