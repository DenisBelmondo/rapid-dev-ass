using System.Collections.Generic;
using System.Linq;
using Level;
using Objects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Player
{
    public class CrewManager : MonoBehaviour
    {
        public List<CharacterInstance> crewMembers = new List<CharacterInstance>();
        public CharacterInstance Leader { get; private set; }
        public CharacterMovement LeaderMovement { get; private set; }
        public float GroupSpeed { get; private set; }

        [Header("Group Speed Settings")] 
        [SerializeField] private float minGroupSpeed = 0.8f;
        [SerializeField] private float maxGroupSpeed = 5f;
        
        [SerializeField] public float waterSpeedMult = 0.5f;
        public bool isAMemberInWater = false;

        [Header("Special Tilemaps")] 
        [SerializeField] public Tilemap waterTilemap;
        
        
        public UnityEvent onGameOver = new();
        public bool isGameOver = false;

        
        //private bool hasMadeFirstMove = false;

        private void OnEnable()
        {
            CharacterInstance.OnStatsChanged += UpdateCrewOrder;
        }

        private void OnDisable()
        {
            CharacterInstance.OnStatsChanged -= UpdateCrewOrder;
        }

        private void Awake()
        {
            waterTilemap = World.Instance.waterTilemap;
        }

        private void Start()
        {
            UpdateCrewOrder();
        }

        private void Update()
        {
            if (Leader != null)
            {
                float speedRatio = Leader.food / 100f;
                GroupSpeed = Mathf.Lerp(minGroupSpeed, maxGroupSpeed, speedRatio);
            }

            //is anyone in water?
            bool foundMemberInWater = false;
            foreach (var member in crewMembers)
            {
                if (member == null) continue;
                
                Vector3Int cellPosition = waterTilemap.WorldToCell(member.transform.position);
                if (waterTilemap.HasTile(cellPosition))
                {
                    foundMemberInWater = true;
                    break; //found one!
                }
            }
            isAMemberInWater = foundMemberInWater;
        }
        private void UpdateCrewOrder()
        {
            if (crewMembers.Count == 0)
            {
                GameOver();
                return;
            }
            
            var previousLeader = Leader;
            crewMembers = crewMembers.OrderByDescending(guy => guy.food).ToList();
            Leader = crewMembers.First();

            if (Leader != previousLeader)
            {
                LeaderMovement = (Leader != null) ? Leader.GetComponent<CharacterMovement>() : null;
                PlayerPathManager.Instance.SetLeader(Leader.transform);
            }
            UpdateFollowTargets();
            if (World.Instance.inventory != null)
            {
                World.Instance.inventory.UpdateInventory(crewMembers);
            }
            //Debug.Log(crewMembers.Count);
        }

        private void UpdateFollowTargets()
        {
            for (int i = 0; i < crewMembers.Count; i++)
            {
                var memberMovement = crewMembers[i].GetComponent<CharacterMovement>();
                if (i == 0)
                {
                    memberMovement.SetFollowTarget(null);
                }
                else
                {
                    memberMovement.SetFollowTarget (crewMembers[i - 1]);
                }
            }
        }

        public bool AssignItemToCrew(ItemData itemData)
        {
            int i = 0;
            foreach (var member in crewMembers)
            {
                if (member.heldItem == null)
                {
                    member.AssignItem(itemData);
                    World.Instance.inventory.UpdateInventory(crewMembers);
                    return true;
                }

                i++;
            }
            Debug.Log("No crew member available to take the item!");
            return false;
        }

        public void UseItem(int memberIndex)
        {
            if (memberIndex < 0 || memberIndex >= crewMembers.Count)
            {
                Debug.Log($"Invalid member index: {memberIndex}");
                return;
            }
            
            CharacterInstance member = crewMembers[memberIndex];
            if (member.heldItem == null)
            {
                Debug.Log($"{member.characterData.characterName} has no item to use.");
                return;
            }
            member.heldItemVisual.GetComponent<HeldItem>().Execute(member);
            //member.ClearItem();
            World.Instance.inventory.UpdateInventory(crewMembers);
        }
        
        private void GameOver()
        {
            if (isGameOver) return;
            isGameOver = true;
            onGameOver.Invoke();
        }
        
        public CharacterInstance GetLastMember()
        {
            return crewMembers.Last();
        }
    }
}
