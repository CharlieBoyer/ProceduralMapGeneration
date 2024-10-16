using System;
using UnityEngine;

namespace DelaunayTriangulation.Old.Data
{
    public class Vertex : IEquatable<Vertex>
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
        public Vertex() {}
        
        public float Distance(Vertex other)
        {
            return Mathf.Sqrt(Mathf.Pow(X - other.X, 2) + Mathf.Pow(Y - other.Y, 2));
        }
        
        public bool IsInsideCircumcircle(Triangle triangle)
        {
            return triangle.Circumcircle.IsInside(this);
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
            return other != null && X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
