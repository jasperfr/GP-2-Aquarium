using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.AI
{
    public class Location
    {
        public Location Previous;
        public int X, Y, F, G, H;
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public List<Location> GetAdjacent(int width, int height)
        {
            var proposed = new List<Location>()
            {
                new Location(X, Y - 1),
                new Location(X, Y + 1),
                new Location(X + 1, Y),
                new Location(X - 1, Y)
            };

            return proposed.Where(l => l.X >= 0 && l.X <= width && l.Y >= 0 && l.Y <= height).ToList();
        }
    }

    public static class Pathfinding
    {
        public static float grid = 32.0f;

        /*
        public static List<Vector2> AStar(Vector2 start, Vector2 target, List<Vector2> locations)
        {
            Location current = null;
            Location start = new Location(start.X, start.Y);
        }
        */
    }
}
