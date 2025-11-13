using UnityEngine;

namespace Player
{
    public class CameraControl : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 0, -10);
        public float smoothTime = 0.1f;
		private Vector3 _velocity = Vector3.zero;

        private void LateUpdate()
        {
            var crewManager = CrewManager.Instance;
            if (crewManager != null && crewManager.Leader != null)
            {
                Vector3 targetPosition = crewManager.Leader.transform.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
            }
        }
    }
}
