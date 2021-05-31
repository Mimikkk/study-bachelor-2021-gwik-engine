using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Silk.NET.Maths;
using Sokoban.engine.renderer;
using Sokoban.utilities;

namespace Sokoban.engine.objects
{
    internal class Mesh : IHasLog
    {
        public string Name { get; }
        public readonly List<Vertex> Vertices;
        public readonly Material Material;
        public List<int> Indices { get; }

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
        public Mesh(string name, IEnumerable<float> vertices, IEnumerable<int> indices, Material material)
        {
            Name = name;
            Material = material;
            Indices = indices.ToList();
            var vs = vertices.ToList();
            Vertices = new List<Vertex>();
            for (var i = 0; i < vs.Count; i += 8)
                Vertices.Add(new Vertex(new Vector3D<float>(vs[i], vs[i + 1], vs[i + 2]),
                    new Vector2D<float>(vs[i + 3], vs[i + 4]), new Vector3D<float>(vs[i + 5], vs[i + 6], vs[i + 7])));
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