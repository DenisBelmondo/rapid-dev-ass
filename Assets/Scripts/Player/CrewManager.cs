using System.Collections.Generic;
using System.Linq;
using Core;
using Level;
using UI;
using UnityEngine;
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
                //FindAnyObjectByType<ProtoScorer>().PlayGameOverText();
                Debug.Log("GAME OVER!!");
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
        
        public CharacterInstance GetLastMember()
        {
            return crewMembers.Last();
        }
    }
}
