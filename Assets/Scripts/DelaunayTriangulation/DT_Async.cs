using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DelaunayTriangulation.Data;
using Utils;

namespace DelaunayTriangulation
{
    public class DT_Async: SingletonMonoBehaviour<DT_Async>
    {
        private static float _yieldTime = .25f;
        
        private static List<Vertex> _currentSet;
        private static Vertex _currentVertex;

        private static List<Triangle> _triangulation;
        private static Triangle _currentTriangle;
        private static Triangle _superTriangle;
        
        private static bool _showTriangulation = true;
        
        private void OnDrawGizmos()
        {
            // if (_superTriangle != null)
            //     _superTriangle.DrawGizmos(0.4f, Color.magenta, Color.magenta);

            if (_showTriangulation)
                DrawGizmos(_triangulation, 0.4f, Color.red, Color.clear);
            
            if (_currentTriangle != null)
                _currentTriangle.DrawGizmos(0.4f, Color.yellow, Color.grey);
            
            if (_currentSet != null || _currentSet?.Count > 0)
            {
                foreach (Vertex point in _currentSet)
                {
                    Gizmos.color = point.Equals(_currentVertex) ? Color.white : Color.black;
                    Gizmos.DrawSphere(_currentVertex.WorldPosition, 0.6f);
                }
            }
        }

        private void OnApplicationQuit()
        {
            _currentSet = null;
            _currentVertex = null;
            _triangulation = null;
            _currentTriangle = null;
            _superTriangle = null;
        }

        public void Triangulate(List<Vertex> points)
        {
            StartCoroutine(BowyerWatsonIncremental_v3(points));
        }

        private static IEnumerator BowyerWatsonIncremental_v1(List<Vertex> points)
        {
            List<Triangle> triangulation = new();
            Triangle superTriangle = Triangle.SuperTriangle(points);
        
            triangulation.Add(superTriangle);

            foreach (Vertex point in points)
            {
                List<Triangle> badTriangles = new();
                List<Edge> polygon = new();

                foreach (Triangle triangle in triangulation.ToList())
                {
                    if (triangle.Circumcircle.IsInside(point))
                        badTriangles.Add(triangle);
                    yield return new WaitForSecondsRealtime(_yieldTime);
                }

                foreach (Triangle tri in badTriangles)
                {
                    foreach (Edge edge in tri.Edges)
                    {
                        if (!edge.IsSharedWith(badTriangles, tri))
                            polygon.Add(edge);
                        yield return new WaitForSecondsRealtime(_yieldTime);
                    }
                }

                foreach (Triangle tri in badTriangles.ToList())
                {
                    badTriangles.Remove(tri);
                    triangulation.Remove(tri);
                }
                
                foreach (Edge edge in polygon)
                    triangulation.Add(new Triangle(edge, point));
            }

            Triangle.RemoveSuperTriangleVertices(triangulation, superTriangle);
        }

        private static IEnumerator BowyerWatsonIncremental_v2(List<Vertex> points)
        {
            _currentSet = points;
            _superTriangle = Triangle.SuperTriangle(points);

            _triangulation = new();
            _triangulation.Add(_superTriangle);
            
            Dictionary<Triangle, bool> trisChecks = new ();
            trisChecks.Add(_superTriangle, false);

            foreach (Vertex point in points.ToList())
            {
                List<Triangle> toRemove = new();
                List<Edge> polygon = new();
                _currentVertex = point;

                foreach (Triangle tri in _triangulation.ToList())
                {
                    _currentTriangle = tri;
                    
                    if (tri.Circumcircle.Center.X < point.X && Mathf.Abs(point.X - tri.Circumcircle.Center.X) < tri.Circumcircle.Radius) // If the triangle is "Delaunay"
                        trisChecks[tri] = true;

                    if (tri.Circumcircle.IsInside(point))
                    {
                        polygon.AddRange(tri.Edges);
                        toRemove.Add(tri);
                        trisChecks.Remove(tri);
                    }

                    yield return new WaitForSecondsRealtime(_yieldTime);
                }

                foreach (Triangle tri in toRemove)
                    _triangulation.Remove(tri);

                foreach (Edge edge in polygon)
                {
                    Vertex a = edge.Start;
                    Vertex b = edge.End;
                    Triangle newTri = new Triangle(a, b, point);
                    
                    _triangulation.Add(newTri);
                    trisChecks.Add(newTri, false);
                }
                
                yield return new WaitForSeconds(_yieldTime);
            }
            
            Triangle.RemoveSuperTriangleVertices(_triangulation, _superTriangle);

            _currentTriangle = null;
            _currentVertex = new Vertex();
            _showTriangulation = true;
        }
        
        private static IEnumerator BowyerWatsonIncremental_v3(List<Vertex> points)
        {
            _currentSet = points;
            _superTriangle = Triangle.SuperTriangle(points);

            _triangulation = new List<Triangle> { _superTriangle };

            foreach (Vertex point in points)
            {
                _currentVertex = point;
                foreach (Triangle tri in _triangulation.ToList())
                {
                    _currentTriangle = tri;
                    if (point.IsInsideCircumcircle(tri))
                    {
                        foreach (Edge edge in tri.Edges)
                        {
                            _triangulation.Add(new Triangle(edge, point));
                            _triangulation.Remove(tri);
                            yield return new WaitForSecondsRealtime(_yieldTime);
                        }
                    }
                }
            }
            
            Triangle.RemoveSuperTriangleVertices(_triangulation, _superTriangle);

            _currentTriangle = null;
            _currentVertex = new Vertex();
            _showTriangulation = true;
        }

        
        public static void DrawGizmos(List<Triangle> triangulation, float vertexSize, Color edgeColor, Color circumcircleColor)
        {
            if (_triangulation == null) return;
            
            foreach (Triangle triangle in triangulation)
            {
                triangle.DrawGizmos(vertexSize, edgeColor, circumcircleColor);
            }
        }
    }
}
