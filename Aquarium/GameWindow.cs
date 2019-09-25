using Aquarium.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aquarium
{
    public partial class GameWindow : Form
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private void TickHandle_Tick(object sender, EventArgs e)
        {
            Game.update();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Game.render(e.Graphics);
        }

        private void GameWindow_KeyUp(object sender, KeyEventArgs e)
        {
            /* -- the keyboard event has been deprecated
            if(e.KeyCode == Keys.F1)
            {
                MainWorld.ShowEntities ^= true;
            }
            if(e.KeyCode == Keys.F3)
            {
                MainWorld.ShowDebug ^= true;
            }
            if(e.KeyCode == Keys.F4)
            {
                MainWorld.ShowDebugGrid ^= true;
            }
            MainWorld.TriggerKeyboardEvent(e);
            */
        }
    }
}
