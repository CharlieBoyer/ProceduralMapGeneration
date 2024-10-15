using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DelaunayTriangulation.Data
{
    public class Triangle
    {
        public readonly Vertex A;
        public readonly Vertex B;
        public readonly Vertex C;
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
        
        public static void RemoveSuperTriangleVertices(List<Triangle> triangulation, Triangle superTriangle)
        {
            foreach (Triangle tri in triangulation.ToList())
            {
                if (tri.A.Equals(superTriangle.A) || tri.A.Equals(superTriangle.B) || tri.A.Equals(superTriangle.C) ||
                    tri.B.Equals(superTriangle.A) || tri.B.Equals(superTriangle.B) || tri.B.Equals(superTriangle.C) ||
                    tri.C.Equals(superTriangle.A) || tri.C.Equals(superTriangle.B) || tri.C.Equals(superTriangle.C))
                {
                    triangulation.Remove(tri);
                }
            }
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

        public void DrawGizmos(float vertexSize, Color edgeColor, Color circumcircleColor)
        {
            Gizmos.color = edgeColor;
            Gizmos.DrawLine(A.WorldPosition, B.WorldPosition);
            Gizmos.DrawLine(B.WorldPosition, C.WorldPosition);
            Gizmos.DrawLine(C.WorldPosition, A.WorldPosition);

            Handles.color = circumcircleColor;
            Gizmos.color = circumcircleColor;
            Handles.DrawWireDisc(Circumcircle.Center.WorldPosition, Vector3.forward, Circumcircle.Radius);
            Gizmos.DrawSphere(Circumcircle.Center.WorldPosition, vertexSize/2f);
            
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(A.WorldPosition, vertexSize);
            Gizmos.DrawSphere(B.WorldPosition, vertexSize);
            Gizmos.DrawSphere(C.WorldPosition, vertexSize);
        }

        public bool IsDelaunay()
        {
            return Circumcircle.IsInside(A) && Circumcircle.IsInside(B) && Circumcircle.IsInside(C);
        }
    }
}
