using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MegaFlock
{
    public static class Behaviours
    {
        public static Vector2 Flock(Item source, LinkedList<Item> neighbors)
        {
            neighbors.Remove(source);

            // prevent execution if list is empty
            if (neighbors.Count == 0) return new Vector2(0, 0);

            // separation
            Vector2 separationForce = new Vector2(0, 0);
            foreach(Item tgt in neighbors) {
                Vector2 toAgent = source.Position - tgt.Position;
                separationForce += Vector2.Normalize(toAgent / toAgent.Length());
            }

            // cohesion
            Vector2 cohesionForce = new Vector2(0, 0);
            Vector2 centerOfMass = new Vector2(0, 0);
            foreach (Item tgt in neighbors)
            {
                centerOfMass += tgt.Position;
            }
            centerOfMass /= neighbors.Count;
            cohesionForce = Vector2.Normalize(centerOfMass - source.Position) * 5.0f - source.Velocity;

            // alignment
            Vector2 alignmentForce = new Vector2(0, 0);
            Vector2 averageHeading = new Vector2();
            foreach (Item tgt in neighbors)
            {
                averageHeading += tgt.Direction;
            }
            alignmentForce = (averageHeading / neighbors.Count) - source.Direction;

            // calculate flocking force
            return (cohesionForce + alignmentForce + separationForce) * 0.33333f;
        }
    }

    public class Item
    {
        public World WorldPtr;
        public LinkedList<Item> ListPtr;
        public Vector2 Position, Velocity = new Vector2();
        public int SpX, SpY;

        public Vector2 Direction
        {
            get
            {
                if (Velocity.X == 0 && Velocity.Y == 0)
                    return new Vector2(0, 0);
                return Vector2.Normalize(Velocity);
            }
        }

        public Item(World world, float x, float y)
        {
            WorldPtr = world;
            Position = new Vector2(x, y);
            SpX = (int)(Position.X / WorldPtr.GridSize);
            SpY = (int)(Position.Y / WorldPtr.GridSize);
        }
    }

    public class World
    {
        public Form1 FormPtr;
        public float GridSize = 64.0f;
        public List<Item> Items;
        public LinkedList<Item>[][] SpatialGrid;

        public World(Form1 form)
        {
            FormPtr = form;
            Items = new List<Item>();

            int w = (int)(form.Width / GridSize);
            int h = (int)(form.Height / GridSize);
            SpatialGrid = new LinkedList<Item>[h+1][];
            for(int y = 0; y <= h; y++) {
                SpatialGrid[y] = new LinkedList<Item>[w+1];
                for(int x = 0; x <= w; x++) {
                    SpatialGrid[y][x] = new LinkedList<Item>();
                }
            }
        }

        public void AddItem(float x, float y)
        {
            var item = new Item(this, x, y);
            Items.Add(item);
            int spx = (int)(x / GridSize);
            int spy = (int)(y / GridSize);
            var lst = SpatialGrid[spy][spx];
            item.ListPtr = lst;
            lst.AddLast(item);
        }

        public void Update(Brush b, Graphics g)
        {
            // (draw the grid)
            for (int x = 0; x < FormPtr.Width; x += (int) GridSize)
                g.DrawLine(Pens.Gray, x, 0, x, FormPtr.Height);
            for (int y = 0; y < FormPtr.Height; y += (int) GridSize)
                g.DrawLine(Pens.Gray, 0, y, FormPtr.Width, y);

            // O(N)
            Items.ForEach(item =>
            {
                // get old spatial coordinates
                int spxOld = (int)(item.Position.X / GridSize);
                int spyOld = (int)(item.Position.Y / GridSize);

                // calculate velocity
                for(int y = spyOld - 1; y < spyOld + 1; y++) {
                    for(int x = spxOld - 1; x < spxOld + 1; x++) {
                        if(x != -1 && y != -1 && x != SpatialGrid[0].Length && y != SpatialGrid.Length)
                            item.Velocity += Behaviours.Flock(item, SpatialGrid[y][x]) * 0.01f;
                    }
                }

                item.Velocity = item.Velocity.TruncateMin(0.2f);
                item.Velocity = item.Velocity.TruncateMax(0.7f);
                if (float.IsNaN(item.Velocity.X) || float.IsInfinity(item.Velocity.X)) item.Velocity.X = 0.0f;
                if (float.IsNaN(item.Velocity.Y) || float.IsInfinity(item.Velocity.Y)) item.Velocity.Y = 0.0f;

                // calculate position
                item.Position += item.Velocity;
                if (item.Position.X < 16) item.Velocity.X = Math.Abs(item.Velocity.X);
                if (item.Position.Y < 16) item.Velocity.Y = Math.Abs(item.Velocity.Y);
                if (item.Position.X > FormPtr.Width - 16) item.Velocity.X = -Math.Abs(item.Velocity.X);
                if (item.Position.Y > FormPtr.Height - 16) item.Velocity.Y = -Math.Abs(item.Velocity.Y);

                // calculate spatial field
                int spx = (int)(item.Position.X / GridSize);
                int spy = (int)(item.Position.Y / GridSize);
                if (spx < 0) spx = 0; if (spx > SpatialGrid[0].Length - 1) spx = SpatialGrid[0].Length - 1;
                if (spy < 0) spy = 0; if (spy > SpatialGrid.Length - 1) spy = SpatialGrid.Length - 1;

                if (spx != item.SpX || spy != item.SpY)
                {
                    SpatialGrid[spyOld][spxOld].Remove(item);
                    SpatialGrid[spy][spx].AddLast(item);
                }

                // render
                g.FillRectangle(b, item.Position.X, item.Position.Y, 5, 5);
            });
        }
    }
}
