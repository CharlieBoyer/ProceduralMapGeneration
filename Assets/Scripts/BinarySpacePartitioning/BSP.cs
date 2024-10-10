using System;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;
using Entities;

namespace BinarySpacePartitioning
{
    public static class BSP
    {
        public enum Mode { Random, Biggest }
        
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

        public static List<Room> Generate(Mode mode, List<Room> raster, int depth, Slice.Mode sliceMode, Slice.Direction sliceDir)
        {
            if (!Initialized)
                throw new Exception("BSP Generator need to be initialized before starting to Generate()");
            
            if (raster == null || raster.Count == 0)
                throw new Exception("Starting raster is empty or null");
            
            if (depth <= 0) return raster; // End recursion

            Room roomToProcess = SelectRoom(mode, raster);
            raster.Remove(roomToProcess);
            SliceRoom(raster, roomToProcess, sliceDir);

            Slice.Direction nextSliceDir = GetNextSliceDirection(sliceMode, sliceDir, roomToProcess);
            Generate(mode, raster, depth - 1, sliceMode, nextSliceDir);
            
            return raster;
        }

        public static List<Room> Generate(Mode mode, int rasterWidth, int rasterHeight, int depth, Slice.Mode sliceMode, Slice.Direction sliceDir)
        {
            if (!Initialized)
                throw new Exception("BSP Generator need to be initialized before starting to Generate()");

            if (!HasInternalRaster)
                _internalRaster = new List<Room> { new Room(rasterWidth, rasterHeight, new Vector2Int(0, 0)) };
            
            Room roomToProcess = SelectRoom(mode, _internalRaster);
            _internalRaster.Remove(roomToProcess);
            SliceRoom(_internalRaster, roomToProcess, sliceDir);
            
            Slice.Direction nextSliceDir = GetNextSliceDirection(sliceMode, sliceDir, roomToProcess);
            Generate(mode, _internalRaster,  depth - 1, sliceMode, nextSliceDir);

            List<Room> results = new (_internalRaster);
            _internalRaster = null;
            return results;
        }
        
        private static void SliceRoom(List<Room> raster, Room roomToSlice, Slice.Direction dir)
        {
            Room roomA;
            Room roomB;
            
            
            if (dir == Slice.Direction.Vertical)
            {
                int cutPoint = _generator.Next(Mathf.FloorToInt(roomToSlice.Width * _rangeMin), Mathf.FloorToInt(roomToSlice.Width * _rangeMax) + 1); // Next() max value is exclusive
                
                roomA = new Room(cutPoint, roomToSlice.Height, roomToSlice.Origin);
                roomB = new Room(roomToSlice.Width - cutPoint, roomToSlice.Height, new Vector2Int(roomToSlice.Origin.x + cutPoint, roomToSlice.Origin.y));
            }
            else
            {
                int cutPoint = _generator.Next(Mathf.FloorToInt(roomToSlice.Height * _rangeMin), Mathf.FloorToInt(roomToSlice.Height * _rangeMax) + 1); // Next() max value is exclusive
                
                roomA = new Room(roomToSlice.Width, cutPoint, roomToSlice.Origin);
                roomB = new Room(roomToSlice.Width, roomToSlice.Height - cutPoint, new Vector2Int(roomToSlice.Origin.x, roomToSlice.Origin.y + cutPoint));
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

        private static Room SelectRoom(BSP.Mode mode, List<Room> raster)
        {
            Room selectedRoom;
            
            switch (mode)
            {
                case Mode.Biggest:
                    selectedRoom = raster[0];
                    foreach (Room room in raster) {
                        if (room.Size > selectedRoom.Size)
                            selectedRoom = room;
                    }
                    break;
                default: // Use Mode.Random
                    selectedRoom = _internalRaster[_generator.Next(_internalRaster.Count)];
                    break;
            }

            return selectedRoom;
        }
        
        private static Slice.Direction GetNextSliceDirection(Slice.Mode mode, Slice.Direction current, Room selectedRoom)
        {
            switch (mode)
            {
               case Slice.Mode.Perpendicular:
                   return selectedRoom.Width > selectedRoom.Height ? Slice.Direction.Vertical : Slice.Direction.Horizontal;
               default: // Slice.Mode.Alternate
                   return (current == Slice.Direction.Vertical) ? Slice.Direction.Horizontal : Slice.Direction.Vertical;
            }
        }
        
        #endregion
    }
}
