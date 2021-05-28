using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
