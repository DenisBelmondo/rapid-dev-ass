using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] private EnemyData enemyData;
    
        //this script has two triggers. "Visible" and "Aggro"
        [Header("Triggers")] 
        [SerializeField] private CircleCollider2D visibleCollider;
        [SerializeField] private CircleCollider2D aggroCollider;
    
        [Header("Vignette")]
        [SerializeField] private GameObject vignette;
        
        private List<CharacterInstance> _charactersInVisibleRange = new List<CharacterInstance>();
        private Coroutine _vignetteFadeCoroutine;

        void Start()
        {
            visibleCollider.radius = enemyData.visibleRangeRadius;
            aggroCollider.radius = enemyData.aggroRangeRadius;
        }
        
        public void OnPlayerEnterAggroRange(CharacterInstance character)
        {
            //go aggro.   
        }

        public void OnPlayerEnterVisibleRange(CharacterInstance character)
        {
            if (!_charactersInVisibleRange.Contains(character))
            {
                _charactersInVisibleRange.Add(character);
            }

            if (_charactersInVisibleRange.Count == 1)
            {
                vignette.GetComponent<Animator>().SetTrigger("StartFadeIn");
            }
        }

        public void OnPlayerExitVisibleRange(CharacterInstance character)
        {
            if (_charactersInVisibleRange.Contains(character))
            {
                _charactersInVisibleRange.Remove(character);
            }
            if (_charactersInVisibleRange.Count == 0)
            {
                vignette.GetComponent<Animator>().SetTrigger("StartFadeOut");
                Debug.Log("FADEOUT!!!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CharacterInstance>(out var character))
            {
                character.Die();
            }
        }
        
    }
}
