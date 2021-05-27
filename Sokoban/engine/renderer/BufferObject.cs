using System;
using Silk.NET.OpenGL;

namespace Sokoban.engine.renderer
{
    public abstract class BufferObject<TDataType> : IDisposable
        where TDataType : unmanaged
    {
        private uint Handle { get; }
        private readonly BufferTargetARB _bufferType;
        protected readonly uint DataCount;
        
        protected unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType)
        {
            _bufferType = bufferType;

            Handle = Api.Gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                DataCount = (uint) (data.Length * sizeof(TDataType));
                Api.Gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind()
        {
            Api.Gl.BindBuffer(_bufferType, Handle);
        }

        public void Dispose()
        {
            Api.Gl.DeleteBuffer(Handle);
        }
    }
}