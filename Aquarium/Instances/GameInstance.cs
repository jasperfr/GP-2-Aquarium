using System.Drawing;
using System.Numerics;

namespace Aquarium.Instances
{
    public class GameInstance : Instance
    {
        public Vector2 position, start, velocity;
        public int spatial_x, spatial_y;
        public Vector2 direction
        {
            get
            {
                if (velocity.X == 0 && velocity.Y == 0)
                    return new Vector2(0, 0);
                return Vector2.Normalize(velocity);
            }
        }
        public float speed
        {
            get => velocity.Length();
        }

        public GameInstance(string name, Sprite sprite, float x, float y)
        {
            this.sprite_index = sprite;
            this.name = name;
            position = new Vector2(x, y);
            start = new Vector2(x, y);
        }

        #region Lua-based functions
        public void jump_to_start()
        {
            position = new Vector2(start.X, start.Y);
        }
        public void change_sprite(Sprite sprite)
        {
            this.sprite_index = sprite;
        }
        #endregion

        #region C#-based functions
        public void AddForce(Vector2 force)
        {
            velocity += (force * 1.2f / mass);

            velocity = velocity.TruncateMin(min_speed);
            velocity = velocity.TruncateMax(max_speed);

            if (float.IsNaN(velocity.X) || float.IsInfinity(velocity.X))
            {
                velocity.X = 0.0f;
            }
            if (float.IsNaN(velocity.Y) || float.IsInfinity(velocity.Y))
            {
                velocity.Y = 0.0f;
            }
        }
        public void Update()
        {
            position += velocity;
        }
        public void Render(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            var pivot = image_size * 0.5f;
            Image image = sprite_index.GetSprite(image_speed, ref image_index);
            g.DrawImage(image, position.X - pivot, position.Y - pivot, image_size, image_size);
        }
        #endregion
    }
}
