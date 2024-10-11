using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Entities
{
    public class Triangle
    {
        public Vertex A;
        public Vertex B;
        public Vertex C;
        
        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
        }
        public Triangle(Vector2 a, Vector2 b, Vector2 c)
        {
            A = new Vertex(a);
            B = new Vertex(b);
            C = new Vertex(c);
        }
        public Triangle(Triangle copy)
        {
            A = copy.A;
            B = copy.B;
            C = copy.C;
        }
        public Triangle()
        {
            A = new Vertex();
            B = new Vertex();
            C = new Vertex();
        }

        public static Triangle SuperTriangle(List<Vector2> pointsToTriangulate)
        {
            Triangle superTriangle = new Triangle();
            
            // Calculate the bounding box
            float minX = pointsToTriangulate.Min(p => p.x);
            float maxX = pointsToTriangulate.Max(p => p.x);
            float minY = pointsToTriangulate.Min(p => p.y);
            float maxY = pointsToTriangulate.Max(p => p.y);

            // Calculate the margin
            float marginX = (maxX - minX) * 1.1f;
            float marginY = (maxY - minY) * 1.1f;

            // Create the super-triangle vertices
            superTriangle.A = new Vertex(minX - marginX, minY - marginY);
            superTriangle.B = new Vertex(maxX + marginX, minY - marginY);
            superTriangle.C = new Vertex((minX + maxX) / 2, maxY + marginY);
            
            return superTriangle;
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(A.WorldPosition, B.WorldPosition);
            Gizmos.DrawLine(B.WorldPosition, C.WorldPosition);
            Gizmos.DrawLine(C.WorldPosition, A.WorldPosition);
        }
    }
}
