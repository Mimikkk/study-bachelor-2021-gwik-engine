using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.entities;
using Sokoban.utilities;

namespace Sokoban.engine.input
{
    internal class CameraController : Controller
    {
        private Camera Camera { get; }
        public CameraController(Camera camera)
        {
            Camera = camera;

            AddHold(Key.W, MoveForward);
            AddHold(Key.S, MoveBackward);
            AddHold(Key.A, StrafeLeft);
            AddHold(Key.D, StrafeRight);
            AddHold(Key.Q, RotateLeft);
            AddHold(Key.E, RotateRight);
            AddHold(Key.Space, FloatUp);
            AddHold(Key.ControlLeft, FloatDown);
            AddHold(Key.Keypad0, LookAtStart);
            AddMove(UpdateMouseRotation);
            IsActive = true;
        }

        private void UpdateMouseRotation(Vector2D<float> position)
        {
            if (Game.LastMousePosition == default) return;
            var offset = (position - Game.LastMousePosition) * 0.1f;
            Camera.Transform.Turn(MathHelper.DegreesToRadians(offset.X));
            Camera.Transform.Rotate(MathHelper.DegreesToRadians(offset.Y), Vector3D<float>.UnitX);
        }
        private void LookAtStart(float dt)
        {
            $"Pos: {Camera.Position}".LogLine();
            Camera.Lens.LookAt(new Vector3D<float>(0, 0, 0));
        }
        private void FloatUp(float dt) => Camera.Transform.Translate(Vector3D<float>.UnitY * dt);
        private void FloatDown(float dt) => Camera.Transform.Translate(-Vector3D<float>.UnitY * dt);
        private void RotateRight(float dt) => Camera.Transform.Rotate(-dt, Vector3D<float>.UnitZ);
        private void RotateLeft(float dt) => Camera.Transform.Rotate(dt, Vector3D<float>.UnitZ);
        private void StrafeRight(float dt) => Camera.Transform.Translate(Vector3D.Cross(Camera.Transform.Forward, Camera.Transform.Up) * dt);
        private void StrafeLeft(float dt) => Camera.Transform.Translate(-Vector3D.Cross(Camera.Transform.Forward, Camera.Transform.Up) * dt);
        private void MoveBackward(float dt) => Camera.Transform.Translate(-Camera.Transform.Forward * dt);
        private void MoveForward(float dt) => Camera.Transform.Translate(Camera.Transform.Forward * dt);
    }
}