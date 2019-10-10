using Aquarium.AI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Aquarium.Instances
{
    public class Game
    {
        public GameWindow window;
        public Random random = new Random();
        public Dictionary<string, dynamic> global = new Dictionary<string, dynamic>();
        public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public Dictionary<string, Instance> instances = new Dictionary<string, Instance>();
        public List<GameInstance> game_instances = new List<GameInstance>();
        public List<StateMachine> state_machines = new List<StateMachine>();

        public int sp_grid_size = 32, sp_width, sp_height;
        public List<GameInstance>[][] sp_list;

        public Location[][] renderable_grid;

        public int speed
        {
            get => window.TickHandle.Interval;
            set => window.TickHandle.Interval = value;
        }
        public int grid_size
        {
            get => sp_grid_size;
            set {
                sp_grid_size = value;
                sp_width = window.Width / sp_grid_size + 1;
                sp_height = window.Height / sp_grid_size + 1;
                sp_list = new List<GameInstance>[sp_height][];
                for (int y = 0; y < sp_height; y++)
                {
                    sp_list[y] = new List<GameInstance>[sp_width];
                    for (int x = 0; x < sp_width; x++)
                    {
                        sp_list[y][x] = new List<GameInstance>();
                    }
                }
            }
        }

        public Game()
        {

        }

        public void start()
        {
            game_instances.ForEach(inst => inst.CreateEvent());
            window.TickHandle.Start();
            Application.Run(window);
        }
        public void add_sprite(string name, Sprite sprite) => sprites.Add(name, sprite);
        public Sprite get_sprite(string name)
        {
            if (sprites.TryGetValue(name, out Sprite sprite))
            {
                return sprite;
            }
            else
            {
                return null;
            }
        }
        public void add_instance(string name, Instance instance) {
            instance.name = name;
            instances.Add(name, instance);
        }
        public GameInstance create_instance(float x, float y, string name, StateMachine state_machine = null)
        {
            if (instances.TryGetValue(name, out Instance instance))
            {
                GameInstance game_object = instance.Create(x, y);
                game_instances.Add(game_object);
                
                if(state_machine != null)
                {
                    state_machine.Entity = game_object;
                    state_machines.Add(state_machine);
                    state_machine.Start();
                }

                return game_object;
            }
            else
            {
                Console.WriteLine("Error: No such instance exists: " + name);
                return null;
            }
        }
        public bool exists(GameInstance instance) => game_instances.Contains(instance);
        public void destroy(GameInstance instance) => game_instances.Remove(instance);
        public List<GameInstance> find_all_instances(string name) => game_instances.Where(gi => gi.name == name).ToList();
        public GameInstance find_nearest_instance(string name, Vector2 position) => game_instances.Where(e => e.name == name).OrderBy(e => Vector2.DistanceSquared(position, e.position)).FirstOrDefault();
        public void move_spatial(GameInstance instance)
        {
            // Remove the entity from the spatial list.
            sp_list[instance.spatial_y][instance.spatial_x].Remove(instance);

            if (instance.position.X < 0) instance.position.X = 1280;
            if (instance.position.X > 1280) instance.position.X = 0;
            if (instance.position.Y < 0) instance.position.Y = 720;
            if (instance.position.Y > 720) instance.position.Y = 0;

            // Get the new spatial position.
            int x = (int)Math.Floor(instance.position.X / sp_grid_size);
            int y = (int)Math.Floor(instance.position.Y / sp_grid_size);

            // Update the entity's spatial position.
            instance.spatial_x = x;
            instance.spatial_y = y;

            // Add the entity to the spatial list.
            sp_list[y][x].Add(instance);
        }
        public List<GameInstance> get_adjacent_in_spatial_field(GameInstance instance)
        {
            List<GameInstance> output = new List<GameInstance>();

            int x = (int)Math.Floor(instance.position.X / sp_grid_size);
            int y = (int)Math.Floor(instance.position.Y / sp_grid_size);

            // Complexity O(N * 9)
            for (int Y = y - 1; Y <= y + 1; Y++)
            {
                for (int X = x - 1; X <= x + 1; X++)
                {
                    if (Y < 0 || X < 0 || Y >= sp_height || X >= sp_width) continue;
                    output.AddRange(sp_list[Y][X].Where(ent => ent.name == instance.name));
                }
            }

            return output;
        }
        public void keyboard_event(KeyEventArgs kva)
        {
            game_instances.ForEach(inst => inst.KeyboardEvent(kva));
        }
        public void update()
        {
            window.LblDebugInfo.Text = "";
            game_instances.ForEach(inst => move_spatial(inst));
            state_machines.ForEach(sm => sm.UpdateState());
            game_instances.ForEach(inst => inst.StepEvent());
            game_instances.ForEach(inst => inst.Update());
            window.DrawPanel.Invalidate();
        }
        public void render(Graphics g)
        {
            game_instances.ForEach(inst => inst.Render(g));
            if(renderable_grid != null)
            {
                PathfindAPI.Render(g, renderable_grid);
            }
        }
        public void log(string str)
        {
            window.LblDebugInfo.Text += str + '\n';
        }
    }
}
