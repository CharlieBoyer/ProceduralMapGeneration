using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace DelaunayTriangulation.Data
{
    public class Triangle
    {
        public readonly Vertex A;
        public readonly Vertex B;
        public readonly Vertex C;

        public readonly Vertex CircumCenter;
        public readonly float CircumRadius;
        
        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
            CircumCenter = CircumcircleCenter();
            CircumRadius = CircumcircleRadius();
        }
        
        public bool IsDelaunay(List<Vertex> points)
        {
            foreach (Vertex point in points)
            {
                if (point == A || point == B || point == C) //
                    continue;                               // Should be superfluous

                if (CircumCenter.Distance(point) < CircumRadius)
                    return false;
            }

            return true;
        }
        
        private Vertex CircumcircleCenter()
        {
            float x1 = A.X, y1 = A.Y;
            float x2 = B.X, y2 = B.Y;
            float x3 = C.X, y3 = C.Y;

            float d = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            float ux = (x1 * x1 + y1 * y1) * (y2 - y3) + (x2 * x2 + y2 * y2) * (y3 - y1) + (x3 * x3 + y3 * y3) * (y1 - y2);
            float uy = (x1 * x1 + y1 * y1) * (x3 - x2) + (x2 * x2 + y2 * y2) * (x1 - x3) + (x3 * x3 + y3 * y3) * (x2 - x1);

            float xc = ux / d;
            float yc = uy / d;

            return new Vertex(xc, yc);
        }

        private float CircumcircleRadius()
        {
            float a = Vertex.Distance(A, B);
            float b = Vertex.Distance(B, C);
            float c = Vertex.Distance(C, A);

            float k = Math.Abs(A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y)) / 2;

            return (a * b * c) / (4 * k);
        }
        
        public void DrawGizmos(float vertexSize, Color edgeColor, Color circumcircleColor)
        {
            Gizmos.color = edgeColor;
            Gizmos.DrawLine(A.Position, B.Position);
            Gizmos.DrawLine(B.Position, C.Position);
            Gizmos.DrawLine(C.Position, A.Position);

            Handles.color = circumcircleColor;
            Gizmos.color = circumcircleColor;
            Handles.DrawWireDisc(CircumCenter.Position, Vector3.forward, CircumRadius);
            Gizmos.DrawSphere(CircumCenter.Position, vertexSize/2f);
            
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(A.Position, vertexSize);
            Gizmos.DrawSphere(B.Position, vertexSize);
            Gizmos.DrawSphere(C.Position, vertexSize);
        }
    }
}
