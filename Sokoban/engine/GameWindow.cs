#nullable enable
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using GWiK_Sokoban.engine.interfaces;
using GWiK_Sokoban.engine.renderer;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;

namespace GWiK_Sokoban.engine
{
    public class GameWindow
    {
        public void Run()
        {
            _window.Run();
        }
        public void Close()
        {
            _window.Close();
        }

        private readonly IWindow _window;
        private static DateTime _startTime;
        public float AspectRatio
        {
            get => (float) _window.Size.X / _window.Size.Y;
        }

        public GameWindow(Func<WindowOptions>? configuration = null)
        {
            _window = Window.Create((configuration ?? DefaultConfiguration)());
            _startTime = DateTime.UtcNow;

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Closing += OnClose;
        }

        private void OnLoad()
        {
            _window.Center(_window.Monitor);
            Game.InitializeOpenGl(GL.GetApi(_window));
            Game.MaybeInitializeInputContext(_window.CreateInput());
            Game.InitializeCamera();
            Game.InitializeGameObjects();


            Game.Updateables.Add(Game.Camera);
        }

        private static void OnUpdate(double deltaTime)
        {
            Game.Updateables.ForEach(u => u.Update(deltaTime));
        }

        private static void OnRender(double deltaTime)
        {
            Renderer.Clear();
            Game.Renderables.ForEach(go => go.MaybeRender());
        }

        private static void OnClose()
        {
        }

        private static Func<WindowOptions> DefaultConfiguration { get; } = () =>
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 700);
            options.Title = "OpenGL: Sokoban";
            options.PreferredDepthBufferBits = 24;
            options.PreferredStencilBufferBits = 24;
            return options;
        };
    }
}