using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunayTriangulation.Data
{
    public class Triangle
    {
        public Vertex A;
        public Vertex B;
        public Vertex C;
        public readonly Circumcircle Circumcircle;
        public readonly List<Edge> Edges;
        
        public static Triangle SuperTriangle(List<Vertex> pointsToTriangulate)
        {
            // Calculate the bounding box
            float minX = pointsToTriangulate.Min(p => p.X);
            float maxX = pointsToTriangulate.Max(p => p.X);
            float minY = pointsToTriangulate.Min(p => p.Y);
            float maxY = pointsToTriangulate.Max(p => p.Y);

            // Calculate the margin
            float marginX = (maxX - minX) * 1.1f;
            float marginY = (maxY - minY) * 1.1f;

            // Create the super-triangle vertices
            Vertex a = new (minX - marginX, minY - marginY);
            Vertex b = new (maxX + marginX, minY - marginY);
            Vertex c = new ((minX + maxX) / 2, maxY + marginY);
            
            return new Triangle(a, b, c);
        }
        
        public bool ContainsSuperTriangleVertex(Triangle superTriangle)
        {
            return A.Equals(superTriangle.A) || A.Equals(superTriangle.B) || A.Equals(superTriangle.C) ||
                   B.Equals(superTriangle.A) || B.Equals(superTriangle.B) || B.Equals(superTriangle.C) ||
                   C.Equals(superTriangle.A) || C.Equals(superTriangle.B) || C.Equals(superTriangle.C);
        }
        
        public Triangle(Vertex a, Vertex b, Vertex toInsert)
        {
            A = a;
            B = b;
            C = toInsert;
            Circumcircle = new Circumcircle(this);
            Edges = new List<Edge>
            {
                new Edge(A, B),
                new Edge(B, C),
                new Edge(C, A)
            };
        }
        public Triangle(Edge edge, Vertex toInsert)
        {
            A = edge.Start;
            B = edge.End;
            C = toInsert;
            Circumcircle = new Circumcircle(this);
            Edges = new List<Edge>
            {
                new Edge(A, B),
                new Edge(B, C),
                new Edge(C, A)
            };
        }
        public Triangle(Triangle copy)
        {
            A = copy.A;
            B = copy.B;
            C = copy.C;
            Circumcircle = new Circumcircle(this);
            Edges = new List<Edge>
            {
                new Edge(A, B),
                new Edge(B, C),
                new Edge(C, A)
            };
        }

        public void DrawGizmos(float vertexSize)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(A.WorldPosition, B.WorldPosition);
            Gizmos.DrawLine(B.WorldPosition, C.WorldPosition);
            Gizmos.DrawLine(C.WorldPosition, A.WorldPosition);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(A.WorldPosition, vertexSize);
            Gizmos.DrawSphere(B.WorldPosition, vertexSize);
            Gizmos.DrawSphere(C.WorldPosition, vertexSize);
        }
    }
}
