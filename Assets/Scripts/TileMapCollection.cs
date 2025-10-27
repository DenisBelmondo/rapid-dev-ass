using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCollection : MonoBehaviour
{
    [SerializeField]
    public Tilemap BackgroundTilemap;

    [SerializeField]
    public Tilemap OverlayTilemap;

    [SerializeField]
    public TileBase DrawTile;

    private float _t;

    public void Update()
    {
        _t += Time.deltaTime;

        OverlayTilemap.size = BackgroundTilemap.size;

        TriangleRasterization.RasterizeTriangle(new()
        {
            BufferWidth = OverlayTilemap.size.x,
            BufferHeight = OverlayTilemap.size.y,
            Set = (x, y, tile) =>
            {
                OverlayTilemap.SetTile(new(x, y, 0), tile);
            },
        }, new(0 + _t, 0 + _t), new(-24 + _t, -24 + _t), new(24 + _t, -24 + _t), DrawTile);
    }
}
