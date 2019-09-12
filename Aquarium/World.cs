using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aquarium
{
    public class World
    {
        public Dictionary<string, dynamic> Globals = new Dictionary<string, dynamic>();
        public List<StateMachine> StateMachines = new List<StateMachine>();
        public List<Entity> Entities = new List<Entity>();
        public Form1 Window;

        public World(Form1 window)
        {
            Window = window;
            Window.MainWorld = this;
        }

        public void SetTickSpeed(int tickSpeed)
        {
            Window.TickHandle.Interval = tickSpeed;
        }

        public void Update()
        {
            StateMachines.ForEach(sm => sm.Update());
            Window.Invalidate();
        }

        public void Render(Graphics g)
        {
            Entities.ForEach(e => e.Render(g));
        }

        public void Start()
        {
            Window.TickHandle.Start();
            Application.Run(Window);
        }

        public void Destroy(Entity entity)
        {
            Entities.Remove(entity);
        }

        public List<Entity> GetEntitiesByTag(string tag)
        {
            return Entities.Where(e => e.Tag == tag).ToList();
        }

        public void AddStateMachine(StateMachine sm)
        {
            StateMachines.Add(sm);
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }
    }
}
