using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Aquarium
{
    public class GameObject
    {
        // Consts
        public const float ForceModifier = 1.2f;

        // Statics
        public static Font BaseFont = new Font("Arial", 10);

        // Lists
        public Dictionary<string, dynamic> LocalVariables = new Dictionary<string, dynamic>();
        public List<Action<GameObject>> LocalActions = new List<Action<GameObject>>();
        
        // Object variables
        public string Name;
        public string GroupTag;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Mass = 5.0f;
        public float MinSpeed = 1.0f;
        public float MaxSpeed = 10.0f;
        public float Size = 5.0f;

        // Property acessors
        public Vector2 Direction
        {
            get {
                if (Velocity.X == 0 && Velocity.Y == 0)
                    return new Vector2(0, 0);
                return Vector2.Normalize(Velocity);
            }
        }
        public float Speed
        {
            get => Velocity.Length();
        }

        // Constructors
        public GameObject(string name) : this(name, 0, 0){ }
        public GameObject(string name, float xpos, float ypos) : this(name, "", xpos, ypos){ }
        public GameObject(string name, string tag, float xpos, float ypos)
        {
            Name = name;
            GroupTag = tag;
            Position.X = xpos;
            Position.Y = ypos;
        }

        // Getters and setters of local variable list
        public void SetLocal(string name, dynamic value) => LocalVariables[name] = value;
        public dynamic GetLocal(string key) => LocalVariables.TryGetValue(key, out dynamic val) ? val : null;
        public bool RemoveLocal(string key) => LocalVariables.ContainsKey(key) ? LocalVariables.Remove(key) : false;
        
        // Getters and setters of action (step function) list
        public void AddAction(Action<GameObject> action) => LocalActions.Add(action);

        // Object functions
        public void AddForce(Vector2 force)
        {
            force *= ForceModifier;
            Vector2 acceleration = (force / Mass);
            Velocity += acceleration;
            
            Velocity = Velocity.TruncateMin(MinSpeed);
            Velocity = Velocity.TruncateMax(MaxSpeed);

            if(float.IsNaN(Velocity.X)) {
                Velocity.X = 0.0f;
            }
            if(float.IsNaN(Velocity.Y)) {
                Velocity.Y = 0.0f;
            }
        }

        // Global game functions
        public void Update()
        {   
            Position += Velocity;
            LocalActions.ForEach(action => action.Invoke(this));

            // Might have to change this. Loops the room around.
            if(Position.X > 1280) Position.X = 0;
            if(Position.X < 0) Position.X = 1280;
            if(Position.Y < 0) Position.Y = 720;
            if(Position.Y > 720) Position.Y = 0;
        }
        public void Render(Graphics g, bool showDebug)
        {
            Vector2 dir = Vector2.Normalize(Direction) * Size;
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
            
            if(!showDebug) return;

            g.DrawString($"{Name} ({GroupTag}) - {Position}", BaseFont, Brushes.Lime, Position.X, Position.Y + 10);
            g.DrawString($"Mass: {Mass} Size: {Size} Vel: {Velocity} Spd: {Speed}", BaseFont, Brushes.Magenta, Position.X, Position.Y + 22);
            int ypos = 24;
            foreach(KeyValuePair<string, dynamic> kv in LocalVariables)
            {
                g.DrawString($"{kv.Key}: {kv.Value}", BaseFont, Brushes.Yellow, Position.X, Position.Y + 10 + ypos);
                ypos += 12;
            }
            if(LocalVariables.ContainsKey("SeekTarget")) {
                GameObject entity = (GameObject) LocalVariables["SeekTarget"];
                g.DrawLine(Pens.Red, Position.X, Position.Y, entity.Position.X, entity.Position.Y);
            }
        }
    }
}
