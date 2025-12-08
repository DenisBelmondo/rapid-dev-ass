using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PylonManager : MonoBehaviour 
    {
        public List<GameObject> activePylons = new List<GameObject>();
        private HashSet<Vector3> _pylonPositions = new HashSet<Vector3>();

        [SerializeField] private int maxPylons = 3;

        public GameObject pylonPrefab;

        public UnityEvent<Vector3, Vector3, Vector3> onTriangleFormed = new UnityEvent<Vector3, Vector3, Vector3>();
        public UnityEvent<GameObject> onPylonRegistered = new();
        public UnityEvent onPylonsCleared = new UnityEvent();
        public UnityEvent<GameObject> onPylonRemoved = new UnityEvent<GameObject>();

        public GameObject pylonToRemove;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void RegisterPylon(Transform playerTransform)
        {
            if (activePylons.Count >= maxPylons)
            {
                return;
            }

            Vector3 snappedPos = GetSnappedPosition(playerTransform.position);

            if (_pylonPositions.Contains(snappedPos) && pylonToRemove != null)
            {
                Debug.Log("Pylon already at this spot...");
                return;
            }

            GameObject newPylon = Instantiate(pylonPrefab, snappedPos, Quaternion.identity);
            
            if (activePylons.Count == 0)
            {
                newPylon.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            activePylons.Add(newPylon);
            _pylonPositions.Add(snappedPos);
            Debug.Log($"Pylon {activePylons.Count} placed at {snappedPos}.");
            
            onPylonRegistered.Invoke(newPylon);
        }

        public void RemovePylon(GameObject pylon)
        {
            if (!activePylons.Contains(pylon)) return;
            
            Debug.Log($"Pylon at {pylon.transform.position} has been removed.");
            activePylons.Remove(pylon);
            _pylonPositions.Remove(pylon.transform.position);
            
            onPylonRemoved.Invoke(pylon);
            Destroy(pylon);
        }

        public void OnPylonInteracted(GameObject interactedPylon)
        {
            if (activePylons.Count != maxPylons)
            {
                Debug.Log($"You need {maxPylons} to make a triangle!");
                return;
            }

            if (interactedPylon == activePylons[0])
            {
                Debug.Log("Triangle formed!");
                onTriangleFormed.Invoke(activePylons[0].transform.position, activePylons[1].transform.position, activePylons[2].transform.position);
                _audioSource.Play();
                ClearPylons();
            }
        }

        public void ClearPylons()
        {
            foreach (var pylon in activePylons)
            {
                Destroy(pylon.gameObject);
            }
            activePylons.Clear();
            _pylonPositions.Clear();
            
            onPylonsCleared.Invoke();
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
