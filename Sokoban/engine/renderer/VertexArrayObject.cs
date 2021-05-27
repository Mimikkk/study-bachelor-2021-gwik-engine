using System;
using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    public class VertexArrayObject : IDisposable
    {
        private uint Handle { get; }
        public VertexBuffer VertexBufferObject { get; }
        public IndexBuffer IndexBufferObject { get; }
        public uint IndexCount =>
            _vertexOffset == 0 ? 0 : VertexBufferObject.VertexDataLength / (sizeof(float) * _vertexOffset);
        private uint _vertexOffset;
        private uint _layoutSize;

        public VertexArrayObject(VertexBuffer vbo, IndexBuffer ibo)
        {
            Handle = Api.Gl.GenVertexArray();
            VertexBufferObject = vbo;
            IndexBufferObject = ibo;
            Bind();
        }
        public void ConfigureLayout(params LayoutElement[] layoutElements)
        {
            Bind();
            foreach (var element in layoutElements) VertexAttributePointer(element);
        }
        public void ConfigureLayout(params (int, uint)[] layoutElements)
        {
            Bind();
            foreach (var (count, size) in layoutElements)
                VertexAttributePointer(new LayoutElement(count, size));
        }

        private unsafe void VertexAttributePointer(LayoutElement element)
        {
            Api.Gl.VertexAttribPointer(_layoutSize, element.Count, VertexAttribPointerType.Float, false,
                element.Size * sizeof(float), (void*) (_vertexOffset * sizeof(float)));
            Api.Gl.EnableVertexAttribArray(_layoutSize);
            _vertexOffset += (uint) element.Count;
            _layoutSize += 1;
        }

        public void Bind()
        {
            Api.Gl.BindVertexArray(Handle);
            VertexBufferObject.Bind();
            IndexBufferObject.Bind();
        }

        public void Dispose()
        {
            IndexBufferObject.Dispose();
            VertexBufferObject.Dispose();
            Api.Gl.DeleteVertexArray(Handle);
        }
    }

    [Serializable]
    public struct LayoutElement
    {
        public int Count { get; }
        public uint Size { get; }

        public LayoutElement(int count, uint size)
        {
            Count = count;
            Size = size;
        }
    }
}
