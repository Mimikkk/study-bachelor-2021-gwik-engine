using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.engine;
using Sokoban.entities;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban
{
    public static class Game
    {
        public static void Start() => GameWindow.Run();

        internal static QuaternionCamera Camera { get; private set; }
        private static GameController GameController { get; } = new();
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
            // Renderables.AddRange(ObjectLoader.Load("Stupid"));
            Renderables.AddRange(ObjectLoader.Load("BoxStack"));
            Updateables.Add(GameController);
        }

        private static void InitializeCamera()
        {
            var cam = new QuaternionCamera(new Vector3D<float>(0, 0.5f, 2), 90, GameWindow.AspectRatio, 0.1f, 100f);
            Camera = cam;
            var cc = new CameraController();
            cc.AddHold(Key.W, dt => { cam.TranslateLocal(Vector3D<float>.UnitZ * (float) dt); });
            cc.AddHold(Key.S, dt => { cam.TranslateLocal(-Vector3D<float>.UnitZ * (float) dt); });
            cc.AddHold(Key.A, dt => { cam.TranslateLocal(Vector3D<float>.UnitX * (float) dt); });
            cc.AddHold(Key.D, dt => { cam.TranslateLocal(-Vector3D<float>.UnitX * (float) dt); });
            cc.AddHold(Key.Q, dt => { cam.Rotate((float) dt, Vector3D<float>.UnitZ); });
            cc.AddHold(Key.E, dt => { cam.Rotate(-(float) dt, Vector3D<float>.UnitZ); });
            cc.AddHold(Key.Space, dt => { cam.TranslateLocal(Vector3D<float>.UnitY * (float) dt); });
            cc.AddHold(Key.ControlLeft, dt => { cam.TranslateLocal(-Vector3D<float>.UnitY * (float) dt); });

            cc.AddHold(Key.Keypad0, dt => {
                $"Pos: {cam.Position}".LogLine();
                cam.TranslateLocal(Vector3D<float>.UnitX * 5 * (float) dt);
                cam.TranslateLocal(Vector3D<float>.UnitY * 5 * (float) dt);
                cam.LookAt(Vector3D<float>.Zero);
            });


            cc.AddMove((position) =>
            {
                if (LastMousePosition == default) return;
                var offset = (position - LastMousePosition) * 0.1f;
                cam.Turn(MathHelper.DegreesToRadians(offset.X));
                cam.Rotate(MathHelper.DegreesToRadians(offset.Y), Vector3D<float>.UnitX);
            });

            cc.IsActive = true;
            Updateables.Add(cc);
            GameController.AddClick(MouseButton.Middle,()=>{cam.Log();});
            cam.Log();
        }

        private static void InitializeCallbacks()
        {
            var clickCounter = 0;
            GameController.AddClick(MouseButton.Left,
                () => { $"<c19 I Clicked> <c16 {++clickCounter}> <c19 Times>".LogLine(); });
            GameController.AddRelease(Key.Escape, () => GameWindow.Close());
            GameController.AddMove(position =>
            {
                $"Last Position: <c19 {LastMousePosition}>, New Position: <c19 {position}>, Difference: <c9 {LastMousePosition - position}>"
                    .LogLine();
                LastMousePosition = position;
            });

            GameController.IsActive = true;
        }
    }
}