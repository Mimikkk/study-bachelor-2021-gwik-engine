#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Silk.NET.OpenGL;
using static Sokoban.utilities.FieldExtensions;

namespace Sokoban.engine.renderer
{
internal class VertexArrayObject : IDisposable
{
    private uint Handle { get; }
    public VertexBuffer VertexBufferObject { get; init; }
    public IndexBuffer? IndexBufferObject { get; init; }

    public uint Size => IndexBufferObject?.Count
                        ?? (PerVertexSize != 0 ? VertexBufferObject.Count / PerVertexSize : 0);
    private uint PerVertexSize => Layout.Size * sizeof(float);

    public ElementLayout Layout {
        get => _layout;
        init => SetAndOperation(() => _layout = value, ReconfigureLayout);
    }

    public VertexArrayObject(VertexBuffer vertexBufferObject)
    {
        Handle = Api.Gl.GenVertexArray();
        VertexBufferObject = vertexBufferObject;
    }

    private unsafe void ReconfigureLayout()
    {
        Bind();
        var offset = 0;
        for (uint i = 0; i < Layout.Elements.Count; ++i)
        {
            var element = Layout.Elements[(int) i];
            Api.Gl.VertexAttribPointer(i, element, VertexAttribPointerType.Float, false,
                Layout.Size * sizeof(float), (void*) (offset * sizeof(float)));
            Api.Gl.EnableVertexAttribArray(i);
            offset += element;
        }
    }

    public void Bind()
    {
        Api.Gl.BindVertexArray(Handle);
        VertexBufferObject.Bind();
        IndexBufferObject?.Bind();
    }

    public void Dispose()
    {
        IndexBufferObject?.Dispose();
        VertexBufferObject.Dispose();
        Api.Gl.DeleteVertexArray(Handle);
    }

    private ElementLayout _layout;
}
internal readonly struct ElementLayout
{
    public uint Size { get; }
    public IReadOnlyList<int> Elements { get; }

    public ElementLayout(uint size, IReadOnlyList<int> elements)
    {
        Size = size;
        Elements = elements;
    }
}
}