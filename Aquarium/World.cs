using NLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Aquarium
{
    public static class Debug
    {

        public enum Error
        {
            UNDEFINED_SPRITE,
            UNDEFINED_OBJECT
        }

        public static void Log(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[DEBUG] " + value);
            Console.ResetColor();
        }

        public static void ShowError(Error error, string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            switch(error)
            {
                case Error.UNDEFINED_SPRITE:
                    Console.WriteLine("ERROR: Attempt to call undefined sprite " + value + ".");
                    break;
                case Error.UNDEFINED_OBJECT:
                    Console.WriteLine("ERROR: Attempt to call undefined object \"" + value + "\".");
                    break;
            }
            Console.ResetColor();
        }
    }

    public class World
    {
        public Random Rand = new Random();

        // Colections
        public Dictionary<string, dynamic> Globals = new Dictionary<string, dynamic>();
        public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        public Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>();
        public List<EStateMachine> StateMachines = new List<EStateMachine>();

        // Lists
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
        
        /// <summary>
        /// Generates a map from the object provided.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="objName"></param>
        public void GenerateMap(string filename, int size)
        {
            // Hacked together! Fix in new update!!!
            Dictionary<char, string> objects = new Dictionary<char, string>();

            int xpos = 0;
            int ypos = 0;
            string[] lines = File.ReadAllLines(filename);
            for(int y = 0; y < lines.Length; y++)
            {
                // It's an object.
                if(lines[y].StartsWith("!"))
                {
                    char enumerator = lines[y].Split('=')[0].TrimStart('!')[0];
                    string objName = lines[y].Split('=')[1];
                    Console.WriteLine($"{enumerator} => {objName}");
                    objects.Add(enumerator, objName);
                    continue;
                }

                for(int x = 0; x < lines[y].Length; x++)
                {
                    if(lines[y][x] != ' ')
                        CreateInstance(objects[lines[y][x]], xpos, ypos);
                    xpos += size;
                }
                ypos += size;
                xpos = 0;
            }
        }

        public void AddObject(string objName, GameObject obj) {
            obj.Name = objName;
            Objects.Add(objName, obj);
        }
        public GameObject GetObject(string objName) => Objects.TryGetValue(objName, out GameObject entity) ? entity : null;
        
        // Getters and setters of Entity list
        public GameObject CreateInstance(string objName, float x = 0.0f, float y = 0.0f, EStateMachine stateMachine = null)
        {
            GameObject instance = GetObject(objName);
            if(instance == null) {
                Debug.ShowError(Debug.Error.UNDEFINED_OBJECT, objName);
                return null;
            }
            else {
                GameObject clone = instance.Duplicate();
                clone.Position = new Vector2(x, y);
                Entities.Add(clone);

                if(stateMachine != null)
                {
                    stateMachine.Entity = clone;
                    StateMachines.Add(stateMachine);
                    stateMachine.Start();
                }
                return clone;
            }
        }
        public void Destroy(GameObject entity) => Entities.Remove(entity);
        public bool Has(string tag) => Entities.Where(e => e.Tag == tag).Count() > 0;
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
                    output.AddRange(SpatialPartitioningList[Y][X].Where(ent => ent.Tag == tag));
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
        public List<GameObject> GetEntitiesByTagName(string tag) => Entities.Where(e => e.Tag == tag).ToList();
        public GameObject GetNearestByTagName(string tag, Vector2 position) => Entities.Where(e => e.Tag == tag).OrderBy(e => Vector2.Distance(position, e.Position)).First();
        
        // Getters and setters of Sprite list
        public void AddSprite(string name, Sprite sprite) => Sprites.Add(name, sprite);
        public Sprite GetSprite(string name) => Sprites.TryGetValue(name, out Sprite sprite) ? sprite : null;

        // Global game functions
        public void TriggerKeyboardEvent(KeyEventArgs key)
        {
            Entities.ForEach(ent => ent.FireKeyboardEvent(key));
        }
        public void Update()
        {
            Entities.ForEach(ent => MoveInSpatialField(ent)); // Move game objects in spatial field.
            StateMachines.ForEach(sm => sm.UpdateState()); // Update state machines.
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
                Entities.ForEach(e => e.Render(g, e.Tag == "shark" && ShowDebug)); // Render entities.
            }
        }
    }
}
