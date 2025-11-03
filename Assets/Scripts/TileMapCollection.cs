using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCollection : MonoBehaviour
{
    [SerializeField]
    public Tilemap BackgroundTilemap;

    [SerializeField]
    public GameObject DrawTile;

    private float _t;
    private Transform _fogCutterRoot;

    public void Start()
    {
        _fogCutterRoot = transform.Find("FogCutters");
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
                Instantiate(DrawTile, new(x, y, 0), Quaternion.identity, _fogCutterRoot);
            },
        },  new(p1.x, p1.y), new(p2.x, p2.y), new(p3.x, p3.y), DrawTile);
    }
}
