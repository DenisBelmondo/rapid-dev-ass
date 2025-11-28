using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogController : MonoBehaviour
{
    [SerializeField] public Tilemap fogTilemap;

    private float _t;
    private HashSet<Vector2Int> _drawnTiles;
    
    //camilo stuff. i am so sorry.
    private ProtoScorer _scorer;

    public void Start()
    {
        if(fogTilemap == null) Debug.LogError("FogController: fogTilemap is null");
        _scorer = FindFirstObjectByType<ProtoScorer>();
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

                fogTilemap.SetTile(new(x, y, 0), tile);
                _drawnTiles.Add(tilePos);
                if (_scorer != null)
                {
                    _scorer.CheckAndRegisterRevealedTile(tilePos);
                }
                
            },
        },  new(p1.x, p1.y), new(p2.x, p2.y), new(p3.x, p3.y), (TileBase)null);
    }
}
