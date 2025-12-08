using Managers;
using TMPro;
using UI.Feedback;
using UnityEngine;

namespace Level
{
    public class Pylon : MonoBehaviour
    {
        private PylonManager _pylonManager;
        private PylonHoldFeedback _holdFeedback;

        private void Awake()
        {
            _pylonManager = World.Instance.pylonManager;
            _holdFeedback = GetComponent<PylonHoldFeedback>();
        }

        public void UpdateHoldProgress(float progress)
        {
            if (_holdFeedback == null) return;

            if (progress > 0)
            {
                _holdFeedback.UpdateProgress(progress);
            }
            else
            {
                _holdFeedback.StopHold();
            }
        }

        public void CancelHold()
        {
            if (_holdFeedback != null)
            {
                _holdFeedback.StopHold();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            _pylonManager.OnPylonInteracted(this.gameObject);

            bool isTriangleCompletionPylon =
                _pylonManager.activePylons.Count == 3 && _pylonManager.activePylons[0] == gameObject;

            if (isTriangleCompletionPylon) return;
            
            Debug.Log("Can be removed!");
            _pylonManager.pylonToRemove =  this.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_pylonManager.pylonToRemove == this.gameObject)
            {
                _pylonManager.pylonToRemove = null;
            }
        }
    }
}
