using Unity.VisualScripting;
using UnityEngine;

namespace Objects
{
    public class SelfDestruct : MonoBehaviour
    {
        [SerializeField] float delayBeforeDestruction = 3.0f; 

        
        void Awake()
        {
            Destroy(gameObject, delayBeforeDestruction);
        }
    }
}
