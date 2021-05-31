using System.Linq;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Sokoban.primitives;
using Sokoban.primitives.components;

namespace Sokoban.engine.renderer
{
    public static class Renderer
    {
        internal static void Draw(IRenderable renderable)
        {
            renderable.ShaderConfiguration();
            Draw(renderable.Mesh.Vao, renderable.Spo, PrimitiveType.Triangles);
        }

        private static void Draw(VertexArrayObject vao, ShaderProgram spo, PrimitiveType primitiveType)
        {
            vao.Bind();
            spo.Bind();
            Api.Gl.DrawArrays(primitiveType, 0, vao.IndexCount);
        }

        public static void Clear()
        {
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            Api.Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public static void SetDrawMode(PolygonMode mode) { Api.Gl.PolygonMode(MaterialFace.FrontAndBack, mode); }

        public static void SetClearColor(Color color) { Api.Gl.ClearColor(color.R, color.G, color.B, color.A); }
    }
}