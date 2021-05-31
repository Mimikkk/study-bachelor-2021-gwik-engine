using System;
using System.Collections.Generic;
using System.Numerics;
using Assimp;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.primitives.components;
using static Sokoban.utilities.FieldExtensions;

namespace Sokoban.primitives
{
    internal class CameraController : Controller { }
    internal class GameController : Controller { }

    internal abstract class Controller : IUpdateable
    {
        private List<(Key, Action<double>)> KeyboardHoldCallbacks { get; } = new();
        private List<Action<IKeyboard, Key, int>> KeyboardPushCallbacks { get; } = new();
        private List<Action<IKeyboard, Key, int>> KeyboardReleaseCallbacks { get; } = new();

        private List<(MouseButton, Action<double>)> MouseHoldCallbacks { get; } = new();
        private List<Action<IMouse, MouseButton>> MousePushCallbacks { get; } = new();
        private List<Action<IMouse, MouseButton>> MouseReleaseCallbacks { get; } = new();
        private List<Action<IMouse, MouseButton, Vector2>> MouseClickCallbacks { get; } = new();
        private List<Action<IMouse, MouseButton, Vector2>> MouseDoubleClickCallbacks { get; } = new();
        private List<Action<IMouse, ScrollWheel>> MouseScrollCallbacks { get; } = new();
        private List<Action<IMouse, Vector2>> MouseMoveCallbacks { get; } = new();

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetAndOperation(() => _isActive = value,
                _isActive != value ? (value ? Activate : Deactivate) : null);
        }
        public void Update(double deltaTime)
        {
            if (!IsActive) return;
            foreach (var (key, callback) in KeyboardHoldCallbacks)
            {
                if (Api.Keyboard.IsKeyPressed(key)) callback(deltaTime);
            }
            foreach (var (button, callback) in MouseHoldCallbacks)
            {
                if (Api.Mouse.IsButtonPressed(button)) callback(deltaTime);
            }
        }

        public void AddHold(Key key, Action<double> callback) { KeyboardHoldCallbacks.Add((key, callback)); }
        public void AddPush(Key key, Action callback)
        {
            KeyboardPushCallbacks.Add((_, k, _) =>
            {
                if (key == k) callback();
            });
        }
        public void AddRelease(Key key, Action callback)
        {
            KeyboardReleaseCallbacks.Add((_, k, _) =>
            {
                if (key == k) callback();
            });
        }

        public void AddHold(MouseButton button, Action<double> callback) { MouseHoldCallbacks.Add((button, callback)); }
        public void AddPush(MouseButton button, Action callback)
        {
            MousePushCallbacks.Add((_, b) =>
            {
                if (button == b) callback();
            });
        }
        public void AddRelease(MouseButton button, Action callback)
        {
            MouseReleaseCallbacks.Add((_, b) =>
            {
                if (button == b) callback();
            });
        }
        public void AddClick(MouseButton button, Action callback)
        {
            MouseClickCallbacks.Add((_, b, _) =>
            {
                if (button == b) callback();
            });
        }
        public void AddDoubleClick(MouseButton button, Action callback)
        {
            MouseClickCallbacks.Add((_, b, _) =>
            {
                if (button == b) callback();
            });
        }
        public void AddScroll(Action<ScrollWheel> callback) { MouseScrollCallbacks.Add((_, wheel) => callback(wheel)); }
        public void AddMove(Action<Vector2D<float>> callback)
        {
            MouseMoveCallbacks.Add((_, position) => callback(new Vector2D<float>(position.X, position.Y)));
        }

        private void Activate()
        {
            KeyboardPushCallbacks.ForEach(callback => Api.Keyboard.KeyDown += callback);
            KeyboardReleaseCallbacks.ForEach(callback => Api.Keyboard.KeyUp += callback);

            MousePushCallbacks.ForEach(callback => Api.Mouse.MouseDown += callback);
            MouseReleaseCallbacks.ForEach(callback => Api.Mouse.MouseUp += callback);
            MouseClickCallbacks.ForEach(callback => Api.Mouse.Click += callback);
            MouseDoubleClickCallbacks.ForEach(callback => Api.Mouse.DoubleClick += callback);
            MouseScrollCallbacks.ForEach(callback => Api.Mouse.Scroll += callback);
            MouseMoveCallbacks.ForEach(callback => Api.Mouse.MouseMove += callback);
        }
        private void Deactivate()
        {
            KeyboardPushCallbacks.ForEach(callback => Api.Keyboard.KeyDown -= callback);
            KeyboardReleaseCallbacks.ForEach(callback => Api.Keyboard.KeyUp -= callback);

            MousePushCallbacks.ForEach(callback => Api.Mouse.MouseDown -= callback);
            MouseReleaseCallbacks.ForEach(callback => Api.Mouse.MouseUp -= callback);
            MouseClickCallbacks.ForEach(callback => Api.Mouse.Click -= callback);
            MouseDoubleClickCallbacks.ForEach(callback => Api.Mouse.DoubleClick -= callback);
            MouseScrollCallbacks.ForEach(callback => Api.Mouse.Scroll -= callback);
            MouseMoveCallbacks.ForEach(callback => Api.Mouse.MouseMove -= callback);
        }
    }
}