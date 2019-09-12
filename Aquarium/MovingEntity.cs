using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium
{
    public class MovingEntity : Entity
    {
        public Vector2 Velocity;
        public float Mass = 5.0f;
        public float MinSpeed = -10.0f;
        public float MaxSpeed = 10.0f;
        public Vector2 Direction
        {
            get {
                if (Velocity.X == 0 && Velocity.Y == 0)
                    return new Vector2(0, 0);
                return Vector2.Normalize(Velocity);
            }
        }

        public MovingEntity(string tag, float x, float y) : base(tag, x, y){ }

        public void AddForce(Vector2 force)
        {
            force *= 1.2f;
            Vector2 acceleration = (force / Mass);
            Velocity += acceleration;
            
            Velocity = this.TruncateMax(Velocity, (float)MaxSpeed);
            Velocity = this.TruncateMin(Velocity, (float)MinSpeed);

            if(float.IsNaN(Velocity.X))
            {
                Velocity.X = 0.0f;
            }
            if(float.IsNaN(Velocity.Y))
            {
                Velocity.Y = 0.0f;
            }
        }

        public override void Update()
        {
            Position += Velocity;
            if(Position.X > 1280) Position.X = 0;
            if(Position.X < 0) Position.X = 1280;
            if(Position.Y < 0) Position.Y = 720;
            if(Position.Y > 720) Position.Y = 0;
        }

        public Vector2 TruncateMax(Vector2 vector, float maximum)
        {
            if (vector.X == 0 && vector.Y == 0)
                return vector;
            if (vector.Length() > maximum)
            {
                Vector2 truncated = Vector2.Normalize(vector);
                truncated *= maximum;
                return truncated;
            }
            return vector;
        }

        public Vector2 TruncateMin(Vector2 vector, float minimum)
        {
            if (vector.X == 0 && vector.Y == 0)
                return vector;
            if (vector.Length() < minimum)
            {
                Vector2 truncated = Vector2.Normalize(vector);
                truncated *= minimum;
                return truncated;
            }
            return vector;
        }

        public override void Render(Graphics g)
        {
            Vector2 dir = Vector2.Normalize(Direction) * DrawSize;
            g.DrawPolygon(Pens.White, new PointF[]
            {
                new PointF(Position.X + dir.X, Position.Y + dir.Y),
                new PointF(Position.X + dir.Y * 0.5f, Position.Y - dir.X * 0.5f),
                new PointF(Position.X - dir.X, Position.Y - dir.Y),
                new PointF(Position.X - dir.X * 2.0f + dir.Y * 0.5f, Position.Y - dir.Y * 2.0f - dir.X * 0.5f),
                new PointF(Position.X - dir.X * 2.0f - dir.Y * 0.5f, Position.Y - dir.Y * 2.0f + dir.X * 0.5f),
                new PointF(Position.X - dir.X, Position.Y - dir.Y),
                new PointF(Position.X - dir.Y * 0.5f, Position.Y + dir.X * 0.5f),
            });

            if(!ShowDebug) return;
            
            g.DrawString($"{Tag} @ {Position}", BaseFont, Brushes.Lime, Position.X, Position.Y + 10);
            int ypos = 12;
            foreach(KeyValuePair<string, dynamic> kv in Locals)
            {
                g.DrawString($"{kv.Key}: {kv.Value}", BaseFont, Brushes.Yellow, Position.X, Position.Y + 10 + ypos);
                ypos += 12;
            }
            if(Locals.ContainsKey("SeekTarget"))
            {
                MovingEntity entity = (MovingEntity) Locals["SeekTarget"];
                g.DrawLine(Pens.Red, Position.X, Position.Y, entity.Position.X, entity.Position.Y);
            }
        }
    }
}
