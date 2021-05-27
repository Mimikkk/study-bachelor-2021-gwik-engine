using System.Collections.Generic;
using System.Linq;
using GWiK_Sokoban.engine.interfaces;
using GWiK_Sokoban.engine.renderer;

namespace GWiK_Sokoban.engine.objects
{
    internal class Mesh : IHasInfo
    {
        public string Name { get; }

        public string Info()
        {
            return $"Mesh '{Name}' Properties:\n"
                   + $"\tIndices: {Vao.IndexCount}\n"
                   + $"\tVertices: {Vao.IndexCount / 3}\n";
        }

        public VertexArrayObject Vao { get; }

        public Mesh(string name, float[] vertices, uint[] indices)
        {
            Name = name;
            Vao = new VertexArrayObject(new VertexBuffer(vertices), new IndexBuffer(indices));
        }
        public Mesh(string name, IEnumerable<Vertex> vertices, IEnumerable<uint> indices, (int, uint)[] layout)
            : this(name, vertices.SelectMany(v => v.ToFloats()).ToArray(), indices.ToArray())
        {
            Vao.ConfigureLayout(layout);
        }
    }
}