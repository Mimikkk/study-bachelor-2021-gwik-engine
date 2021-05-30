#nullable enable
using Assimp;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.engine.renderer;
using Sokoban.utilities;

namespace Sokoban.engine
{
    public class GameWindow
    {
        public void Run() => _window.Run();
        public void Close() => _window.Close();
        public float AspectRatio => (float) _window.Size.X / _window.Size.Y;


        public GameWindow()
        {
            _window = Window.Create(OptionConfiguration());
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
        }

        private readonly IWindow _window;
        private void OnLoad()
        {
            _window.Center();
            Api.Initialize(GL.GetApi(_window), _window.CreateInput());
            Game.Initialize();
        }
        private static void OnUpdate(double deltaTime)
        {
            Game.Updateables.ForEach(u => u.Update(deltaTime));
        }
        private static void OnRender(double deltaTime)
        {
            Renderer.Clear();
            Game.Renderables.ForEach(go => go.Render());
        }

        private static WindowOptions OptionConfiguration()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 700);
            options.Title = "OpenGL: AA";
            options.PreferredDepthBufferBits = 24;
            options.PreferredStencilBufferBits = 24;
            options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default,
                new APIVersion(4, 5));
            return options;
        }
    }
}
