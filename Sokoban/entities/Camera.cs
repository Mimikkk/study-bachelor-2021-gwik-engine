#nullable enable
using System;
using Assimp;
using Silk.NET.Maths;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban.entities
{
    internal class Camera : Frustum, ILens
    {
        public ITransform Transform => this;
        public ILens Lens => this;

        public Camera(float fieldOfView, float aspectRatio, float nearDistance, float farDistance) 
            : base(fieldOfView, aspectRatio, nearDistance, farDistance) { }

        public Vector3D<float> Position { get; set; } = Vector3D<float>.Zero;
        public Quaternion<float> Rotation { get; set; } = Quaternion<float>.Identity;
        public float Scale { get; set; }

        public void Log(int depth = 0)
        {
            $"Position:\t<c25 {Position}>".LogLine(depth + 2);
            $"Roll/Pitch/Yaw | Local:\t{Rotation.ToRollPitchYaw()}".LogLine(depth + 2);
            $"Left:\t{Transform.Left}".LogLine(depth + 2);
            $"Up:\t\t{Transform.Up}".LogLine(depth + 2);
            $"Forward:\t{Transform.Forward}".LogLine(depth + 2);
        }
    }
}