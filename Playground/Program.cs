using System;
using MonoEngine2D.Engine.Root;

namespace Playground
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameRoot())
                game.Run();
        }
    }
}
