using Managers;
using UnityEngine;

namespace Level
{
    public class Pylon : MonoBehaviour
    {
        private PylonManager _pylonManager;

        private void Awake()
        {
            _pylonManager = World.Instance.pylonManager;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _pylonManager.OnPylonInteracted(this.gameObject);
            }
        }
    }
}
