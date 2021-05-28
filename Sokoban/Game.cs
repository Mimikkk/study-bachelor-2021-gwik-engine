using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Silk.NET.Input;
using Sokoban.engine;
using Sokoban.engine.input;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban
{
    public static class Game
    {
        public static void Start() => GameWindow.Run();

        internal static Camera Camera { get; private set; }
        private static Controller GameController { get; } = new();

        internal static readonly List<IRenderable> Renderables = new();
        internal static readonly List<IUpdateable> Updateables = new();
        internal static GameWindow GameWindow { get; } = new();

        internal static void Initialize()
        {
            InitializeCamera();
            InitializeGameObjects();
            InitializeCallbacks();
        }
        private static void InitializeGameObjects()
        {
            Renderables.AddRange(ObjectLoader.Load("Stupid"));
            Renderables.AddRange(ObjectLoader.Load("BoxStack"));
        }
        private static void InitializeCamera()
        {
            Camera = new Camera(new Vector3(0, 0.5f, 2), Vector3.UnitZ * -1, Vector3.UnitY);
            Updateables.Add(Camera);
        }
        private static void InitializeCallbacks()
        {
            GameController.AddKeyUps((Key.Escape, () => GameWindow.Close()));
        }
    }
}
