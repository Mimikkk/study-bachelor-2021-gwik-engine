using System.Collections.Generic;
using System.Linq;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.engine;
using Sokoban.engine.input;
using Sokoban.entities;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban
{
    public static class Game
    {
        public static void Start() => GameWindow.Run();

        internal static Camera Camera { get; private set; }
        private static Controller GameController { get; } = new();
        internal static Vector2D<float> LastMousePosition { get; set; }

        internal static readonly List<IRenderable> Renderables = new();
        internal static readonly List<IUpdateable> Updateables = new();
        private static GameWindow GameWindow { get; } = new();

        internal static void Initialize()
        {
            InitializeCamera();
            InitializeGameObjects();
            InitializeCallbacks();
        }
        private static void InitializeGameObjects()
        {
            var box = new Cube("hoh?");
            var stupid = new Stupid();
            var bc = new Controller();
            Renderables.Add(stupid);
            Renderables.Add(box);
            bc.AddHold(Key.KeypadDecimal, dt =>
            {
                box.Transform.Rotate(dt, Vector3D<float>.UnitX);
                stupid.Transform.Rotate(-dt, Vector3D<float>.UnitY);
                box.Log();
                stupid.Log();
            });
            
            bc.AddHold(Key.Keypad4, dt => box.Transform.TranslateLocal(-Vector3D<float>.UnitX * dt));
            bc.AddHold(Key.Keypad6, dt => box.Transform.TranslateLocal(Vector3D<float>.UnitX * dt));
            bc.AddHold(Key.Keypad8, dt => box.Transform.TranslateLocal(Vector3D<float>.UnitZ * dt));
            bc.AddHold(Key.Keypad2, dt => box.Transform.TranslateLocal(-Vector3D<float>.UnitZ * dt));
            
            bc.IsActive = true;
            Updateables.Add(bc);
        }

        private static void InitializeCamera()
        {
            var cam = new Camera(90, GameWindow.AspectRatio, 0.1f, 100f)
            {
                Position = new Vector3D<float>(0, 0.5f, 2)
            };
            Camera = cam;
            var cc = new Controller();
            cc.AddHold(Key.W, dt => { (Camera as ITransform).TranslateLocal(Vector3D<float>.UnitZ * dt); });
            cc.AddHold(Key.S, dt => { cam.Transform.TranslateLocal(-Vector3D<float>.UnitZ * dt); });
            cc.AddHold(Key.A, dt => { cam.Transform.TranslateLocal(Vector3D<float>.UnitX * dt); });
            cc.AddHold(Key.D, dt => { cam.Transform.TranslateLocal(-Vector3D<float>.UnitX * dt); });
            cc.AddHold(Key.Q, dt => { cam.Transform.Rotate(dt, Vector3D<float>.UnitZ); });
            cc.AddHold(Key.E, dt => { cam.Transform.Rotate(-dt, Vector3D<float>.UnitZ); });
            cc.AddHold(Key.Space, dt => { cam.Transform.TranslateLocal(Vector3D<float>.UnitY * dt); });
            cc.AddHold(Key.ControlLeft, dt => { cam.Transform.TranslateLocal(-Vector3D<float>.UnitY * dt); });
            cc.AddHold(Key.Keypad0, dt =>
            {
                var pos = cam.Position;
                $"Pos: {pos}".LogLine();
                cam.Lens.LookAt(new Vector3D<float>(pos.X, pos.Y, 0));
            });


            cc.AddMove(position =>
            {
                if (LastMousePosition == default) return;
                var offset = (position - LastMousePosition) * 0.1f;
                cam.Transform.Turn(MathHelper.DegreesToRadians(offset.X));
                cam.Transform.Rotate(MathHelper.DegreesToRadians(offset.Y), Vector3D<float>.UnitX);
            });

            cc.IsActive = true;
            Updateables.Add(cc);
            GameController.AddClick(MouseButton.Middle, () => { cam.Log(); });
            cam.Log();
        }

        private static void InitializeCallbacks()
        {
            var clickCounter = 0;
            GameController.AddClick(MouseButton.Left,
                () => $"<c19 I Clicked> <c16 {++clickCounter}> <c19 Times>".LogLine());
            GameController.AddRelease(Key.Escape, () => GameWindow.Close());
            GameController.AddMove(position =>
            {
                ($"Last Position: <c19 {LastMousePosition}>, "
                 + $"New Position: <c19 {position}>, Difference: <c9 {LastMousePosition - position}>").LogLine();
                LastMousePosition = position;
            });

            GameController.IsActive = true;
            Updateables.Add(GameController);
        }
    }
}