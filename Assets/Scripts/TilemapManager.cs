using UnityEngine;
using UnityEngine.Tilemaps;

using Internal;

public class TilemapManager : SingletonMonoBehaviour<TilemapManager>
{
    [SerializeField] private TileBase _baseTile;
    
    private Tilemap _tilemap;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    [ContextMenu("Generate base Tilemap")]
    public void GenerateTilemapBase(int width, int height)
    {
        Vector3Int[] surface = new Vector3Int[width * height];
        TileBase[] tiles = new TileBase[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                surface[x * width + y] = new Vector3Int(x, y, 0);
                tiles[x * width + y] = _baseTile;
            }
        }
        
        _tilemap.SetTiles(surface, tiles);
    }
}
