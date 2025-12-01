using System;
using Level;
using Managers;
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
		
		private CrewManager _crewManager;
		private PylonManager _pylonManager;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_spriteRenderer.sprite = characterData.characterSprite;
			_crewManager = World.Instance.crewManager;
			_pylonManager = World.Instance.pylonManager;
		}

		//follow distance should be relative to the leader, so we calculate follow distance in this nifty function.
		public float GetFollowDistance()
		{
			if (_crewManager == null || _crewManager.Leader == null)
			{
				return minFollowDistance;
			}
			
			var leader = _crewManager.Leader;
			float speedRatio = leader.food / 100f;
			
			float dynamicMaxDistance = Mathf.Lerp(minFollowDistance, maxFollowDistance, speedRatio);
			return Mathf.Lerp(dynamicMaxDistance, minFollowDistance, food/100f);
		}

		public void Die()
		{
			Instantiate(characterData.corpsePrefab, transform.position, Quaternion.identity);
			_crewManager.crewMembers.Remove(this);
			OnStatsChanged?.Invoke();
			_pylonManager.ClearPylons();
			Destroy(gameObject);
		}
    }
}
