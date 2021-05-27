using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using Silk.NET.Input;

namespace GWiK_Sokoban.engine.input
{
    internal class Controller
    {
        private readonly Dictionary<Key, Action<double>> KeyboardCallbacks = new();
        private readonly Dictionary<MouseButton, Action<double>> MouseCallbacks = new();

        public void Update(double deltaTime)
        {
            foreach (var (key, callback) in KeyboardCallbacks)
            {
                if (Game.Keyboard.IsKeyPressed(key))
                    callback(deltaTime);
            }
            foreach (var (button, callback) in MouseCallbacks)
            {
                if (Game.Mouse.IsButtonPressed(button))
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
                Game.Keyboard.KeyUp += (_, k, _) =>
                {
                    if (k == key) callback();
                };
            }
        }
        public void AddKeyDowns(params (Key, Action)[] callbacks)
        {
            foreach (var (key, callback) in callbacks)
            {
                Game.Keyboard.KeyDown += (_, k, _) =>
                {
                    if (k == key) callback();
                };
            }
        }

        public void AddButtons(params (MouseButton, Action<double>)[] callbacks)
        {
            foreach (var (button, callback) in callbacks) MouseCallbacks.Add(button, callback);
        }

        public void AddMouseMoves(params Action<Vector2>[] callbacks)
        {
            foreach (var callback in callbacks)
                Game.Mouse.MouseMove += (_, position) => callback(position);
        }
        public void AddMouseScrolls(params Action<ScrollWheel>[] callbacks)
        {
            foreach (var callback in callbacks)
                Game.Mouse.Scroll += (_, scroll) => callback(scroll);
        }
    }
}
