using UnityEngine;

public class ZoomingCamera : MonoBehaviour
{
    private Camera _camera;

    public void Start()
    {
        _camera = GetComponent<Camera>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            _camera.orthographicSize = 15;
        }
        else if (Input.GetKey(KeyCode.Minus))
        {
            _camera.orthographicSize = 30;
        }
    }
}
