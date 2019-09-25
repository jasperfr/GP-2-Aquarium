using System.Drawing;
using System.Numerics;

namespace Aquarium.Instances
{
    public class GameInstance : Instance
    {
        public Vector2 position, start, velocity;
        public float mass, min_speed, max_speed;
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
            : base(name, sprite)
        {
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
            this.sprite = sprite;
        }
        #endregion

        #region C#-based functions
        public void AddForce(Vector2 force)
        {
            velocity += (force * 1.2f / mass);

            velocity = velocity.TruncateMin(min_speed);
            velocity = velocity.TruncateMax(max_speed);

            if (float.IsNaN(velocity.X))
            {
                velocity.X = 0.0f;
            }
            if (float.IsNaN(velocity.Y))
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
            Image image = sprite.GetSprite(image_speed, ref image_index);
            g.DrawImage(image, position.X, position.Y, image_size, image_size);
        }
        #endregion
    }
}
