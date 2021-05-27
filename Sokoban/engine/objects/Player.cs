using Sokoban.engine.input;
using Sokoban.primitives.components;

namespace Sokoban.engine.objects
{
    internal class Player : IRenderable, IUpdateable
    {
        public Player(Controller controller)
        {
            Controller = controller;
        }

        public bool IsConfigured { get; set; } = false;

        public void Render()
        {
        }
        public void Update(double deltaTime)
        {
        }

        private Controller Controller { get; }
    }
}