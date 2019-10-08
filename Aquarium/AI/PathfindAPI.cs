using Aquarium.Instances;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI
{
    public class Location
    {
        public Location Previous;
        public bool Disabled = false;
        public int X, Y, F, G, H;
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
        public List<Location> Adjacent(Location[][] locations)
        {
            int W = locations[0].Length;
            int H = locations.Length;
            var proposed = new List<Location>();
            if (X > 0) if (!locations[Y][X - 1].Disabled) proposed.Add(locations[Y][X - 1]);
            if (Y > 0) if (!locations[Y - 1][X].Disabled) proposed.Add(locations[Y - 1][X]);
            if (X < W-1) if (!locations[Y][X + 1].Disabled) proposed.Add(locations[Y][X + 1]);
            if (Y < H-1) if (!locations[Y + 1][X].Disabled) proposed.Add(locations[Y + 1][X]);
            return proposed;
        }
        public void CalculateH(Location target)
        {
            H = Math.Abs(target.X - X) + Math.Abs(target.Y - Y);
        }
    }

    public static class PathfindAPI
    {
        public static int _gridSize = 32;

        /// <summary>
        /// Generates a new grid map of locations.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Location[][] generate(int width, int height)
        {
            // generate map
            Location[][] locations = new Location[height][];
            for(int y = 0; y < locations.Length; y++)
            {
                locations[y] = new Location[width];
            }

            // add values
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    locations[y][x] = new Location(x, y);
                }
            }

            return locations;
        }

        /// <summary>
        /// Disables nodes that contain instances. Note: Experimental (and heavy). Do not use this in update ticks.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="ignoreInstance"></param>
        public static void disable_at(ref Location[][] locations, Game game, string ignoreInstance)
        {
            List<GameInstance> instances = game.find_all_instances(ignoreInstance);
            foreach(GameInstance instance in instances)
            {
                // get the borders
                float left = instance.position.X - instance.image_size * 0.5f;
                float right = instance.position.X + instance.image_size * 0.5f;
                float top = instance.position.Y - instance.image_size * 0.5f;
                float down = instance.position.Y + instance.image_size * 0.5f;

                // complexity O(N^2)*instance_count.
                // don't worry though, you'll only have to call this function once.
                for (int y = 0; y < locations.Length; y++)
                {
                    for(int x = 0; x < locations[0].Length; x++)
                    {
                        // adjust positions
                        float xpos = locations[y][x].X * game.grid_size;
                        float ypos = locations[y][x].Y * game.grid_size;

                        // tricky part
                        if (xpos >= left && xpos <= right && ypos >= top && ypos <= down) locations[y][x].Disabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// You should by now have a configured location 2d array grid. This function will plan a path from start to end, using the A*-algorithm.
        /// <para>
        /// ( It might not actually be A*. )
        /// </para>
        /// </summary>
        /// <param name="configuredLocations"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Stack<Vector2> a_star(Location[][] configuredLocations, Vector2 start, Vector2 end)
        {
            // create output list
            Stack<Vector2> output = new Stack<Vector2>();

            Location currentLocation = null;

            // get the nearest location to the start by obtaining grid pos
            // warning - might throw exceptions
            var startx = (int)Math.Floor((decimal)((int)start.X / _gridSize));
            var starty = (int)Math.Floor((decimal)((int)start.Y / _gridSize));
            Location startLocation = configuredLocations[starty][startx];

            // get the nearest location to the end
            // warning - might throw exceptions
            var endx = (int)Math.Floor((decimal)((int)end.X / _gridSize));
            var endy = (int)Math.Floor((decimal)((int)end.Y / _gridSize));
            Location endLocation = configuredLocations[endy][endx];

            // make an open and closed list
            var openList = new List<Location>();
            var closedList = new List<Location>();

            int g = 0;
            // the values A* uses are:
            // startLocation
            // currentLocation
            // endLocation
            // openList
            // closedList
            // g

            // okay now the calculating magic sorcery
            openList.Add(startLocation);

            while(openList.Count > 0)
            {
                var lowest = openList.Min(l => l.F);
                currentLocation = openList.First(l => l.F == lowest);

                closedList.Add(currentLocation);
                openList.Remove(currentLocation);

                if (closedList.FirstOrDefault(l => l.X == endLocation.X && l.Y == endLocation.Y) != null)
                    break;

                var adjacentLocations = currentLocation.Adjacent(configuredLocations);
                g++;

                foreach(var adjacentLocation in adjacentLocations)
                {
                    if (closedList.FirstOrDefault(l => l.X == adjacentLocation.X && l.Y == adjacentLocation.Y) != null)
                        continue;

                    if (openList.FirstOrDefault(l => l.X == adjacentLocation.X && l.Y == adjacentLocation.Y) == null)
                    {
                        adjacentLocation.G = g;
                        adjacentLocation.CalculateH(endLocation);
                        adjacentLocation.F = adjacentLocation.G + adjacentLocation.H;
                        adjacentLocation.Previous = currentLocation;

                        openList.Insert(0, adjacentLocation);
                    }
                    else
                    {
                        if(g + adjacentLocation.H < adjacentLocation.F)
                        {
                            adjacentLocation.G = g;
                            adjacentLocation.F = adjacentLocation.G + adjacentLocation.H;
                            adjacentLocation.Previous = currentLocation;
                        }
                    }
                }
            }

            // add the end vector to the output.
            output.Push(end);

            // Add all locations from end to start.
            var loc = endLocation;
            while(loc.Previous != null)
            {
                Vector2 position = new Vector2(loc.X * _gridSize, loc.Y * _gridSize);
                // prevent out of memory exceptions
                if (output.Contains(position)) return null;
                output.Push(position);
                loc = loc.Previous;
            }
            Console.WriteLine(endLocation);

            // add the start vector to the output.
            output.Push(start);

            // reverse the list.
            output.Reverse();

            // halt build errors for now.
            return output;
        }

        public static void Render(Graphics g, Location[][] locations)
        {
            for (int y = 0; y < locations.Length; y++)
            {
                for (int x = 0; x < locations[0].Length; x++)
                {
                    Location loc = locations[y][x];
                    if (loc.Disabled) g.FillEllipse(Brushes.Red, x * _gridSize - 2.0f, y * _gridSize - 2.0f, 4.0f, 4.0f);
                    else g.FillEllipse(Brushes.Lime, x * _gridSize - 2.0f, y * _gridSize - 2.0f, 4.0f, 4.0f);
                }
            }
        }
    }
}
