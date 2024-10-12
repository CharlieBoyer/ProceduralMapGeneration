//
// Delaunay triangulation
// Using the Bowyer-Watson algorithm
//

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunayTriangulation
{
    public static class DT
    {
        public static List<Triangle> Triangulate(List<Vertex> points)
        {
            List<Triangle> triangulation = new();
            Triangle superTriangle = Triangle.SuperTriangle(points);
        
            triangulation.Add(superTriangle);

            foreach (Vertex point in points)
            {
                List<Triangle> badTriangles = new();
                List<Edge> polygon = new();

                foreach (Triangle triangle in triangulation)
                {
                    if (triangle.Circumcircle.IsInside(point))
                        badTriangles.Add(triangle);
                }

                foreach (Triangle tri in badTriangles)
                {
                    foreach (Edge edge in tri.Edges)
                    {
                        if (!edge.IsSharedWith(badTriangles, tri))
                            polygon.Add(edge);
                    }
                }

                foreach (Triangle tri in badTriangles.ToList())
                    badTriangles.Remove(tri);

                foreach (Edge edge in polygon)
                    triangulation.Add(new Triangle(edge, point));
            }


            foreach (Triangle triangle in triangulation.ToList())
            {
                if (triangle.ContainsSuperTriangleVertex(superTriangle))
                    triangulation.Remove(triangle);
            }

            return triangulation;
        }
        
        public static void DrawGizmos(List<Triangle> triangulation, float vertexSize)
        {
            Gizmos.color = Color.red;
            foreach (Triangle triangle in triangulation)
            {
                triangle.DrawGizmos(vertexSize);
            }
        }
    }
}
