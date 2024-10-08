using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;
using Entities;

namespace BSP
{
    public enum Intersect { Vertical, Horizontal }
    
    public class BinarySpacePartition: MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int _rasterWidth;
        [SerializeField] private int _rasterHeight;
        [SerializeField] private int _seed;
        [SerializeField] [Range(0f, 1f)] private float _rangeMin;
        [SerializeField] [Range(0f, 1f)] private float _rangeMax;
        [SerializeField] private int _depth;
        [SerializeField] private Intersect _startingDir;
        
        private Random _generator;
        
        #region Debug
        
        private void OnGUI()
        {
            List<Room> raster = new List<Room>();
            
            if (GUILayout.Button("Start BSD"))
            {
                Init();
                raster = BSD(new List<Room> { new Room(_rasterWidth, _rasterHeight, new Vector2(0, 0)) }, _startingDir, _depth);
                
                Debug.Log("Finished BSP");
                Debug.Log(raster);
            }

            if (raster.Count > 0)
            {
                foreach (var room in raster)
                {
                    DrawRoom(room);
                }
            }
        }

        private void DrawRoom(Room room)
        {
            Vector3 topLeft = new Vector3(room.Origin.x, room.Origin.y, 0);
            Vector3 topRight = new Vector3(room.Origin.x + room.Width, room.Origin.y, 0);
            Vector3 bottomLeft = new Vector3(room.Origin.x, room.Origin.y + room.Height, 0);
            Vector3 bottomRight = new Vector3(room.Origin.x + room.Width, room.Origin.y + room.Height, 0);

            Debug.DrawLine(topLeft, topRight, Color.red);
            Debug.DrawLine(topRight, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red);
            Debug.DrawLine(bottomLeft, topLeft, Color.red);
        }
        
        #endregion

        public void Init()
        {
            _generator = new Random(_seed);
            
            List<Room> raster = new List<Room> { new Room(_rasterWidth, _rasterHeight, new Vector2(0, 0)) };
        }

        private List<Room> BSD(List<Room> raster, Intersect direction, int cutNumber) // cutNumber = depth;
        {
            if (cutNumber <= 0) return raster; // End recursion

            Room selectedRoom = raster[_generator.Next(raster.Count)];
            raster.Remove(selectedRoom);
            
            Slice(raster, selectedRoom, direction);
            BSD(raster, (direction == Intersect.Vertical) ? Intersect.Horizontal : Intersect.Vertical, cutNumber - 1);
            
            return raster;
        }

        private void Slice(List<Room> raster, Room room, Intersect direction)
        {
            Room roomA;
            Room roomB;
            
            if (direction == Intersect.Vertical)
            {
                int cutPoint = _generator.Next(Mathf.FloorToInt(room.Width * _rangeMin), Mathf.FloorToInt(room.Width * _rangeMax) + 1); // Next() max value is exclusive
                
                roomA = new Room(cutPoint, room.Height, room.Origin);
                roomB = new Room(room.Width - cutPoint, room.Height, new Vector2(room.Origin.x + cutPoint, room.Origin.y));
            }
            else
            {
                int cutPoint = _generator.Next(Mathf.FloorToInt(room.Height * _rangeMin), Mathf.FloorToInt(room.Height * _rangeMax) + 1); // Next() max value is exclusive
                
                roomA = new Room(room.Width, cutPoint, room.Origin);
                roomB = new Room(room.Width, room.Height - cutPoint, new Vector2(room.Origin.x, room.Origin.y + cutPoint));
            }
            
            raster.Add(roomA);
            raster.Add(roomB);
        }
    }
}
