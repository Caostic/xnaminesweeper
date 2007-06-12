using System;

namespace Minesweeper
{
    static class MinesweeperProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MinesweeperGame game = new MinesweeperGame())
            {
                game.Run();
            }
        }
    }
}

