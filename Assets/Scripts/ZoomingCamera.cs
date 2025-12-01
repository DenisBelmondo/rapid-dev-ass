using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomingCamera : MonoBehaviour
{
    private Camera _camera;
    private InputSystem_Actions _playerInput;

    public void Start()
    {
        _camera = GetComponent<Camera>();
        _playerInput = new InputSystem_Actions();
        _playerInput.Enable();
        _playerInput.Player.Zoom.performed += context => ToggleZoom();
    }
    
    private void ToggleZoom()
    {
        _camera.orthographicSize = Mathf.Approximately(_camera.orthographicSize, 15) ? 30 : 15;
    }
}
