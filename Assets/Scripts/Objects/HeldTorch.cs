using Player;
using UI.Feedback;
using UnityEngine;

namespace Objects
{
    public class HeldTorch : HeldItem
    {
        [SerializeField] private GameObject torchVignettePrefab;
        [SerializeField] private AudioClip useSound;
        public override void Execute(CharacterInstance character)
        {
            DebugTexter.Instance.UpdateText("Used Torch!", "", Color.cyan);
            character.audioSource.clip = useSound;
            character.audioSource.Play();
            character.InstantiateEffect(torchVignettePrefab);
            //Debug.Log("Used torch!");
            character.ClearItem();
        }
    }
}
