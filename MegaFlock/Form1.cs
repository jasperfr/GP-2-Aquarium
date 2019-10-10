using System;
using System.Drawing;
using System.Windows.Forms;

namespace MegaFlock
{
    public partial class Form1 : Form
    {
        private Brush _b = Brushes.Black;
        private World _world;

        public Form1()
        {
            InitializeComponent();
            _world = new World(this);
            FormTimer.Interval = 1;
            FormTimer.Start();

            Random rand = new Random();
            for(int i = 0; i < 3000; i++)
            {
                _world.AddItem(rand.Next(Width), rand.Next(Height));
            }
        }

        private void FormTimer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _world.Update(_b, e.Graphics);
        }
    }
}
