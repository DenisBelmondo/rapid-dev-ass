using System.Collections.Generic;
using System.Linq;
using Core;
using Managers;
using UnityEngine;

namespace Player
{
    public class CrewManager : Singleton<CrewManager>
    {
        protected override bool PersistBetweenScenes => false;

        public List<CharacterInstance> crewMembers = new List<CharacterInstance>();
        public CharacterInstance Leader { get; private set; }
        public CharacterMovement LeaderMovement { get; private set; }
        
        public float GroupSpeed { get; private set; }

        [Header("Group Speed Settings")] 
        [SerializeField] private float minGroupSpeed = 0.8f;
        [SerializeField] private float maxGroupSpeed = 5f;

        private void OnEnable()
        {
            CharacterInstance.OnStatsChanged += UpdateCrewOrder;
        }

        private void OnDisable()
        {
            CharacterInstance.OnStatsChanged -= UpdateCrewOrder;
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
        }
        private void UpdateCrewOrder()
        {
            if (crewMembers.Count == 0) return;
            
            var previousLeader = Leader;
            crewMembers = crewMembers.OrderByDescending(guy => guy.food).ToList();
            Leader = crewMembers.First();

            if (Leader != previousLeader)
            {
                LeaderMovement = (Leader != null) ? Leader.GetComponent<CharacterMovement>() : null;
                UpdateFollowTargets();
                PlayerPathManager.Instance.SetLeader(Leader.transform);
            }
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
