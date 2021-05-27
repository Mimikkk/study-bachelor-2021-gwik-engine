namespace GWiK_Sokoban.engine.interfaces
{
    public interface IRenderable
    {
        public bool IsConfigured { get; set; }

        public void Render();
        public void MaybeRender()
        {
            if (IsConfigured) Render();
        }
    }
}