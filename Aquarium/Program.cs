using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aquarium.Instances;
using NLua;

namespace Aquarium
{
    static class Program
    {
        public static Lua state = new Lua();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game game = new Game();
            game.window = new GameWindow(game);

            // Let the rest be handled by the Lua file.
            state.LoadCLRPackage();
            state["game"] = game;
            object res = state.DoFile(@"lua\main.lua")[0];
            Console.WriteLine(res);
        }
    }
}
