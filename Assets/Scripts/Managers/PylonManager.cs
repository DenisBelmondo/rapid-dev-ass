using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PylonManager : Singleton<PylonManager>
    {
        private List<GameObject> _activePylons = new List<GameObject>();
        private HashSet<Vector3> _pylonPositions = new HashSet<Vector3>();

        [SerializeField] private int maxPylons = 3;
        
        public GameObject pylonPrefab;
        
        public UnityEvent<Vector3, Vector3, Vector3> onTriangleFormed = new UnityEvent<Vector3, Vector3, Vector3>();

        public void RegisterPylon(Transform playerTransform)
        {
            if (_activePylons.Count >= maxPylons)
            {
                Debug.Log("Pylon limit reached. Clear the current triangle first!");
                return;
            }
            
            Vector3 snappedPos = GetSnappedPosition(playerTransform.position);

            if (_pylonPositions.Contains(snappedPos))
            {
                Debug.Log("Pylon already registered.");
                return;
            }
            
            GameObject newPylon = Instantiate(pylonPrefab, snappedPos, Quaternion.identity);
            _activePylons.Add(newPylon);
            _pylonPositions.Add(snappedPos);
            Debug.Log($"Pylon {_activePylons.Count} placed at {snappedPos}.");
        }

        public void OnPylonInteracted(GameObject interactedPylon)
        {
            if (_activePylons.Count != maxPylons)
            {
                Debug.Log($"You need {maxPylons} to make a triangle!");
                return;
            }

            if (interactedPylon == _activePylons[0])
            {
                Debug.Log("Triangle formed!");
                
                onTriangleFormed.Invoke(_activePylons[0].transform.position, _activePylons[1].transform.position, _activePylons[2].transform.position);

                foreach (var pylon in _activePylons)
                {
                    Destroy(pylon.gameObject);
                }
                _activePylons.Clear();
                _pylonPositions.Clear();
            }
            else
            {
                Debug.Log("This isn't the first pylon you placed!");
            }
        }
        
        private Vector3 GetSnappedPosition(Vector3 rawPos)
        {
            float snappedX = Mathf.Floor(rawPos.x);
            float snappedY = Mathf.Floor(rawPos.y);
            float snappedZ = rawPos.z;
            
            return new Vector3(snappedX, snappedY, snappedZ);


        }
        


    }
}
