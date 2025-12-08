using UnityEngine;
using UnityEngine.Serialization;

namespace Objects
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Info")] 
        [SerializeField] public string itemName;
        
        [Header("Prefabs")]
        [SerializeField] public GameObject worldItemPrefab;
        [SerializeField] public HeldItem heldItemPrefab;
        
    }
}
