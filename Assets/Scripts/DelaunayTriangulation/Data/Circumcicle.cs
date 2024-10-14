using UnityEngine;

namespace DelaunayTriangulation.Data
{
    public class Circumcircle
    {
        private Triangle _triangle;
        private Vertex _center;
        private float _radius;
        
        public Circumcircle(Triangle triangle)
        {
            _triangle = triangle;
            _center = ComputeCenter();
            _radius = ComputeRadius();
        }
        
        public Circumcircle() {}
        
        private Vertex ComputeCenter()
        {
            Vertex v1 = _triangle.A;
            Vertex v2 = _triangle.B;
            Vertex v3 = _triangle.C;

            float cx = ((Mathf.Pow(v1.X, 2) + Mathf.Pow(v1.Y, 2)) * (v2.Y - v3.Y) +
                       (Mathf.Pow(v2.X, 2) + Mathf.Pow(v2.Y, 2)) * (v3.Y - v1.Y) +
                       (Mathf.Pow(v3.X, 2) + Mathf.Pow(v3.Y, 2)) * (v1.Y - v2.Y)) /
                       (2 * (v1.X * (v2.Y - v3.Y) + v2.X * (v3.Y - v1.Y) + v3.X * (v1.Y - v2.Y)));
            float cy = ((Mathf.Pow(v1.X, 2) + Mathf.Pow(v1.Y, 2)) * (v3.X - v2.X) +
                        (Mathf.Pow(v2.X, 2)+ Mathf.Pow(v2.Y, 2)) * (v1.X - v3.X) +
                        (Mathf.Pow(v3.X, 2) + Mathf.Pow(v3.Y, 2)) * (v2.X - v1.X)) /
                        (2 * (v1.X * (v2.Y - v3.Y) + v2.X * (v3.Y - v1.Y) + v3.X * (v1.Y - v2.Y)));
            
            return new Vertex(cx, cy);
        }

        private float ComputeRadius()
        {
            return _center.Distance(_triangle.A);
        }
        
        public bool IsInside(Vertex point)
        {
            return _center.Distance(point) <= _radius;
        }
    }
}
