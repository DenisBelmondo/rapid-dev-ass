using Core;
using Managers;
using Player;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    /// <summary>
    /// World.cs resolves all dependencies between tiles and the players. 
    /// </summary>
    public class World : Singleton<World>
    {
        [Header("Systems")]
        [SerializeField] public FogController fogController;
        [SerializeField] public PylonManager  pylonManager;
        [SerializeField] public Inventory inventory;
        [SerializeField] public Boat boat;

        [Header("Tilemaps")] 
        [SerializeField] public Tilemap waterTilemap;
        [SerializeField] public Tilemap targetTilemap; //tilemap for core objectives.
        [SerializeField] public CrewManager crewManager;
        
        

        protected override void Awake()
        {
            base.Awake();
            if (fogController == null) Debug.Log("WORLD: FogController is null!");
            if (pylonManager == null) Debug.Log("WORLD: PylonSystem is null!");
            if (waterTilemap == null) Debug.Log("WORLD: WaterTilemap is null!");
            if (targetTilemap == null) Debug.Log("WORLD: TargetTilemap is null!");
            if (crewManager == null) Debug.Log("WORLD: CrewManager is null!");
            if(inventory == null) Debug.Log("WORLD: Inventory is null!");
            if(boat == null) Debug.Log("WORLD: Boat is null!");
        }

        public void Start()
        {
            pylonManager.onTriangleFormed.AddListener(fogController.OnTriangleFormed);
        }
    }
}
