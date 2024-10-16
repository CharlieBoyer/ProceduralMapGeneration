using System;
using UnityEngine;

namespace DelaunayTriangulation.Data
{
    public class Vertex
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public Vector3 Position => new Vector3(X, Y, 1);
        
        public Vertex(Vector2 vector)
        {
            X = vector.x;
            Y = vector.y;
        }
        
        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static float Distance(Vertex a, Vertex b)
        {
            return a.Distance(b);
        }
        
        public float Distance(Vertex other)
        {
            return (float) Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
    }
}
