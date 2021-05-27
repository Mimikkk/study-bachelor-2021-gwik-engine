using Silk.NET.OpenGL;
using Silk.NET.SDL;

namespace GWiK_Sokoban.engine.renderer
{
    public static class Renderer
    {
        public static void Draw(VertexArrayObject vao, ShaderProgram spo)
        {
            vao.Bind();
            spo.Bind();
            spo.Configuration();
            Game.Gl.DrawArrays(PrimitiveType.Triangles, 0, vao.IndexCount);
        }

        public static void Clear()
        {
            Game.Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public static void SetDrawMode(PolygonMode mode)
        {
            Game.Gl.PolygonMode(MaterialFace.FrontAndBack, mode);
        }

        public static void SetClearColor(Color color)
        {
            Game.Gl.ClearColor(color.R, color.G, color.B, color.A);
        }
    }
}