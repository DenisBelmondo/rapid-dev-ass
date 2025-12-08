using System.Collections;
using System.Collections.Generic;
using Level;
using Managers;
using Player;
using UI.Feedback;
using UnityEngine;

namespace Objects
{
    public class HeldPigeon : HeldItem
    {
        [SerializeField] private AudioClip useSound;
        //[SerializeField] private GameObject pigeonEffect;
        
        public override void Execute(CharacterInstance character)
        {
            var pylonManager = World.Instance.pylonManager;
            if (pylonManager.activePylons.Count == 3)
            {
                character.audioSource.clip = useSound;
                character.audioSource.Play();
                
                var pylons = new List<Vector3>();
                foreach (var pylon in pylonManager.activePylons)
                {
                    pylons.Add(pylon.transform.position);
                }
                pylons.Add(character.transform.position);
                
                pylonManager.onTriangleFormed.Invoke(pylons[0], pylons[1], pylons[2]);
                
                pylonManager.ClearPylons();
                character.ClearItem();
            }
            else
            {
                DebugTexter.Instance.UpdateText("", "You need to place your pylons first!", Color.red);
            }
        }
    }
}