using Aquarium.Instances;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aquarium.Instances
{

    /// <summary>
    /// An Instance contains all basic object data.
    /// </summary>
    public class Instance
    {
        public string name;
        public float image_size = 32.0f, image_speed = 0.0f, image_index = 0.0f;
        public Dictionary<string, dynamic> local;

        public Sprite sprite_index { get; set; }

        public Action<Instance> event_create;
        public Action<Instance> event_step;
        public Action<Instance, KeyEventArgs> event_keyboard;

        #region Local values
        public bool has(string key) => local.ContainsKey(key);
        public dynamic get(string key) => local.TryGetValue(key, out dynamic value) ? value : null;
        public void set(string key, dynamic value) => local[key] = value;
        public void remove(string key) => local.Remove(key);
        #endregion

        public Instance()
        {
            local = new Dictionary<string, dynamic>();
        }

        public GameInstance Create(float x, float y) => new GameInstance(name, sprite_index, x, y) 
        { 
            local = local.Clone(),
            event_create = event_create,
            event_keyboard = event_keyboard,
            event_step = event_step,
            image_index = image_index,
            image_size = image_size,
            image_speed = image_speed
        };
        public void CreateEvent()
        {
            event_create?.Invoke(this);
        }
        public void StepEvent()
        {
            event_step?.Invoke(this);
        }
        public void KeyboardEvent(KeyEventArgs kea)
        {
            event_keyboard?.Invoke(this, kea);
        }
    }
}
