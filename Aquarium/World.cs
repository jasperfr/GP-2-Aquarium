using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
        public List<MovingEntity> MovingEntities = new List<MovingEntity>();
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
            Entities.ForEach(ent => ent.Update());
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
            if(entity.GetType() == typeof(MovingEntity)) {
                MovingEntities.Remove((MovingEntity) entity);
            }
        }

        public List<Entity> GetEntitiesByTag(string tag)
        {
            return Entities.Where(e => e.Tag == tag).ToList();
        }

        public List<MovingEntity> GetMovingEntitiesByTag(string tag)
        {
            return MovingEntities.Where(e => e.Tag == tag).ToList();
        }

        public MovingEntity GetNearestByTag(string tag, Vector2 position)
        {
            float nearestDistance = 4294967295f;
            MovingEntity nearest = null;
            foreach(MovingEntity me in MovingEntities.Where(m => m.Tag == tag).ToList())
            {
                float pos = Vector2.Distance(position, me.Position);
                if(pos < nearestDistance)
                {
                    nearestDistance = pos;
                    nearest = me;
                }
            }
            return nearest;
        }

        public void AddStateMachine(StateMachine sm)
        {
            StateMachines.Add(sm);
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            if(entity.GetType() == typeof(MovingEntity)) {
                MovingEntities.Add((MovingEntity) entity);
            }
        }
    }
}
