using System;
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
        public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        public Dictionary<string, dynamic> GlobalVariables = new Dictionary<string, dynamic>();
        public List<StateMachine> StateMachines = new List<StateMachine>();
        public List<GameObject>[][] SpatialPartitioningList;
        public List<GameObject> Entities = new List<GameObject>();
        
        // Object variables
        public GameWindow Window;
        public bool ShowDebug = false;
        public bool ShowDebugGrid = false;
        public bool ShowEntities = true;
        public int GridSize = 32;
        public double GridDivisor = 0.0; // Prevents division calls
        public int SpatialWidth = 0;
        public int SpatialHeight = 0;
        
        // Constructors
        public World(GameWindow window)
        {
            Window = window;
            Window.MainWorld = this;
        }
        
        // Object functions
        public void SetTickSpeed(int tickSpeed) => Window.TickHandle.Interval = tickSpeed;
        public void SetSpatialGridSize(int size)
        {
            GridSize = size;
            GridDivisor = 1.0 / size;
            SpatialHeight = Window.Height / size + 1;
            SpatialWidth = Window.Width / size + 1;
            SpatialPartitioningList = new List<GameObject>[SpatialHeight][];
            for(int y = 0; y < SpatialHeight; y++) {
                SpatialPartitioningList[y] = new List<GameObject>[SpatialWidth];
                for(int x = 0; x < SpatialWidth; x++) {
                    SpatialPartitioningList[y][x] = new List<GameObject>();
                }
            }
        }
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
        public bool Has(string tag) => Entities.Where(e => e.GroupTag == tag).Count() > 0;
        public bool Exists(GameObject entity) => Entities.Where(e => e.Equals(entity)).Count() > 0;

        // Getters and (internal) setters of Spatial Partitioning list
        public void MoveInSpatialField(GameObject entity)
        {
            // Remove the entity from the spatial list.
            SpatialPartitioningList[entity.SpatialY][entity.SpatialX].Remove(entity);

            // Get the new spatial position.
            int x = (int) Math.Floor(entity.Position.X * GridDivisor); // / 32.0);
            int y = (int) Math.Floor(entity.Position.Y * GridDivisor); // / 32.0);
            
            // Update the entity's spatial position.
            entity.SpatialX = x;
            entity.SpatialY = y;

            // Add the entity to the spatial list.
            SpatialPartitioningList[y][x].Add(entity);
        }
        public List<GameObject> GetAdjacentEntities(GameObject entity)
        {
            List<GameObject> output = new List<GameObject>();

            int x = (int) Math.Floor(entity.Position.X * GridDivisor); // / 32.0);
            int y = (int) Math.Floor(entity.Position.Y * GridDivisor); // / 32.0);

            // Complexity O(N * 9)
            for(int Y = y - 1; Y <= y + 1; Y++) {
                for(int X = x - 1; X <= x + 1; X++) {
                    output.AddRange(SpatialPartitioningList[Y][X]);
                }
            }

            return output;
        }
        public List<GameObject> GetAdjacentEntitiesByTagName(GameObject entity, string tag)
        {
            List<GameObject> output = new List<GameObject>();

            int x = (int) Math.Floor(entity.Position.X * GridDivisor); // / 32.0);
            int y = (int) Math.Floor(entity.Position.Y * GridDivisor); // / 32.0);

            // Complexity O(N * 9)
            for(int Y = y - 1; Y <= y + 1; Y++) {
                for(int X = x - 1; X <= x + 1; X++) {
                    if(Y < 0 || X < 0 || Y >= SpatialHeight || X >= SpatialWidth) continue;
                    output.AddRange(SpatialPartitioningList[Y][X].Where(ent => ent.GroupTag == tag));
                }
            }

            return output;
        }
        public Tuple<int, int> GetCoordinatesInSpatialField(GameObject entity)
        {
            int x = (int) entity.Position.X / 32;
            int y = (int) entity.Position.Y / 32;
            return Tuple.Create(x, y);
        }

        // Get / Filter functions of Entity list
        public List<GameObject> GetEntitiesByTagName(string tag) => Entities.Where(e => e.GroupTag == tag).ToList();
        public GameObject GetNearestByTagName(string tag, Vector2 position) => Entities.Where(e => e.GroupTag == tag).OrderBy(e => Vector2.Distance(position, e.Position)).First();
        
        // Getters and setters of Sprite list
        public void AddSprite(string name, Sprite sprite) => Sprites.Add(name, sprite);
        public Sprite GetSprite(string name) => Sprites.TryGetValue(name, out Sprite sprite) ? sprite : null;

        // Global game functions
        public void Update()
        {
            Entities.ForEach(ent => MoveInSpatialField(ent)); // Move game objects in spatial field.
            StateMachines.ForEach(sm => sm.Update()); // Update state machines.
            Entities.ForEach(ent => ent.Update()); // Update game objects.
            Window.Invalidate(); // Invalidate the window.
        }
        public void Render(Graphics g)
        {
            if(ShowDebugGrid)
            {
                /*
                for(int x = 0; x < SpatialWidth; x++) {
                    g.DrawLine(Pens.Lime, x * GridSize, 0, x * GridSize, 720);
                }
                for(int y = 0; y < SpatialHeight; y++) {
                    g.DrawLine(Pens.Lime, 0, y * GridSize, 1280, y * GridSize);
                }
                */

                for(int y = 0; y < SpatialHeight; y++)
                {
                    for(int x = 0; x < SpatialWidth; x++)
                    {
                        int color = SpatialPartitioningList[y][x].Count * 16;
                        if(color > 255) color = 255;
                        SolidBrush brush = new SolidBrush(Color.FromArgb(color, 0, 255, 255));
                        g.FillRectangle(brush, x * GridSize, y * GridSize, GridSize, GridSize);
                        brush.Dispose();
                    }
                }
            }
            if(ShowEntities)
            {
                Entities.ForEach(e => e.Render(g, e.GroupTag == "shark" && ShowDebug)); // Render entities.
            }
        }
    }
}
