using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Sokoban.primitives.components;

namespace Sokoban.engine.renderer
{
public static class Renderer
{
    internal static void Draw(IRenderable renderable)
    {
        renderable.Spo.Bind();
        renderable.ShaderConfiguration();
        Draw(renderable.Mesh.Vao, PrimitiveType.Triangles);
    }

    private static unsafe void Draw(VertexArrayObject vao, PrimitiveType primitiveType)
    {
        vao.Bind();
        Api.Gl.DrawElements(primitiveType, vao.Size, DrawElementsType.UnsignedInt, null);
    }

    // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
    public static void Clear() => Api.Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

    public static void SetDrawMode(PolygonMode mode) => Api.Gl.PolygonMode(MaterialFace.FrontAndBack, mode);
    public static void SetClearColor(Color color) => Api.Gl.ClearColor(color.R, color.G, color.B, color.A);
}
}