using Silk.NET.Maths;
using Sokoban.primitives.components;

namespace Sokoban.primitives
{
    internal interface ICamera
    {
        public Matrix4X4<float> Projection { get; }
        public Matrix4X4<float> View { get; }
    }
}