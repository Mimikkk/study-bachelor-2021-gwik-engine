using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Sokoban.engine;
using Sokoban.engine.input;
using Sokoban.entities;
using Sokoban.primitives.components;
using Sokoban.utilities;
using GameController = Sokoban.engine.input.GameController;
using Renderer = Sokoban.engine.renderer.Renderer;
using Texture = Sokoban.engine.renderer.Texture;

namespace Sokoban
{
public static class Game
{
    public static void Start() => GameWindow.Run();

    internal static Camera Camera { get; private set; }
    internal static Vector2D<float> LastMousePosition { get; set; }

    internal static readonly List<IRenderable> Renderables = new();
    internal static readonly List<IUpdateable> Updateables = new();
    internal static GameWindow GameWindow { get; } = new();

    internal static void Initialize()
    {
        InitializeCamera();
        InitializeGameObjects();
        InitializeControllers();
    }
    private static void InitializeGameObjects()
    {
        var cube = new Cube("rock");
        cube.Mesh.Material.DiffuseTexture = new Texture("brick/Color.jpg");
        cube.Mesh.Material.NormalTexture = new Texture("brick/Normal.jpg");
        cube.Mesh.Material.DisplacementTexture = new Texture("brick/Displacement.jpg");
        cube.Log();
        Renderables.Add(cube);

    }

    private static void InitializeCamera()
    {
        Camera = new Camera(90, GameWindow.AspectRatio, 0.1f, 100f) {
            Position = new Vector3D<float>(0, 0.5f, 2)
        };
    }

    private static void InitializeControllers()
    {
        Updateables.Add(new CameraController(Camera));
        Updateables.Add(new GameController());
    }
}
}