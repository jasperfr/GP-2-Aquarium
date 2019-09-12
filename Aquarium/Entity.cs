using System.Numerics;
using System.Drawing;
using System.Collections.Generic;

namespace Aquarium
{
    public class Entity
    {
        public static Font BaseFont = new Font("Arial", 10);

        public float DrawSize = 5.0f;
        public bool ShowDebug = true;
        public Dictionary<string, dynamic> Locals = new Dictionary<string, dynamic>();
        public string Tag;
        public Vector2 Position;

        public Entity(string tag, float x, float y)
        {
            Tag = tag;
            Position = new Vector2(x, y);
        }

        public void SetLocal(string name, dynamic value)
        {
            if(Locals.ContainsKey(name))
            {
                Locals[name] = value;
            }
            else
            {
                Locals.Add(name, value);
            }
        }

        public dynamic GetLocal(string key)
        {
            if(Locals.ContainsKey(key)) {
                return Locals[key];
            }
            return null;
        }

        public void RemoveLocal(string key)
        {
            if(Locals.ContainsKey(key))
            {
                Locals.Remove(key);
            }
        }

        public void SetPosition(float x, float y, bool relative)
        {
            if(relative) {
                Position += new Vector2(x, y);
            }
            else {
                Position = new Vector2(x, y);
            }
        }

        public virtual void Update() { }

        public virtual void Render(Graphics g)
        {
            g.DrawEllipse(Pens.White, Position.X, Position.Y, DrawSize, DrawSize);
            if(!ShowDebug) return;
            
            g.DrawString($"{Tag} @ {Position}", BaseFont, Brushes.Lime, Position.X, Position.Y + 10);
            int ypos = 12;
            foreach(KeyValuePair<string, dynamic> kv in Locals)
            {
                g.DrawString($"{kv.Key}: {kv.Value}", BaseFont, Brushes.Yellow, Position.X, Position.Y + 10 + ypos);
                ypos += 12;
            }
        }
    }
}
