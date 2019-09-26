using System;
using System.Collections.Generic;
using System.Drawing;

namespace Aquarium.Instances
{
    public class Sprite
    {
        public List<Image> Images = new List<Image>();

        public Sprite() { }

        public void add_image(string filename) => Images.Add(Image.FromFile(filename));
        public Image GetSprite(float imageSpeed, ref float index)
        {
            index += imageSpeed;
            if(index > Images.Count) index = 0.0f;
            return Images[(int) Math.Floor(index)];
        }
    }
}
