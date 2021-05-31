#nullable enable
using Assimp;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Sokoban.engine.renderer;
using WindowType = Silk.NET.Windowing.Window;

namespace Sokoban.engine
{
    public class GameWindow
    {
        public void Run() => Window.Run();
        public void Close() => Window.Close();

        public float Width => Window.Size.X;
        public float Height => Window.Size.Y;
        public float AspectRatio => Width / Height;

        public GameWindow()
        {
            Window = WindowType.Create(WindowOptions);
            Window.Load += OnLoad;
            Window.Update += OnUpdate;
            Window.Render += OnRender;
        }

        private void OnLoad()
        {
            Window.Center();
            Api.Initialize(Window);
            Game.Initialize();
        }
        private static void OnUpdate(double deltaTime)
        {
            // Game.Camera.LookAt(Vector3D<float>.One, Vector3D<float>.UnitY);
            Game.Updateables.ForEach(u => u.Update(deltaTime));
        }

        private static void OnRender(double deltaTime)
        {
            Renderer.Clear();
            Game.Renderables.ForEach(go => go.Render());
        }

        public IWindow Window { get; }
        private static WindowOptions WindowOptions
        {
            get
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
}
