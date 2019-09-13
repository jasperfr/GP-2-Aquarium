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
        public World MainWorld;

        public GameWindow()
        {
            InitializeComponent();
        }

        private void TickHandle_Tick(object sender, EventArgs e)
        {
            MainWorld.Update();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            MainWorld.Render(e.Graphics);
        }

        private void GameWindow_KeyUp(object sender, KeyEventArgs e)
        {
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
        }
    }
}
