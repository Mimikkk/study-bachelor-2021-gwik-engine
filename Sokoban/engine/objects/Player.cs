using System;
using GWiK_Sokoban.engine.input;
using GWiK_Sokoban.engine.interfaces;

namespace GWiK_Sokoban.engine.objects
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