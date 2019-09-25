using System;
using System.Collections.Generic;
using System.Drawing;

namespace Aquarium
{
    public class Sprite
    {
        public List<Image> Images = new List<Image>();

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
        public Image GetSprite(float imageSpeed, ref float index)
        {
            index += imageSpeed;
            if(index > Images.Count) index = 0.0f;
            return Images[(int) Math.Floor(index)];
        }
    }
}
