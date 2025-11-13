using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterName = "Adventurer";
        
        //TODO- add sprites, that sorta thing.
    }
}
