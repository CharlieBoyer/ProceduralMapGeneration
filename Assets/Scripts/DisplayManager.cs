using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using Internal;
using Entities;

public class DisplayManager : SingletonMonoBehaviour<DisplayManager>
{
    [Header("Tile management")]
    [SerializeField] private TileBase _baseTile;

    [Header("BSP Generator")]
    [SerializeField] private int _seed;
    [SerializeField] private BSP.Mode _mode = BSP.Mode.Random;
    [SerializeField] private Slice.Mode _sliceMode;
    [SerializeField] private Slice.Direction _startingDirection;
    [SerializeField] private int _depth;
    [SerializeField] private int _rasterWidth;
    [SerializeField] private int _rasterHeight;
    [SerializeField] private float _minRange = .2f;
    [SerializeField] private float _maxRange = .8f;
    
    private Tilemap _tilemap;
    private List<Room> _currentShownRaster;
    
    // Super triangle draw Test
    private Triangle _superTriangle;
    private List<Vector2> _points = new();
    
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        // GenerateTilemapBase(_rasterWidth, _rasterHeight);
        GenerateTilemapRooms();
    }

    private void OnDrawGizmos()
    {
        if (_currentShownRaster == null) return;

        List<Vector2> points = new();
            
        foreach (Room room in _currentShownRaster)
        {
            Gizmos.DrawSphere(room.Center, 0.2f);
            points.Add(room.Center);
        }
        
        Triangle superTriangle = Triangle.SuperTriangle(points);
        superTriangle.DrawGizmos();
    }

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        GenerateTilemapRooms();
    }
    
    public void GenerateTilemapRooms(List<Room> rooms = null)
    {
        BSP.Init(_seed, _minRange, _maxRange);
        
        List<Room> finalRaster;

        if (rooms == null)
            finalRaster = BSP.Generate(_mode, _rasterWidth, _rasterHeight, _depth, _sliceMode, _startingDirection);
        else
            finalRaster = BSP.Generate(_mode, rooms, _depth, _sliceMode, _startingDirection);
        
        DrawRasterToTilemap(finalRaster);
    }

    private void DrawRasterToTilemap(List<Room> raster)
    {
        Vector2Int rasterSize = BSP.GetRasterSize(raster);

        Vector3Int[] positions = new Vector3Int[rasterSize.x * rasterSize.y];
        TileBase[] tiles = new TileBase[rasterSize.x * rasterSize.y];

        _currentShownRaster = raster;
        
        foreach (Room room in raster)
        {
            for (int indexWidth = 0; indexWidth < room.Width; indexWidth++)
            {
                for (int indexHeight = 0; indexHeight < room.Height; indexHeight++)
                {
                    int index = indexWidth * room.Height + indexHeight;
                    positions[index] = new Vector3Int(room.Origin.x + indexWidth, room.Origin.y + indexHeight, 1);
                    tiles[index] = _baseTile;
                    _tilemap.SetTile(positions[index], tiles[index]);
                    _tilemap.SetTileFlags(positions[index], TileFlags.None);
                    _tilemap.SetColor(positions[index], room.Color);
                }
            }
            
            // _tilemap.SetTiles(positions, tiles); // Should replace individual calls to SetTile()
        }
    }

    private void UpdateTileColors(TileBase[] tiles)
    {
        // Should replace tiles colors in bulk and disable LockTile flag
        throw new System.NotImplementedException();
    }
}