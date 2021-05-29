using Sokoban.utilities;

namespace Sokoban
{
    internal class Test
    {
        private float _field = 7;
        public float Field
        {
            get => _field;
            set => OperationAndSet(ref _field, value);
        }
        private static void OperationAndSet<T>(ref T field, T value)
        {
            field = value;
        }
    }

    internal static class Application
    {
        private static void Main()
        {
            Logger.InitializeLogging();
            Game.Start();
        }
    }
}
