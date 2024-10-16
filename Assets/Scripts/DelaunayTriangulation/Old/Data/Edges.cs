using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunayTriangulation.Old.Data
{
    public readonly struct Edge : IEquatable<Edge>
    {
        public readonly Vertex Start;
        public readonly Vertex End;
        
        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        public Edge(Vector2 a, Vector2 b)
        {
            Start = new Vertex(a);
            End = new Vertex(b);
        }
        
        public bool IsSharedWith(List<Triangle> triangles, Triangle currentTriangle)
        {
            foreach (Triangle triangle in triangles)
            {
                if (triangle.Equals(currentTriangle)) continue; // Skip 'this' triangle owner
                
                foreach (Edge edge in triangle.Edges)
                {
                    if (this.Equals(edge))
                        return true;
                }
            }

            return false;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Edge other)
            {
                return (Start.Equals(other.Start) && End.Equals(other.End)) ||
                       (Start.Equals(other.End) && End.Equals(other.Start));
            }

            return false;
        }

        public bool Equals(Edge other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }
    }
}
