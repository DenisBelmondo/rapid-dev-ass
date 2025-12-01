using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogController : MonoBehaviour
{
    [SerializeField] public Tilemap fogTilemap;
    private float _t;
    private HashSet<Vector2Int> _drawnTiles;
    
    public event Action OnTileRevealed; //For Score Uses.
    

    private IEnumerator RasterizeTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        yield return TriangleRasterization.RasterizeTriangleAsync(new()
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

                fogTilemap.SetTile(new(x, y, 0), tile);
                _drawnTiles.Add(tilePos);
                
                OnTileRevealed?.Invoke();
            },
        },  new(p1.x, p1.y), new(p2.x, p2.y), new(p3.x, p3.y), (TileBase)null);
    }

    public void Start()
    {
        if(fogTilemap == null) Debug.LogError("FogController: fogTilemap is null");
        _drawnTiles = new();
        
    }

    public void Update()
    {
        _t += Time.deltaTime;
    }

    public void OnTriangleFormed(Vector3 p1, Vector3 p2, Vector3 p3) => StartCoroutine(RasterizeTriangle(p1, p2, p3));
}
