using Aquarium.AI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Aquarium.Instances
{
    public static class Game
    {
        public static GameWindow window;
        public static Random random = new Random();
        public static Dictionary<string, dynamic> global = new Dictionary<string, dynamic>();
        public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        public static Dictionary<string, Instance> instances = new Dictionary<string, Instance>();
        public static List<GameInstance> game_instances = new List<GameInstance>();
        public static List<StateMachine> state_machines = new List<StateMachine>();

        public static int sp_grid_size = 32, sp_width, sp_height;
        public static List<GameInstance>[][] sp_list;

        public static int speed
        {
            get => window.TickHandle.Interval;
            set => window.TickHandle.Interval = value;
        }
        public static int grid_size
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

        public static void start()
        {
            game_instances.ForEach(inst => inst.CreateEvent());
            window.TickHandle.Start();
            Application.Run(window);
        }
        public static void add_sprite(string name, Sprite sprite) => sprites.Add(name, sprite);
        public static Sprite get_sprite(string name) => sprites.TryGetValue(name, out Sprite sprite) ? sprite : null;
        public static void add_object(string name, Instance instance) { instance.name = name; instances.Add(name, instance); }
        public static GameInstance create_instance(float x, float y, string name, StateMachine state_machine = null)
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
                return null;
            }
        }
        public static List<GameInstance> find_all_instances(string name) => game_instances.Where(gi => gi.name == name).ToList();
        public static GameInstance find_nearest_instance(string name, Vector2 position) => game_instances.Where(e => e.name == name).OrderBy(e => Vector2.Distance(position, e.position)).First();
        public static void move_spatial(GameInstance instance)
        {
            // Remove the entity from the spatial list.
            sp_list[instance.spatial_y][instance.spatial_x].Remove(instance);

            // Get the new spatial position.
            int x = (int)Math.Floor(instance.position.X / sp_grid_size);
            int y = (int)Math.Floor(instance.position.Y / sp_grid_size);

            // Update the entity's spatial position.
            instance.spatial_x = x;
            instance.spatial_y = y;

            // Add the entity to the spatial list.
            sp_list[y][x].Add(instance);
        }
        public static List<GameInstance> get_adjacent_in_spatial_field(GameInstance instance)
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
        public static void keyboard_event(KeyEventArgs kva)
        {
            game_instances.ForEach(inst => inst.KeyboardEvent(kva));
        }
        public static void update()
        {
            game_instances.ForEach(inst => move_spatial(inst));
            state_machines.ForEach(sm => sm.UpdateState());
            game_instances.ForEach(inst => inst.StepEvent());
            game_instances.ForEach(inst => inst.Update());
            window.Invalidate();
        }
        public static void render(Graphics g)
        {
            game_instances.ForEach(inst => inst.Render(g));
        }
    }
}
