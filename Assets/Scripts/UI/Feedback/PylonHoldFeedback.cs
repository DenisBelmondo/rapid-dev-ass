using UnityEngine;
using UnityEngine.UI;

namespace UI.Feedback
{
    public class PylonHoldFeedback : MonoBehaviour
    {
        [SerializeField] private Image progressRadial;

        private void Awake()
        {
            if (progressRadial == null)
            {
                Debug.LogError("_progressRadial is null");
                return;
            }

            progressRadial.gameObject.SetActive(false);
            progressRadial.fillAmount = 0;
        }

        public void StartHold()
        {
            progressRadial.gameObject.SetActive(true);
            progressRadial.fillAmount = 0;
        }

        public void UpdateProgress(float progress)
        {
            if (!progressRadial.gameObject.activeInHierarchy)
            {
                progressRadial.gameObject.SetActive(true);
            }
            progressRadial.fillAmount = Mathf.Clamp01(progress);
        }

        public void StopHold()
        {
            progressRadial.fillAmount = 0;
            progressRadial.gameObject.SetActive(false);
        }
    }
}
