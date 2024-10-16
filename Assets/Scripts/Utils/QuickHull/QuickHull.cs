using System.Collections.Generic;
using DelaunayTriangulation.Old.Data;
using UnityEngine;

namespace Utils.QuickHull
{
    public static class QuickHull
    {
        private static List<Edge> _convexHull = new();
        
        public static List<Edge> Build(List<Vector2> points)
        {

            Vector2 leftMost = points[0];
            Vector2 rightMost = points[0];
            
            foreach (Vector2 point in points)
            {
                if (point.x < leftMost.x)
                    leftMost = point;
                else if (point.x > rightMost.x)
                    rightMost = point;
            }
            
            _convexHull.Add(new Edge(leftMost, rightMost));
            
            List<Vector2> leftSet = new();
            List<Vector2> rightSet = new();

            foreach (Vector2 point in points)
            {
                if (point.Equals(leftMost) || point.Equals(rightMost)) continue;
                
                if (IsLeft(point, leftMost, rightMost))
                    leftSet.Add(point);
                else
                    rightSet.Add(point);
            }

            return _convexHull;
        }

        private static void FindHull(List<Vector2> set, Vector2 p, Vector2 q)
        {
            if (set.Count <= 0) return;
        }
        
        private static bool IsLeft(Vector2 point, Vector2 leftMost, Vector2 rightMost)
        {
            return ((rightMost.x - leftMost.x) * (point.y - leftMost.y) - (rightMost.y - leftMost.y) * (point.x - leftMost.x)) > 0;
        }
    }
}
