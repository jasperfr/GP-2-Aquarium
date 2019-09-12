using System.Numerics;

namespace Aquarium
{
    public static class VectorExtensions
    {
        public static Vector2 TruncateMin(this Vector2 vector, float minimum)
        {
            if (vector.X == 0 && vector.Y == 0)
                return vector;
            if (vector.Length() < minimum) {
                Vector2 truncated = Vector2.Normalize(vector);
                truncated *= minimum;
                return truncated;
            }
            return vector;
        }

        public static Vector2 TruncateMax(this Vector2 vector, float maximum)
        {
            if (vector.X == 0 && vector.Y == 0)
                return vector;
            if (vector.Length() > maximum) {
                Vector2 truncated = Vector2.Normalize(vector);
                truncated *= maximum;
                return truncated;
            }
            return vector;
        }
    }
}
