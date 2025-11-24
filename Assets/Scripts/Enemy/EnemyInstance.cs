using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] public EnemyData enemyData;
    
        //this script has two triggers. "Visible" and "Aggro"
        [Header("Triggers")] 
        [SerializeField] private CircleCollider2D visibleCollider;
        [SerializeField] public CircleCollider2D aggroCollider;
        [SerializeField] private CircleCollider2D killCollider;
    
        [Header("Vignette")]
        [SerializeField] private GameObject vignette;
        private List<CharacterInstance> _charactersInVisibleRange = new List<CharacterInstance>();

        private EnemyMovement _enemyMovement;
        private AudioSource _audioSource;
        

        void Start()
        {
            visibleCollider.radius = enemyData.visibleRangeRadius;
            aggroCollider.radius = enemyData.aggroRangeRadius;
            killCollider.radius = enemyData.killRangeRadius;
            
            _enemyMovement = GetComponent<EnemyMovement>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void OnPlayerEnterAggroRange(CharacterInstance character)
        {
            // If this is an alligator, let the AlligatorAI handle targeting logic.
            if (GetComponent<AlligatorAI>() != null)
            {
                return;
            }
            
            //Debug.Log($"{character.name} entered aggro range!");
            if (_enemyMovement.cooldownTimer <= 0)
            {
                _enemyMovement.StartChasing(CrewManager.Instance.GetLastMember());
            }
            
        }

        public void OnPlayerEnterVisibleRange(CharacterInstance character)
        {
            if (!_charactersInVisibleRange.Contains(character))
            {
                _charactersInVisibleRange.Add(character);
                UpdateVignetteState();
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
                UpdateVignetteState();
                //vignette.GetComponent<Animator>().SetTrigger("StartFadeOut");
                //Debug.Log("FADEOUT!!!");
            }
        }
        
        public void OnPlayerEnterKillRange(CharacterInstance character)
        {
            Debug.Log($"OnPlayerEnterKillRange triggered by: {character.name}", this);
            if (_enemyMovement.cooldownTimer <= 0)
            {
                Debug.Log($"Cooldown check passed. Attempting to kill {character.name}.", this);
                character.Die();
                Debug.Log($"character.Die() called. Playing sound and starting cooldown.", this);
                _audioSource.resource = enemyData.killSound;
                _audioSource.Play();
                _enemyMovement.StartCooldown();
            }
            else
            {
                Debug.Log($"Kill attempt failed: Cooldown timer is {_enemyMovement.cooldownTimer}", this);
            }
        }

        private void UpdateVignetteState()
        {
            bool isVisible = _charactersInVisibleRange.Count > 0;
            vignette.GetComponent<Animator>().SetBool("IsVisible", isVisible);
        }
    }
}
