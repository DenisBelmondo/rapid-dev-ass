using System;
using Level;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Collider2D))]
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (itemData == null) return;

            if (collision.CompareTag("Player"))
            {
                bool pickedUp = World.Instance.crewManager.AssignItemToCrew(itemData);

                if (pickedUp)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
