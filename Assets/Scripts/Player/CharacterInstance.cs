using System;
using UnityEngine;

namespace Player
{
    public class CharacterInstance : MonoBehaviour
    {
        [Header("Data")] public CharacterData characterData;

        public static event System.Action OnStatsChanged;
        //TODO- Link this event to any change in food, mood, or deaths.
        
        [Header("Runtime Stats")] 
        [Range(1f, 100f)]
        public float food = 100f;

        [Range(1f, 100f)] 
        public float mood = 100f;

		[Header("Follow Settings")]
		public float minFollowDistance = 1.5f;
		public float maxFollowDistance = 5f;
		
		private SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_spriteRenderer.sprite = characterData.characterSprite;
		}

		//follow distance should be relative to the leader, so we calculate follow distance in this nifty function.
		public float GetFollowDistance()
		{
			var crewManager = CrewManager.Instance;
			if (crewManager == null || crewManager.Leader == null)
			{
				return minFollowDistance;
			}
			
			var leader = crewManager.Leader;
			float speedRatio = leader.food / 100f;
			
			float dynamicMaxDistance = Mathf.Lerp(minFollowDistance, maxFollowDistance, speedRatio);
			return Mathf.Lerp(dynamicMaxDistance, minFollowDistance, food/100f);
		}

		public void Die()
		{
			var crewManager = CrewManager.Instance;
			Instantiate(characterData.corpsePrefab, transform.position, Quaternion.identity);
			crewManager.crewMembers.Remove(this);
			OnStatsChanged?.Invoke();
			Destroy(gameObject);
		}
    }
}
