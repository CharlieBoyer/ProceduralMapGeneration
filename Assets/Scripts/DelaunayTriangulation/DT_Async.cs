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
        private static float _yieldTime = 0f;
        
        private static List<Vertex> _currentSet;
        private static Vertex _currentVertex;

        private static List<Triangle> _triangulation;
        private static Triangle _currentTriangle;
        private static Triangle _superTriangle;
        
        private static bool _showTriangulation = true;
        
        private void OnDrawGizmos()
        {
            if (_currentSet != null || _currentSet?.Count > 0)
            {
                foreach (Vertex point in _currentSet)
                {
                    Gizmos.color = point.Equals(_currentVertex) ? Color.white : Color.black;
                    Gizmos.DrawSphere(_currentVertex.WorldPosition, 0.6f);
                }
            }
            
            // if (_superTriangle != null)
            //     _superTriangle.DrawGizmos(0.4f, Color.magenta, Color.magenta);

            if (_showTriangulation)
                DrawGizmos(_triangulation, 0.4f, Color.red, Color.clear);
            
            if (_currentTriangle != null)
                _currentTriangle.DrawGizmos(0.4f, Color.yellow, Color.grey);
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
            StartCoroutine(BowyerWatsonIncremental(points));
        }

        private static IEnumerator BowyerWatsonIncremental(List<Vertex> points)
        {
            _currentSet = points;
            _triangulation = new();
            _superTriangle = Triangle.SuperTriangle(points);
        
            _triangulation.Add(_superTriangle);

            foreach (Vertex point in points)
            {
                _currentVertex = point;
                List<Triangle> badTriangles = new();
                List<Edge> polygon = new();

                foreach (Triangle triangle in _triangulation)
                {
                    _currentTriangle = triangle;
                    if (triangle.Circumcircle.IsInside(point))
                        badTriangles.Add(triangle);
                    yield return new WaitForSecondsRealtime(_yieldTime);
                }

                foreach (Triangle tri in badTriangles)
                {
                    _currentTriangle = tri;
                    foreach (Edge edge in tri.Edges)
                    {
                        if (!edge.IsSharedWith(badTriangles, tri))
                            polygon.Add(edge);
                        yield return new WaitForSecondsRealtime(_yieldTime);
                    }
                    yield return new WaitForSecondsRealtime(_yieldTime);
                }

                foreach (Triangle tri in badTriangles.ToList())
                {
                    badTriangles.Remove(tri);
                    _triangulation.Remove(tri);
                    yield return new WaitForSecondsRealtime(_yieldTime);
                }

                foreach (Edge edge in polygon)
                {
                    _triangulation.Add(new Triangle(edge, point));
                    yield return new WaitForSecondsRealtime(_yieldTime);
                }
            }


            foreach (Triangle triangle in _triangulation.ToList())
            {
                if (triangle.ContainsSuperTriangleVertex(_superTriangle))
                    _triangulation.Remove(triangle);
                
                yield return new WaitForSecondsRealtime(_yieldTime);
            }

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
