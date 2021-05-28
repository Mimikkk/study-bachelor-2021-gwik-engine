using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    public class IndexBuffer : BufferObject<int>
    {
        public IndexBuffer(IEnumerable<int> indices) 
            : base(indices.ToArray(), BufferTargetARB.ElementArrayBuffer)
        {
        }
    }
}