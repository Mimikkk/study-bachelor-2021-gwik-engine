using System.Collections.Generic;
using System.Linq;
using Sokoban.engine.renderer;
using Sokoban.utilities;

namespace Sokoban.engine.objects
{
    internal class Mesh : IHasLog
    {
        public string Name { get; }
        public readonly List<Vertex> Vertices;
        public readonly Material Material;
        public List<int> Indices { get; set; }

        public VertexArrayObject Vao { get; }

        public Mesh(string name, IEnumerable<Vertex> vertices, IEnumerable<int> indices, Material material)
        {
            Name = name;
            Material = material;
            Indices = indices.ToList();
            Vertices = vertices.ToList();
            Vao = new VertexArrayObject(new VertexBuffer(Vertices), new IndexBuffer(Indices));
            Vao.ConfigureLayout((3, 8u), (2, 8u), (3, 8u));
        }

        public void Log(int depth = 0)
        {
            $"Mesh: <c20 {Name}>".LogLine(depth);
            $"Number of Vertices: <c22 {Vertices.Count}>".LogLine(depth + 2);
            $"Number of Indices: <c22 {Indices.Count}>".LogLine(depth + 2);
        }
    }
}
