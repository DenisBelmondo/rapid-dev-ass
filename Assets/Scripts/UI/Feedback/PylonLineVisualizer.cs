using Level;
using Managers;
using Player;
using UnityEngine;

namespace UI.Feedback
{
    [RequireComponent(typeof(LineRenderer))]
    public class PylonLineVisualizer : MonoBehaviour
    {
        private LineRenderer _lines;
        private CrewManager _crewManager;
        private PylonManager _pylonManager;
        void Start()
        {
            _lines = GetComponent<LineRenderer>();
            if(_lines == null) Debug.LogError("No line renderer found in PylonLineVisualizer");
            _crewManager = World.Instance.crewManager;
            _pylonManager = World.Instance.pylonManager;
            
            _pylonManager.onPylonRegistered.AddListener(OnPylonRegistered);
            _pylonManager.onPylonsCleared.AddListener(OnPylonsCleared);
            _pylonManager.onTriangleFormed.AddListener(OnTriangleFormed);
            _pylonManager.onPylonRemoved.AddListener(OnPylonRemoved);

            UpdateLinePositions();
        }

        private void Update()
        {
            if (_pylonManager.activePylons.Count > 0)
            {
                _lines.SetPosition(_lines.positionCount - 1, _crewManager.Leader.transform.position);
            } else if (_lines.positionCount > 0)
            {
                _lines.positionCount = 0;
            }
            
            if (_pylonManager.activePylons.Count == 3)
            {
                _lines.startColor = Color.yellow;
                _lines.endColor = Color.yellow;
            }
            else
            {
                _lines.startColor = Color.white;
                _lines.endColor = Color.white;
            }
        }

        private void UpdateLinePositions()
        {
            if (_pylonManager.activePylons.Count == 0)
            {
                _lines.positionCount = 0;
                return;
            }

            _lines.positionCount = _pylonManager.activePylons.Count + 1;

            for (int i = 0; i < _pylonManager.activePylons.Count; i++)
            {
                _lines.SetPosition(i, _pylonManager.activePylons[i].transform.position);
            }
        }
        
        private void OnPylonRegistered(GameObject pylon)
        {
            UpdateLinePositions();
        }

        private void OnPylonRemoved(GameObject removedPylon)
        {
            UpdateLinePositions();
        }

        private void OnPylonsCleared()
        {
            _lines.positionCount = 0;
        }

        private void OnTriangleFormed(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            _lines.positionCount = 0;
        }
    }
}
