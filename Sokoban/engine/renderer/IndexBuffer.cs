using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    public class IndexBuffer : BufferObject<uint>
    {
        public IndexBuffer(uint[] indices) : base(indices, BufferTargetARB.ElementArrayBuffer)
        {
        }
    }
}