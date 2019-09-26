using System.Collections.Generic;

namespace Aquarium.Instances
{
    public static class DictionaryExtensions
    {
        public static Dictionary<K, V> Clone<K, V>(this Dictionary<K, V> cloneable)
        {
            var output = new Dictionary<K, V>();
            foreach(KeyValuePair<K, V> entry in cloneable)
            {
                output.Add(entry.Key, entry.Value);
            }
            return output;
        }
    }

    
}
