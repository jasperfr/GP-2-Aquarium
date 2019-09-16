using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Aquarium
{
    public class EStateMachine
    {
        public GameObject Entity;
        public EntityState State;
        
        public EStateMachine(EntityState beginState)
        {
            State = beginState;
        }

        public void Start()
        {
            State.StateMachine = this;
            State.Enter?.Invoke(Entity);
        }

        public void SetState(EntityState state)
        {
            State.StateMachine = this;
            State?.Exit?.Invoke(Entity);
            State = state;
            State.StateMachine = this;
            State.Enter?.Invoke(Entity);
        }

        public void UpdateState()
        {
            State.StateMachine = this;
            State?.Execute?.Invoke(Entity);
        }
    }

    public class EntityState
    {
        public EStateMachine StateMachine;
        public Action<GameObject> Enter;
        public Action<GameObject> Execute;
        public Action<GameObject> Exit;
    }

    public class GameObject
    {
        public static Font BaseFont = new Font("Arial", 10);
        public Dictionary<string, dynamic> LocalVariables = new Dictionary<string, dynamic>();
        public List<Action<GameObject>> LocalActions = new List<Action<GameObject>>();
        
        public string Name;
        public string Tag;
        public Vector2 Position;
        public int SpatialX = 0, SpatialY = 0;
        
        public GameObject() : this(""){ }
        public GameObject(string tag)
        {
            Tag = tag;
        }
        
        public void SetLocal(string name, dynamic value) => LocalVariables[name] = value;
        public dynamic GetLocal(string key) => LocalVariables.TryGetValue(key, out dynamic val) ? val : null;
        public bool RemoveLocal(string key) => LocalVariables.ContainsKey(key) ? LocalVariables.Remove(key) : false;
        public void AddAction(Action<GameObject> action) => LocalActions.Add(action);

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

        #region Movement Behaviour functionality
        public Vector2 Velocity;
        public float Mass = 5.0f;
        public float MinSpeed = 1.0f;
        public float MaxSpeed = 10.0f;
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
        public void AddForce(Vector2 force)
        {
            force *= 1.2f;
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
        #endregion

        #region Rendering functionality
        public Sprite BaseSprite;
        public float Size = 5.0f;
        public float ImageIndex = 0.0f;
        public void Render(Graphics g, bool showDebug)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            Image toDraw = BaseSprite.GetSprite(ref ImageIndex);
            g.DrawImage(toDraw, Position.X, Position.Y, Size, Size);
            
            if(!showDebug || Tag != "shark") return;
            
            int ypos = 50;
            g.DrawString($"{Name} ({Tag}) - {Position}", BaseFont, Brushes.Lime, Position.X, Position.Y + ypos);
            ypos += 12;
            g.DrawString($"Mass: {Mass} Size: {Size} Vel: {Velocity} Spd: {Speed}", BaseFont, Brushes.Magenta, Position.X, Position.Y + ypos);
            ypos += 12;
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
        #endregion

        /// <summary>
        /// Clones this GameObject.
        /// ICloneable does not work - Use this function instead.
        /// </summary>
        /// <returns></returns>
        public GameObject Duplicate()
        {
            GameObject duplicate = new GameObject()
            {
                Name = this.Name,
                Tag = this.Tag,
                Size = this.Size,
                Mass = this.Mass,
                MinSpeed = this.MinSpeed,
                MaxSpeed = this.MaxSpeed,
                BaseSprite = this.BaseSprite  
            };

            foreach(KeyValuePair<string, dynamic> kv in this.LocalVariables) {
                duplicate.SetLocal(kv.Key, kv.Value);
            }

            LocalActions.ForEach(action => duplicate.LocalActions.Add(action));

            return duplicate;
        }
    }
}
