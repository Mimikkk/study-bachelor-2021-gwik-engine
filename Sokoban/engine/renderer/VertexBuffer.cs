using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    internal class VertexBuffer : BufferObject<float>
    {
        public uint Length => DataCount;
        public VertexBuffer(float[] vertices)
            : base(vertices, BufferTargetARB.ArrayBuffer)
        {
        }
        public VertexBuffer(IEnumerable<Vertex> vertices)
            : base(vertices.SelectMany(v => v.ToFloats()).ToArray(), BufferTargetARB.ArrayBuffer)
        {
        }
    }
}
