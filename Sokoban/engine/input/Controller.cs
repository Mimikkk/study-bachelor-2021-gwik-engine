using System;
using System.Collections.Generic;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace Sokoban.engine.input
{
    internal class Controller
    {
        private Dictionary<Key, Action<double>> KeyboardCallbacks { get; } = new();
        private Dictionary<MouseButton, Action<double>> MouseCallbacks { get; } = new();

        public void Update(double deltaTime)
        {
            foreach (var (key, callback) in KeyboardCallbacks)
            {
                if (Api.Keyboard.IsKeyPressed(key))
                    callback(deltaTime);
            }
            foreach (var (button, callback) in MouseCallbacks)
            {
                if (Api.Mouse.IsButtonPressed(button))
                    callback(deltaTime);
            }
        }

        public void AddKeys(params (Key, Action<double>)[] callbacks)
        {
            foreach (var (key, callback) in callbacks) KeyboardCallbacks.Add(key, callback);
        }

        public void AddKeyUps(params (Key, Action)[] callbacks)
        {
            foreach (var (key, callback) in callbacks)
            {
                Api.Keyboard.KeyUp += (_, k, _) =>
                {
                    if (k == key) callback();
                };
            }
        }
        public void AddKeyDowns(params (Key, Action)[] callbacks)
        {
            foreach (var (key, callback) in callbacks)
            {
                Api.Keyboard.KeyDown += (_, k, _) =>
                {
                    if (k == key) callback();
                };
            }
        }

        public void AddButtons(params (MouseButton, Action<double>)[] callbacks)
        {
            foreach (var (button, callback) in callbacks) MouseCallbacks.Add(button, callback);
        }

        public void AddMouseMoves(params Action<Vector2D<float>>[] callbacks)
        {
            foreach (var callback in callbacks)
                Api.Mouse.MouseMove += (_, position) => callback(new Vector2D<float>(position.X, position.Y));
        }
        public void AddMouseScrolls(params Action<ScrollWheel>[] callbacks)
        {
            foreach (var callback in callbacks)
                Api.Mouse.Scroll += (_, scroll) => callback(scroll);
        }
    }
}
