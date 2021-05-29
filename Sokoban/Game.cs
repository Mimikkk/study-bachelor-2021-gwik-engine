using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
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

        internal static QuaternionCamera Camera { get; private set; }
        private static Controller GameController { get; } = new();
        internal static Vector2D<float> LastMousePosition { get; set; }


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
            // Renderables.AddRange(ObjectLoader.Load("BoxStack"));
        }
        private static void InitializeCamera()
        {
            Camera = new QuaternionCamera(new Vector3D<float>(0, 0.5f, 2), 90, GameWindow.AspectRatio, 0.1f, 100f);
            // Camera.NormalizeCamera();
            // Updateables.Add(Camera);
        }
        private static void InitializeCallbacks()
        {
            GameController.AddKeyUps((Key.Escape, () => GameWindow.Close()));
        }
    }
}
