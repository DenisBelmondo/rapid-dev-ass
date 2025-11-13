using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCollection : MonoBehaviour
{
    [SerializeField]
    public Tilemap BackgroundTilemap;

    [SerializeField]
    public GameObject DrawTile;

    [SerializeField]
    public Tilemap FogTilemap;

    private float _t;
    private HashSet<Vector2Int> _drawnTiles;

    public void Start()
    {
        _drawnTiles = new();
    }

    public void Update()
    {
        _t += Time.deltaTime;
    }

    public void OnTriangleFormed(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        TriangleRasterization.RasterizeTriangle(new()
        {
            BufferWidth = 1000,
            BufferHeight = 1000,
            Set = (x, y, tile) =>
            {
                var tilePos = new Vector2Int(x, y);
                if (_drawnTiles.Contains(tilePos))
                {
                    return;
                }

                FogTilemap.SetTile(new(x, y, 0), tile);
                _drawnTiles.Add(tilePos);
            },
        },  new(p1.x, p1.y), new(p2.x, p2.y), new(p3.x, p3.y), (TileBase)null);
    }
}
