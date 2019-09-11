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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Lua state = new Lua();
            Form1 form = new Form1();
            World game = new World(form);

            state.LoadCLRPackage();
            state["game"] = game;
            var res = state.DoFile("Application.lua")[0];
            Console.WriteLine(res);
        }
    }
}
