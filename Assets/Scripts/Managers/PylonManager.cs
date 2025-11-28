using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class PylonManager : Singleton<PylonManager>
    {
        protected override bool PersistBetweenScenes => false;

        public List<GameObject> activePylons = new List<GameObject>();
        private readonly HashSet<Vector3> _pylonPositions = new HashSet<Vector3>();

        [SerializeField] private int maxPylons = 3;

        public GameObject pylonPrefab;

        public UnityEvent<Vector3, Vector3, Vector3> OnTriangleFormed = new UnityEvent<Vector3, Vector3, Vector3>();
        public UnityEvent<GameObject> OnPylonRegistered = new();
        public UnityEvent OnPylonsCleared = new UnityEvent();

        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        public void RegisterPylon(Transform playerTransform)
        {
            if (activePylons.Count >= maxPylons)
            {
                return;
            }

            Vector3 snappedPos = GetSnappedPosition(playerTransform.position);

            if (_pylonPositions.Contains(snappedPos))
            {
                //DebugTexter.Instance.UpdateText("There's already a pylon at this position!", Color.red);
                return;
            }

            GameObject newPylon = Instantiate(pylonPrefab, snappedPos, Quaternion.identity);

            //make it a diff color if it's the first one

            if (activePylons.Count == 0)
            {
                newPylon.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            activePylons.Add(newPylon);
            _pylonPositions.Add(snappedPos);
            Debug.Log($"Pylon {activePylons.Count} placed at {snappedPos}.");

            if (activePylons.Count < maxPylons)
            {
                //DebugTexter.Instance.UpdateText($"Pylon Placed! {maxPylons - _activePylons.Count} Pylons left!", Color.yellow);
            }
            else
            {
                //DebugTexter.Instance.UpdateText("Go back to the first pylon you placed to clear the triangle!", Color.yellow);
            }

            OnPylonRegistered.Invoke(newPylon);
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
                //DebugTexter.Instance.UpdateText("Triangle Cleared! You may place more pylons.", Color.blue);

                OnTriangleFormed.Invoke(activePylons[0].transform.position, activePylons[1].transform.position, activePylons[2].transform.position);
                _audioSource.Play();
                
                foreach (var pylon in activePylons)
                {
                    Destroy(pylon.gameObject);
                }
                activePylons.Clear();
                _pylonPositions.Clear();
            }
            else
            {
                Debug.Log("This isn't the first pylon you placed!");
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
            
            OnPylonsCleared.Invoke();
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
