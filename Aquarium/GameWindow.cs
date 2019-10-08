using Aquarium.Instances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aquarium
{
    public partial class GameWindow : Form
    {
        private DateTime _lastCheck = DateTime.Now;
        private long _frameCount = 0;

        private Game _game;

        public GameWindow(Game game)
        {
            _game = game;
            InitializeComponent();
            // double buffer hack
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic, null, DrawPanel, new object[] { true });
        }

        private void TickHandle_Tick(object sender, EventArgs e)
        {
            _game.update();

            // add FPS
            Interlocked.Increment(ref _frameCount);
            double secondsElapsed = (DateTime.Now - _lastCheck).TotalSeconds;
            long count = Interlocked.Exchange(ref _frameCount, 0);
            double fps = count / secondsElapsed;
            _lastCheck = DateTime.Now;
            LabelDebug.Text = $"{string.Format("{0:0}", fps)} FPS";

            // add entity count
            LabelDebug.Text += $" | {_game.instances.Count()} instances";
            LabelDebug.Text += $" | {_game.game_instances.Count()} entities";
            LabelDebug.Text += $" | {_game.sp_grid_size}x{_game.sp_grid_size}spatial grid";
            LabelDebug.Text += $" | {_game.state_machines.Count()} state machines ";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GameWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F11)
            {
                ToggleFullScreen();
            }
            _game.keyboard_event(e);
        }
        private bool _fullscreen = false;
        private void ToggleFullScreen()
        {
            _fullscreen ^= true;
            if(_fullscreen)
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            _game.render(e.Graphics);
        }
    }
}
