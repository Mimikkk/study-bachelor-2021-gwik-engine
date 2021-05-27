using System;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GWiK_Sokoban.engine.objects
{
    internal class Texture : IDisposable
    {
        private uint Handle { get; }
        public string Name { get; }
        private Path Imagepath => Path.Textures / Name;

        public unsafe Texture(string name)
        {
            Handle = Game.Gl.GenTexture();
            Name = name;

            var image = (Image<Rgba32>) Image.Load(Imagepath.ToString());
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            fixed (void* d = &MemoryMarshal.GetReference(image.GetPixelRowSpan(0)))
                Load(d, (uint) image.Width, (uint) image.Height);

            image.Dispose();
        }

        private unsafe void Load(void* data, uint width, uint height)
        {
            Bind();
            Game.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba,
                width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            Game.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.ClampToEdge);
            Game.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.ClampToEdge);
            Game.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
            Game.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
            Game.Gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(int textureSlot = 0)
        {
            Game.Gl.ActiveTexture(TextureUnit.Texture0 + textureSlot);
            Game.Gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            Game.Gl.DeleteTexture(Handle);
        }
    }
}