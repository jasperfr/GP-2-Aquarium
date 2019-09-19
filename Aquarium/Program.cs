using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GameWindow form = new GameWindow();
            World game = new World(form);

            // Let the rest be handled by the Lua file.
            state.LoadCLRPackage();
            state["game"] = game;
            var res = state.DoFile("lua/main.lua")[0];
            Console.WriteLine(res);
        }
    }
}
