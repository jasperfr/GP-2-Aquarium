using Aquarium.Instances;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aquarium.Helpers
{
    public static class MapLoader
    {
        public static void Load(Game game, string filename, int size)
        {
            // Hacked together! Fix in new update!!!
            Dictionary<char, string> objects = new Dictionary<char, string>();

            int xpos = 0;
            int ypos = 0;
            string[] lines = File.ReadAllLines(filename);
            for (int y = 0; y < lines.Length; y++)
            {
                // It's an object.
                if (lines[y].StartsWith("!"))
                {
                    char enumerator = lines[y].Split('=')[0].TrimStart('!')[0];
                    string objName = lines[y].Split('=')[1];
                    Console.WriteLine($"{enumerator} => {objName}");
                    objects.Add(enumerator, objName);
                    continue;
                }

                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] != ' ')
                        game.create_instance(xpos, ypos, objects[lines[y][x]]);
                    xpos += size;
                }
                ypos += size;
                xpos = 0;
            }
        }
    }
}
