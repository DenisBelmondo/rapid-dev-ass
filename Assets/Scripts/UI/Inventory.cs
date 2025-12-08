using System.Collections.Generic;
using Objects;
using Player;
using UnityEngine;

namespace UI
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] ItemBox itemPrefab;
        private readonly List<GameObject> _itemBoxes = new();

        public void UpdateInventory(List<CharacterInstance> crew)
        {
            // Clear existing UI items
            foreach (var itemBox in _itemBoxes)
            {
                Destroy(itemBox);
            }
            _itemBoxes.Clear();

            // Rebuild UI items
            for (var i = 0; i < crew.Count; i++)
            {
                if (crew[i].heldItem != null)
                {
                    var newItem = Instantiate(itemPrefab, transform);
                    newItem.Initalize(crew[i].heldItem.itemSprite, i);
                    _itemBoxes.Add(newItem.gameObject);
                }
            }
        }
    }
}
