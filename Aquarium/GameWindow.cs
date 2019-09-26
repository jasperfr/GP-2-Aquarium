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
            Game.keyboard_event(e);
        }
    }
}
