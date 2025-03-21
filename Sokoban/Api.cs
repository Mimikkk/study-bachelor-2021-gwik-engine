﻿using System;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Sokoban
{
internal static class Api
{
    internal static GL Gl { get; set; }
    internal static IMouse Mouse { get; private set; }
    internal static IKeyboard Keyboard { get; private set; }

    private static IInputContext InputContext { get; set; }

    private static void InitializeOpenGl(GL gl)
    {
        Gl = gl;
        Gl.Enable(EnableCap.DepthTest);
        Gl.Enable(EnableCap.Blend);
        Gl.Enable(EnableCap.CullFace);
        Gl.Enable(EnableCap.Multisample);
        Gl.Enable(EnableCap.LineSmooth);
        Gl.Enable(EnableCap.MultisampleSgis);
        Gl.Enable(EnableCap.MinmaxExt);
        Gl.Enable(EnableCap.PolygonSmooth);
        Gl.Enable(EnableCap.SampleShading);
        Gl.Hint(HintTarget.MultisampleFilterHintNV, HintMode.Nicest);
        Gl.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
        Gl.Hint(HintTarget.LineQualityHintSgix, HintMode.Nicest);
        Gl.Hint(HintTarget.WideLineHintPgi, HintMode.Nicest);
        Gl.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        Gl.Hint(HintTarget.AlwaysSoftHintPgi, HintMode.Nicest);
    }

    private static void InitializeInputContext(IInputContext inputContext)
    {
        InputContext = inputContext;
        if (InputContext == null) throw new Exception("Main: Failed to create input context");
        InitializeInputContext();
    }
    private static void InitializeInputContext()
    {
        InitializeKeyboard();
        InitializeMouse();
    }
    private static void InitializeMouse()
    {
        if (InputContext.Mice.Count > 0) Mouse = InputContext.Mice[0];
        if (Mouse == null) throw new Exception("Main: No Mouse detected");
        Mouse.Cursor.CursorMode = CursorMode.Raw;
    }
    private static void InitializeKeyboard()
    {
        if (InputContext.Keyboards.Count > 0) Keyboard = InputContext.Keyboards[0];
        if (Keyboard == null) throw new Exception("Main: No Keyboard detected");
    }
    internal static void Initialize(IWindow window)
    {
        InitializeOpenGl(GL.GetApi(window));
        InitializeInputContext(window.CreateInput());
    }
}
}