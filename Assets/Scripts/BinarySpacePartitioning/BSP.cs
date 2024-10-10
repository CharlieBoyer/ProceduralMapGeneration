using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;
using Entities;

namespace BinarySpacePartitioning
{
    public enum Intersect { Vertical, Horizontal }
    
    public static class BSP
    {
        public static bool Initialized { get; private set; }
        
        private static Random _generator;
        private static int _seed;
        private static float _rangeMin;
        private static float _rangeMax;

        private static bool HasInternalRaster => _internalRaster != null;
        private static List<Room> _internalRaster;

        public static void Init(int seed, float minRange = .2f, float maxRange = .8f)
        {
            _seed = seed;
            _generator = new Random(_seed);
            _rangeMin = minRange;
            _rangeMax = maxRange;

            Initialized = true;
        }

        public static List<Room> Generate(List<Room> raster, int depth, Intersect startingDir = Intersect.Vertical) // cutNumber = depth;
        {
            if (!Initialized)
                throw new Exception("BSP Generator need to be initialized before starting to Generate()");
            
            if (raster == null || raster.Count == 0)
                throw new Exception("Starting raster is empty or null");
            
            if (depth <= 0) return raster; // End recursion

            Room selectedRoom = raster[_generator.Next(raster.Count)];
            raster.Remove(selectedRoom);
            
            Slice(raster, selectedRoom, startingDir);
            Generate(raster, depth - 1, (startingDir == Intersect.Vertical) ? Intersect.Horizontal : Intersect.Vertical);
            
            return raster;
        }

        public static List<Room> Generate(int rasterWidth, int rasterHeight, int depth, Intersect startingDir = Intersect.Vertical)
        {
            if (!Initialized)
                throw new Exception("BSP Generator need to be initialized before starting to Generate()");

            if (!HasInternalRaster)
                _internalRaster = new List<Room> { new Room(rasterWidth, rasterHeight, new Vector2Int(0, 0)) };
            
            Room selectedRoom = _internalRaster[_generator.Next(_internalRaster.Count)];
            _internalRaster.Remove(selectedRoom);
            
            Slice(_internalRaster, selectedRoom, startingDir);
            Generate(_internalRaster,  depth - 1, (startingDir == Intersect.Vertical) ? Intersect.Horizontal : Intersect.Vertical);

            List<Room> results = new (_internalRaster);
            _internalRaster = null;
            return results;
        }
        
        private static void Slice(List<Room> raster, Room room, Intersect direction)
        {
            Room roomA;
            Room roomB;
            
            if (direction == Intersect.Vertical)
            {
                int cutPoint = _generator.Next(Mathf.FloorToInt(room.Width * _rangeMin), Mathf.FloorToInt(room.Width * _rangeMax) + 1); // Next() max value is exclusive
                
                roomA = new Room(cutPoint, room.Height, room.Origin);
                roomB = new Room(room.Width - cutPoint, room.Height, new Vector2Int(room.Origin.x + cutPoint, room.Origin.y));
            }
            else
            {
                int cutPoint = _generator.Next(Mathf.FloorToInt(room.Height * _rangeMin), Mathf.FloorToInt(room.Height * _rangeMax) + 1); // Next() max value is exclusive
                
                roomA = new Room(room.Width, cutPoint, room.Origin);
                roomB = new Room(room.Width, room.Height - cutPoint, new Vector2Int(room.Origin.x, room.Origin.y + cutPoint));
            }
            
            raster.Add(roomA);
            raster.Add(roomB);
        }

        #region Utils

        public static Vector2Int GetRasterSize(List<Room> raster)
        {
            int width = 0;
            int height = 0;
            
            foreach (Room room in raster)
            {
                int maxX = room.Width + room.Origin.x;
                int maxY = room.Height + room.Origin.y;
                
                if (width < maxX)
                    width = maxX;
                if (height < maxY)
                    height = maxY;
            }

            return new Vector2Int(width, height);
        }

        #endregion
    }
}
