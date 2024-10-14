using UnityEngine;

namespace BinarySpacePartitioning.Data
{
    public class Room
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2Int Origin { get; set; }
        public Vector2 Center { get; private set; }
        public int Size { get; private set; }
        
        // DEBUG
        public Color Color { get; private set; }
        
        public Room(int width, int height, Vector2Int origin)
        {
            Width = width;
            Height = height;
            Origin = origin;
            Center = new Vector2(Origin.x + Width / 2f, Origin.y + Height / 2f);
            Size = Width * Height;
            
            Color = Random.ColorHSV();
        }

        public Room() {}
    }
}
