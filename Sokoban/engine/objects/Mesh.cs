#nullable enable
using System.Collections.Generic;
using Sokoban.engine.renderer;
using Sokoban.utilities;

namespace Sokoban.engine.objects
{
internal class Mesh : IHasLog
{
    public string Name { get; }
    public readonly Material Material;
    private int VertexCount => (int) Vao.IndexBufferObject?.Count;
    private int IndexCount => (int) Vao.VertexBufferObject.Count;

    public VertexArrayObject Vao { get; }
    public Mesh(string name, Material material, IEnumerable<Vertex> vertices, IEnumerable<int>? indices = null)
    {
        Name = name;
        Material = material;
        Vao = new VertexArrayObject(new VertexBuffer(vertices)) {
            IndexBufferObject = indices != null ? new IndexBuffer(indices) : null,
            Layout = Vertex.ElementLayout
        };
    }

    public void Log(int depth = 0)
    {
        $"Mesh: <c20 {Name}|>".LogLine(depth);
        $"Number of Vertices: <c22 {VertexCount}|>".LogLine(depth + 2);
        $"Number of Indices: <c22 {IndexCount}|>".LogLine(depth + 2);
    }
}
}