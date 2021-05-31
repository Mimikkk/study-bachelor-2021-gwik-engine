using Sokoban.engine.objects;
using Sokoban.engine.renderer;

namespace Sokoban.primitives.components
{
    internal interface IRenderable
    {
        public Mesh Mesh { get; }
        public ShaderProgram Spo { get; }
        public void Render()
        {
            Renderer.Draw(Mesh.Vao, Spo);
        }
    }
}
