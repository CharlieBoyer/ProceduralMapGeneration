using System;
using UnityEngine;

namespace DelaunayTriangulation
{
    public readonly struct Vertex : IEquatable<Vertex>
    {
        public readonly float X;
        public readonly float Y;
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
        
        public float Distance(Vertex other)
        {
            return Mathf.Sqrt(Mathf.Pow(X - other.X, 2) + Mathf.Pow(Y - other.Y, 2));
        }

        public static float Distance(Vertex v1, Vertex v2)
        {
            return Mathf.Sqrt(Mathf.Pow(v1.X - v2.X, 2) + Mathf.Pow(v1.Y - v2.Y, 2));
        }

        public override bool Equals(object obj)
        {
            if (obj is Vertex other)
            {
                return Mathf.Approximately(X, other.X) && Mathf.Approximately(Y, other.Y);
            }

            return false;
        }

        public bool Equals(Vertex other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
