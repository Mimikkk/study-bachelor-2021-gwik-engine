using System.Linq;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Sokoban.primitives.components;

namespace Sokoban.engine.renderer
{
    public static class Renderer
    {
        internal static void Draw(VertexArrayObject vao, ShaderProgram spo)
        {
            vao.Bind();
            spo.Bind();
            spo.Configuration();
            Api.Gl.DrawArrays(PrimitiveType.Triangles, 0, vao.IndexCount);
        }

        public static void Clear()
        {
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            Api.Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public static void SetDrawMode(PolygonMode mode)
        {
            Api.Gl.PolygonMode(MaterialFace.FrontAndBack, mode);
        }

        public static void SetClearColor(Color color)
        {
            Api.Gl.ClearColor(color.R, color.G, color.B, color.A);
        }
    }
}
