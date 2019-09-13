using System;
using System.Collections.Generic;
using System.Drawing;

namespace Aquarium
{
    public class Sprite
    {
        public List<Image> Images = new List<Image>();
        public float ImageSpeed = 1.0f;

        public Sprite() { }
        public Sprite(string filename)
        {
            AddImage(filename);
        }
        public Sprite(List<string> filenames)
        {
            filenames.ForEach(file => AddImage(file));
        }

        public void AddImage(string filename) => Images.Add(Image.FromFile(filename));
        public Image GetSprite(ref float index)
        {
            index += ImageSpeed;
            if(index > Images.Count) index = 0.0f;
            return Images[(int) Math.Floor(index)];
        }
    }
}
