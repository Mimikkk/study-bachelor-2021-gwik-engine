using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.entities;
using Sokoban.utilities;

namespace Sokoban.engine.input
{
    internal class GameController : Controller
    {
        public GameController()
        {
            AddRelease(Key.Escape, CloseGame);
            AddMove(UpdateMousePosition);
            IsActive = true;
        }
        
        private static void CloseGame() => Game.GameWindow.Close();
        private void UpdateMousePosition(Vector2D<float> position)
        {
            $"Last Position: <c19 {Game.LastMousePosition}|>, New Position: <c19 {position}|>, Difference: <c9 {Game.LastMousePosition - position}|>".LogLine();
            Game.LastMousePosition = position;
        }
    }
}