namespace Sokoban.primitives.components
{
    public interface IRenderable
    {
        public bool IsConfigured { get; set; }

        public void Render();
    }
}
