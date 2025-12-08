using System;
using Level;
using Managers;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class CharacterInstance : MonoBehaviour
    {
        [Header("Data")] public CharacterData characterData;

        public static event System.Action OnStatsChanged;
        
        [Header("Runtime Stats")] 
        [Range(1f, 100f)]
        public float food = 100f;

        [Range(1f, 100f)] 
        public float mood = 100f;

		[Header("Follow Settings")]
		public float minFollowDistance = 1.5f;
		public float maxFollowDistance = 5f;

		
		public ItemData heldItem;
		[SerializeField] private Transform itemHoldPoint;
		private GameObject _heldItemVisual;
		
		private SpriteRenderer _spriteRenderer;
		
		private CrewManager _crewManager;
		private PylonManager _pylonManager;
		
		public AudioSource audioSource;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_spriteRenderer.sprite = characterData.characterSprite;
			_crewManager = World.Instance.crewManager;
			_pylonManager = World.Instance.pylonManager;
			audioSource = GetComponent<AudioSource>();
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

		public void AssignItem(ItemData itemData)
		{
			if (heldItem != null)
			{
				Debug.Log("Already holding something!");
				return;
			}
			heldItem = itemData;
			Debug.Log($"{characterData.characterName} received {itemData.itemName}");

			if (itemData.heldItemPrefab != null && itemHoldPoint != null)
			{
				_heldItemVisual = Instantiate(itemData.heldItemPrefab.gameObject, itemHoldPoint);
			}
		}

		public void InstantiateEffect(GameObject effect)
		{
			Instantiate(effect, itemHoldPoint);
		}

		public void DropItem()
		{
			if (heldItem == null) return;
			
			Debug.Log($"{characterData.characterName} dropped {heldItem.itemName}");

			if (heldItem.worldItemPrefab != null)
			{
				Instantiate(heldItem.worldItemPrefab, transform.position, Quaternion.identity);
			}

			ClearItem();
		}

		public void ClearItem()
		{
			if (_heldItemVisual != null)
			{
				Destroy(_heldItemVisual);
			}
			heldItem = null;
		}
		
		public void Die()
		{
			DropItem();
			Instantiate(characterData.corpsePrefab, transform.position, Quaternion.identity);
			_crewManager.crewMembers.Remove(this);
			OnStatsChanged?.Invoke();
			_pylonManager.ClearPylons();
			Destroy(gameObject);
		}
    }
}
