using Managers;
using UnityEngine;

/// <summary>
/// World.cs resolves all dependencies between tiles and the players. 
/// </summary>
public class World : MonoBehaviour
{
    [SerializeField] private FogController FogController;
    [SerializeField] private PylonManager  PylonSystem;

    public void Awake()
    {
        if (FogController == null) Debug.Log("WORLD: FogController is null!");
        if (PylonSystem == null) Debug.Log("WORLD: PylonSystem is null!");
        
    }

    public void Start()
    {
        PylonSystem.OnTriangleFormed.AddListener(FogController.OnTriangleFormed);
    }
}
