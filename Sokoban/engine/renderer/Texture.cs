using System;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Sokoban.utilities;

namespace Sokoban.engine.renderer
{
    internal class Texture : IDisposable
    {
        public string Name { get; }
        public unsafe Texture(string name)
        {
            Handle = Api.Gl.GenTexture();
            Name = name;

            var image = (Image<Rgba32>) Image.Load(Imagepath.ToString());
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            fixed (void* d = &MemoryMarshal.GetReference(image.GetPixelRowSpan(0)))
                Load(d, (uint) image.Width, (uint) image.Height);
        }

        private uint Handle { get; }
        private Path Imagepath => Path.Textures / Name;


        private unsafe void Load(void* data, uint width, uint height)
        {
            Bind();
            Api.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba,
                width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            Api.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.ClampToEdge);
            Api.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.ClampToEdge);
            Api.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
            Api.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
            Api.Gl.GenerateTextureMipmap(Handle);
        }

        public void Bind(int textureSlot = 0)
        {
            Api.Gl.ActiveTexture(TextureUnit.Texture0 + textureSlot);
            Api.Gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            Api.Gl.DeleteTexture(Handle);
        }
    }
}
