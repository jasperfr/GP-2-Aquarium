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
    public partial class Form1 : Form
    {
        public World MainWorld;

        public Form1()
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
    }
}
