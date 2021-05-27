using System;
using Silk.NET.OpenGL;

namespace GWiK_Sokoban.engine.renderer
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

            Handle = Game.Gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                DataCount = (uint) (data.Length * sizeof(TDataType));
                Game.Gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind()
        {
            Game.Gl.BindBuffer(_bufferType, Handle);
        }

        public void Dispose()
        {
            Game.Gl.DeleteBuffer(Handle);
        }
    }
}