﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Silk.NET.OpenGL;
using Sokoban.engine.objects;
using Sokoban.utilities;
using Shader = Sokoban.engine.renderer.Shader;

namespace Sokoban.engine.renderer
{
    internal class ShaderProgram : IDisposable
    {
        private uint Handle { get; }
        private List<Shader> Shaders { get; } = new();
        public Action Configuration;
        public ShaderProgram(Action configuration = null, params Shader[] shaders)
        {
            Handle = Api.Gl.CreateProgram();
            AttachShaders(shaders);
            Configuration = configuration;
            Link();
        }

        public void Link()
        {
            Api.Gl.LinkProgram(Handle);
            VerifyLinkStatus();
        }
        private void VerifyLinkStatus()
        {
            Api.Gl.GetProgram(Handle, GLEnum.LinkStatus, out var status);
            if (status != 0) return;
            $"Program <c9 failed> to link with error: <c9 {Api.Gl.GetProgramInfoLog(Handle)}>".LogLine();
            throw new Exception();
        }

        public void Bind()
        {
            Api.Gl.UseProgram(Handle);
        }

        public unsafe void SetUniform(string name, int value)
        {
            Api.Gl.Uniform1(UniformLocation(name), value);
        }
        public unsafe void SetUniform(string name, float value)
        {
            Api.Gl.Uniform1(UniformLocation(name), value);
        }
        public unsafe void SetUniform(string name, double value)
        {
            Api.Gl.Uniform1(UniformLocation(name), value);
        }

        public unsafe void SetUniform(string name, Vector2 value)
        {
            Api.Gl.Uniform2(UniformLocation(name), value);
        }
        public unsafe void SetUniform(string name, Vector3 value)
        {
            Api.Gl.Uniform3(UniformLocation(name), value);
        }
        public unsafe void SetUniform(string name, Vector4 value)
        {
            Api.Gl.Uniform4(UniformLocation(name), value);
        }
        public unsafe void SetUniform(string name, Color color)
        {
            Api.Gl.Uniform4(UniformLocation(name), new Vector4(color.R, color.G, color.B, color.A));
        }

        public unsafe void SetUniform(string name, Matrix4x4 value, bool transpose = false)
        {
            Api.Gl.UniformMatrix4(UniformLocation(name), 1, transpose, (float*) &value);
        }

        private int UniformLocation(string name)
        {
            var location = Api.Gl.GetUniformLocation(Handle, name);
            if (location == -1) throw new Exception($"{name} uniform not found on shader.");
            return location;
        }
        private int AttributeLocation(string name)
        {
            var location = Api.Gl.GetAttribLocation(Handle, name);
            if (location == -1) throw new Exception($"{name} attribute not found on shader.");
            return location;
        }

        public void AttachShaders(params Shader[] shaders)
        {
            foreach (var shader in shaders) AttachShader(shader);
        }
        public void AttachShader(Shader shader)
        {
            Shaders.Add(shader);
            Api.Gl.AttachShader(Handle, shader.Handle);
        }

        public void DetachShaders()
        {
            foreach (var shader in Shaders) DetachShader(shader);
        }
        private void DetachShader(Shader shader)
        {
            Shaders.Remove(shader);
            Api.Gl.DetachShader(Handle, shader.Handle);
        }

        public void Dispose()
        {
            Api.Gl.DeleteProgram(Handle);
        }
        public static readonly ShaderProgram DefaultBase = new
        (
            () => { },
            new Shader(ShaderType.VertexShader, "BaseShader"),
            new Shader(ShaderType.FragmentShader, "BaseShader")
        );
        public static readonly ShaderProgram Default = new
        (
            () => { },
            new Shader(ShaderType.VertexShader, "Default"),
            new Shader(ShaderType.FragmentShader, "Default")
        );
    }
}
