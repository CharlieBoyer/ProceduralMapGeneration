using UnityEngine;

namespace Entities
{
    public class Room
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Center { get; private set; }
        public int Size { get; private set; }
        
        public Room(int width, int height, Vector2 origin)
        {
            Width = width;
            Height = height;
            Origin = origin;
            Center = new Vector2(Origin.x + Width / 2f, Origin.y + Height / 2f);
        }

        public Room() {}
    }
}
