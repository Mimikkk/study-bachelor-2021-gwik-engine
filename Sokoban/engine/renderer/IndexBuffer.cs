using Silk.NET.OpenGL;

namespace GWiK_Sokoban.engine.renderer
{
    public class IndexBuffer : BufferObject<uint>
    {
        public IndexBuffer(uint[] indices) : base(indices, BufferTargetARB.ElementArrayBuffer)
        {
        }
    }
}