using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Aquarium
{
    public class World
    {
        // Lists
        public Dictionary<string, dynamic> GlobalVariables = new Dictionary<string, dynamic>();
        public List<StateMachine> StateMachines = new List<StateMachine>();
        public List<GameObject> Entities = new List<GameObject>();
        
        // Object variables
        public GameWindow Window;
        public bool ShowDebug = false;
        
        // Constructors
        public World(GameWindow window)
        {
            Window = window;
            Window.MainWorld = this;
        }
        
        // Object functions
        public void SetTickSpeed(int tickSpeed) => Window.TickHandle.Interval = tickSpeed;
        public void Start()
        {
            Window.TickHandle.Start();
            Application.Run(Window);
        }
        
        // Getters and setters of State Machine list
        public void AddStateMachine(StateMachine sm) => StateMachines.Add(sm);
        
        // Getters and setters of Entity list
        public void AddEntity(GameObject entity) => Entities.Add(entity);
        public void DestroyEntity(GameObject entity) => Entities.Remove(entity);

        // Get / Filter functions of Entity list
        public List<GameObject> GetEntitiesByTagName(string tag) => Entities.Where(e => e.GroupTag == tag).ToList();
        public GameObject GetNearestByTagName(string tag, Vector2 position) => Entities.Where(e => e.GroupTag == tag).OrderBy(e => Vector2.Distance(position, e.Position)).First();
        
        // Global game functions
        public void Update()
        {
            StateMachines.ForEach(sm => sm.Update()); // Update state machines.
            Entities.ForEach(ent => ent.Update()); // Update game objects.
            Window.Invalidate(); // Invalidate the window.
        }
        public void Render(Graphics g) => Entities.ForEach(e => e.Render(g, ShowDebug));
    }
}
