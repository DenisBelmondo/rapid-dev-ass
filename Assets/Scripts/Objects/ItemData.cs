using UnityEngine;

namespace Objects
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Info")] 
        [SerializeField] public string itemName;

        [SerializeField] public string itemDesc;
        [SerializeField] public Sprite itemSprite;
        
        [Header("Prefabs")]
        [SerializeField] public GameObject worldItemPrefab;
        [SerializeField] public HeldItem heldItemPrefab;
        
    }
}
