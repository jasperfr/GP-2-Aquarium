using NLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Numerics;
using System.Windows.Forms;

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
        private static Font _font = new Font("Arial", 10);

        public Dictionary<string, dynamic> Locals = new Dictionary<string, dynamic>();
        public Action<GameObject> StepEvent { get; set; }           // Step event of the object.
        public Action<int, GameObject> KeyboardEvent { get; set; }  // Keyboard events.
        
        public string Name;
        public string Tag;
        public Vector2 Position;
        public int SpatialX = 0, SpatialY = 0;
        
        public GameObject() : this(""){ }
        public GameObject(string tag)
        {
            Tag = tag;
        }
        
        public bool Has(string key) => Locals.ContainsKey(key);
        public dynamic Get(string key) => Locals.TryGetValue(key, out dynamic value) ? value : null;
        public void Set(string key, dynamic value) => Locals[key] = value;
        public void Remove(string key) => Locals.Remove(key);

        public void FireKeyboardEvent(KeyEventArgs key) => KeyboardEvent?.Invoke(key.KeyValue, this);

        public void Update()
        {   
            /*
            Dictionary<object, object> dict = Program.state.GetTableDict(_);
            foreach(var kv in dict)
            {
                Debug.Log(kv.Key.ToString() + " => " + kv.Value.ToString());
            }
            */

            Position += Velocity;
            StepEvent?.Invoke(this);

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
            g.DrawString($"{Name} ({Tag}) - {Position}", _font, Brushes.Lime, Position.X, Position.Y + ypos);
            ypos += 12;
            g.DrawString($"Mass: {Mass} Size: {Size} Vel: {Velocity} Spd: {Speed}", _font, Brushes.Magenta, Position.X, Position.Y + ypos);
            ypos += 12;
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
                BaseSprite = this.BaseSprite,
                StepEvent = StepEvent,
                KeyboardEvent = KeyboardEvent
            };
            
            
            foreach(var kvp in Locals) {
                duplicate.Set(kvp.Key, kvp.Value);
            }

            return duplicate;
        }
    }
}
