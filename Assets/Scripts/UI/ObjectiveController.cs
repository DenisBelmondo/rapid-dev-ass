using UnityEngine;

namespace UI
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.Events;

    public class ObjectiveController : MonoBehaviour
    {
        [Header("Objective Setup")]
        [SerializeField] private Tilemap targetAreaTilemap;

        [SerializeField]
        private string objectiveName = "Map the River";

        [Header("Events")]
        public UnityEvent OnObjectiveCompleted;

        private int _totalTilesInArea;
        private int _revealedTilesCount;

        void Start()
        {
            // Count all the tiles in the target area at the beginning.
            _totalTilesInArea = 0;
            if (targetAreaTilemap != null)
            {
                foreach (var pos in targetAreaTilemap.cellBounds.allPositionsWithin)
                {
                    if (targetAreaTilemap.HasTile(pos))
                    {
                        _totalTilesInArea++;
                    }
                }
                Debug.Log($"'{objectiveName}' started. Total tiles to reveal: {_totalTilesInArea}");
            }
        }
        
        public void CheckAndRegisterRevealedTile(Vector3Int tilePosition)
        {
            if (_revealedTilesCount >= _totalTilesInArea) return; // Already complete

            // Check if the revealed tile is part of our objective area.
            if (targetAreaTilemap != null && targetAreaTilemap.HasTile(tilePosition))
            {
                _revealedTilesCount++;
                Debug.Log($"'{objectiveName}' progress: {_revealedTilesCount}/{_totalTilesInArea}");

                if (_revealedTilesCount >= _totalTilesInArea)
                {
                    Debug.Log($"Objective '{objectiveName}' COMPLETED!");
                    OnObjectiveCompleted?.Invoke();
                }
            }
        }
    }
}
