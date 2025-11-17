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
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_camera.orthographicSize == 15)
            {
                _camera.orthographicSize = 30;
            }
            else
            {
                _camera.orthographicSize = 15;
            }
        }
    }
}
