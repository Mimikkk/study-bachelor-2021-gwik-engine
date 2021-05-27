using Silk.NET.OpenGL;

namespace GWiK_Sokoban.engine.renderer
{
    public class VertexBuffer : BufferObject<float>
    {
        public uint VertexDataLength => DataCount;
        public VertexBuffer(float[] vertices) : base(vertices, BufferTargetARB.ArrayBuffer)
        {
        }
    }
}