using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using GWiK_Sokoban.engine;
using GWiK_Sokoban.engine.input;
using GWiK_Sokoban.engine.interfaces;
using GWiK_Sokoban.engine.models;
using GWiK_Sokoban.engine.objects;
using GWiK_Sokoban.engine.renderer;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace GWiK_Sokoban
{
    internal static class Game
    {
        #region [[Internal Fields]]

        private static IInputContext _inputContext;
        internal static IKeyboard Keyboard { get; private set; }
        internal static IMouse Mouse { get; private set; }

        internal static Camera Camera { get; private set; }
        internal static GameWindow GameWindow { get; private set; }
        internal static GL Gl { get; set; }

        internal static readonly List<IRenderable> Renderables = new();
        internal static readonly List<IUpdateable> Updateables = new();

        #endregion

        private static void Main()
        {
            InitializeGameWindow();
            GameWindow.Run();
        }

        private static void InitializeGameWindow()
        {
            GameWindow = new GameWindow();
        }
        internal static void InitializeGameObjects()
        {
            Renderables.AddRange(ObjectLoader.MaybeLoad("stupid"));
            foreach (var go in Renderables) ((GameObject) go).DefaultInitialization();
        }

        internal static void InitializeOpenGl(GL gl)
        {
            Gl = gl;
            Gl.Enable(EnableCap.DepthTest);
            Gl.Enable(EnableCap.Blend);
            Gl.Enable(EnableCap.CullFace);
            Gl.Enable(EnableCap.Multisample);
            Gl.Enable(EnableCap.LineSmooth);
            Gl.Hint(HintTarget.MultisampleFilterHintNV, HintMode.Nicest);
            Gl.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            Gl.Hint(HintTarget.LineQualityHintSgix, HintMode.Nicest);
        }
        internal static void InitializeCamera()
        {
            Camera = new Camera(new Vector3(0, 0.5f, 2), Vector3.UnitZ * -1, Vector3.UnitY);
        }

        private static Controller GameController { get; } = new();

        internal static void MaybeInitializeInputContext(IInputContext inputContext)
        {
            _inputContext = inputContext;
            if (_inputContext == null) throw new Exception("Main: Failed to create input context");
            InitializeInputContext();
        }
        private static void InitializeInputContext()
        {
            MaybeInitializeKeyboard();
            MaybeInitializeMouse();
            InitializeCallbacks();
        }
        private static void MaybeInitializeMouse()
        {
            if (_inputContext.Mice.Count > 0) Mouse = _inputContext.Mice[0];
            if (Mouse == null) throw new Exception("Main: No Mouse detected");
            Mouse.Cursor.CursorMode = CursorMode.Raw;
        }
        private static void MaybeInitializeKeyboard()
        {
            if (_inputContext.Keyboards.Count > 0) Keyboard = _inputContext.Keyboards[0];
            if (Keyboard == null) throw new Exception("Main: No Keyboard detected");
        }
        private static void InitializeCallbacks()
        {
            GameController.AddKeyUps((Key.Escape, () => GameWindow.Close()));
        }
    }
}
