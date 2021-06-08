using System;
using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    public abstract class BufferObject<TDataType> : IDisposable where TDataType : unmanaged
    {
        private uint Handle { get; }
        private BufferTargetARB BufferType { get; }
        public readonly uint Count;
        public readonly uint Size;

        protected unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType)
        {
            BufferType = bufferType;

            Handle = Api.Gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                Count = (uint) data.Length;
                Size = (uint) (Count * sizeof(TDataType));
                Api.Gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind() { Api.Gl.BindBuffer(BufferType, Handle); }

        public void Dispose() { Api.Gl.DeleteBuffer(Handle); }
    }
}