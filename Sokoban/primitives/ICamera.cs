using Silk.NET.Maths;
using Sokoban.primitives.components;

namespace Sokoban.primitives
{
    internal interface ILens : ITransform
    {
        public Matrix4X4<float> Projection { get; }
        public void LookAt(Vector3D<float> target)
            => Rotation = Quaternion<float>.CreateFromRotationMatrix(Matrix4X4.CreateLookAt(Position, target, Up));
    }
}