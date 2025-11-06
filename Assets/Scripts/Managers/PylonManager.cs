using System.Collections.Generic;
using Core;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PylonManager : Singleton<PylonManager>, IService
    {
        private List<GameObject> _activePylons = new List<GameObject>();
        private HashSet<Vector3> _pylonPositions = new HashSet<Vector3>();

        [SerializeField] private int maxPylons = 3;
        
        public GameObject pylonPrefab;
        
        public UnityEvent<Vector3, Vector3, Vector3> onTriangleFormed = new UnityEvent<Vector3, Vector3, Vector3>();

        protected override void Awake()
        {
            base.Awake();
            ServiceManager.Instance.RegisteredService(this);
            
            DebugTexter.Instance.UpdateText("Find somewhere to place your first pylon!", Color.yellow);
        }

        public void RegisterPylon(Transform playerTransform)
        {
            if (_activePylons.Count >= maxPylons)
            {
                //Debug.Log("Pylon limit reached. Clear the current triangle first!");
                DebugTexter.Instance.UpdateText("Pylon limit reached. Clear the current triangle first!", Color.red);
                return;
            }
            
            Vector3 snappedPos = GetSnappedPosition(playerTransform.position);

            if (_pylonPositions.Contains(snappedPos))
            {
                DebugTexter.Instance.UpdateText("There's already a pylon at this position!", Color.red);
                return;
            }
            
            GameObject newPylon = Instantiate(pylonPrefab, snappedPos, Quaternion.identity);
            
            //make it a diff color if its the first one

            if (_activePylons.Count == 0)
            {
                newPylon.GetComponent<SpriteRenderer>().color = Color.red;
            }
            
            _activePylons.Add(newPylon);
            _pylonPositions.Add(snappedPos);
            Debug.Log($"Pylon {_activePylons.Count} placed at {snappedPos}.");

            if (_activePylons.Count < maxPylons)
            {
                DebugTexter.Instance.UpdateText($"Pylon Placed! {maxPylons - _activePylons.Count} Pylons left!", Color.yellow);
            }
            else
            {
                DebugTexter.Instance.UpdateText("Go back to the first pylon you placed to clear the triangle!", Color.yellow);
            }
            
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
                DebugTexter.Instance.UpdateText("Triangle Cleared! You may place more pylons.", Color.blue);
                
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


        public void Initialize()
        {
            //TODO:Implement this jawn
        }
    }
}
