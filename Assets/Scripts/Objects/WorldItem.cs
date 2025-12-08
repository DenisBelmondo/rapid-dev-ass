using System;
using Level;
using UI.Feedback;
using Unity.VisualScripting;
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
                    DebugTexter.Instance.UpdateText($"Picked up {itemData.name}", itemData.itemDesc, Color.cyan);
                    Destroy(gameObject);
                }
            }
        }
    }
}
