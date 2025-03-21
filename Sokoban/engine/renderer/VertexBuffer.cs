﻿using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    internal class VertexBuffer : BufferObject<float>
    {
        public VertexBuffer(IEnumerable<float> vertices)
            : base(vertices.ToArray(), BufferTargetARB.ArrayBuffer)
        {
        }
        public VertexBuffer(IEnumerable<Vertex> vertices)
            : this(vertices.SelectMany(v => v.ToFloats()))
        {
        }
    }
}
