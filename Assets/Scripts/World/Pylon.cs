using System;
using Managers;
using UnityEngine;

namespace Objects
{
    public class Pylon : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PylonManager.Instance.OnPylonInteracted(this.gameObject);
            }
        }
    }
}
