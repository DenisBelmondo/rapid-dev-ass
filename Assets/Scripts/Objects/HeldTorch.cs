using Player;
using UnityEngine;

namespace Objects
{
    public class HeldTorch : HeldItem
    {
        public override void Execute(CharacterInstance character)
        {
            Debug.Log("Used torch!");
        }
    }
}
