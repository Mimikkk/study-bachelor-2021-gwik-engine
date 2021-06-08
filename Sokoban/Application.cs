using Sokoban.utilities;

namespace Sokoban
{
internal static class Application
{
    private static void Main()
    {
        Logger.InitializeLogging();
        Game.Start();
    }
}
}