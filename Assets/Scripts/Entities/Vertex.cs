using UnityEngine;

namespace Entities
{
    public struct Vertex
    {
        public float X;
        public float Y;
        public Vector3 WorldPosition => new Vector3(X, Y, 1);
        
        public Vertex(Vector2 positions)
        {
            X = positions.x;
            Y = positions.y;
        }
        
        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
