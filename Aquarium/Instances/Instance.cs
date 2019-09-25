using Aquarium.Instances;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Aquarium.Instances
{

    /// <summary>
    /// An Instance contains all basic object data.
    /// </summary>
    public class Instance
    {
        public string name;
        public Sprite sprite;
        public float image_size = 32.0f, image_speed = 1.0f, image_index = 0.0f;
        public Dictionary<string, dynamic> local;

        #region Local values
        public bool has(string key) => local.ContainsKey(key);
        public dynamic get(string key) => local.TryGetValue(key, out dynamic value) ? value : null;
        public void set(string key, dynamic value) => local[key] = value;
        public void remove(string key) => local.Remove(key);
        #endregion

        public Instance(string name, Sprite sprite)
        {
            local = new Dictionary<string, dynamic>();
            this.name = name;
            this.sprite = sprite;
        }

        public GameInstance Create(float x, float y)
        {
            var instance = new GameInstance(name, sprite, x, y);
            instance.local = local.Clone();
            return instance;
        }
    }

    
}
