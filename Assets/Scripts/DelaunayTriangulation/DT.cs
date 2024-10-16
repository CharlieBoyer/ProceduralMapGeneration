using System.Collections.Generic;
using DelaunayTriangulation.Data;

namespace DelaunayTriangulation
{
    public static class DT
    {
        public static List<Triangle> Triangulate(List<Vertex> points)
        {
            return RawFilterTriangulation(points);
        }
        
        #region Raw-Filter triangulation
        
        private static List<Triangle> RawFilterTriangulation(List<Vertex> points)
        {
            List<Triangle> rawTriangulation = RawTriangulation(points);
            List<Triangle> delaunayTriangulation = new();
            
            foreach (Triangle triangle in rawTriangulation)
            {
                if (triangle.IsDelaunay(points))
                    delaunayTriangulation.Add(triangle);
            }

            return delaunayTriangulation;
        }
        
        private static List<Triangle> RawTriangulation(List<Vertex> points)
        {
            List<Triangle> triangles = new ();
            
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    for (int k = j + 1; k < points.Count; k++)
                    {
                        Triangle triangle = new (points[i], points[j], points[k]);
                        
                        if (!triangles.Contains(triangle))
                            triangles.Add(triangle);
                    }
                }
            }

            return triangles;
        }
        
        #endregion
    }
}
